using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Services.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HotelBooking.Services.Contracts.BookingApiConfigurationContracts;

namespace HotelBooking.Services.BookingApiConfiguration
{
    public class ApiService : IApiService
    {
        private const string LocationsUrl = "v1/hotels/locations";
        private const string SearchUrl = "v1/hotels/search";
        

        public async Task<List<Hotel>?> GetHotelsByLocation(HttpClient httpClient,ApiDataViewModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (string.IsNullOrWhiteSpace(model.City)) return new List<Hotel>();
            if (model.CheckinDate > model.CheckoutDate)
                throw new ArgumentException("Check-in date must be on or before check-out date.");


            var city = Uri.EscapeDataString(model.City.Trim());
            var locationsUri = $"{LocationsUrl}?name={city}&locale=en-us";

            using (var response = await httpClient.GetAsync(locationsUri))
            {
                response.EnsureSuccessStatusCode();
                var responseJson = await response.Content.ReadAsStringAsync();
                var locations = JsonConvert.DeserializeObject<List<LocationId>>(responseJson) ?? new List<LocationId>();

                var destIds = locations
                    .Where(x => !string.IsNullOrWhiteSpace(x.Dest_id))
                    .Select(x => x.Dest_id)
                    .Distinct()
                    .ToList();

                if (destIds.Count == 0) return new List<Hotel>();

                string formattedCheckinDate = model.CheckinDate.ToString("yyyy-MM-dd");
                string formattedCheckoutDate = model.CheckoutDate.ToString("yyyy-MM-dd");

                var baseQuery = new StringBuilder()
                    .Append("?order_by=price")
                    .Append($"&adults_number={model.AdultsNumber}")
                    .Append($"&checkin_date={formattedCheckinDate}")
                    .Append("&filter_by_currency=BGN")
                    .Append("&locale=en-us")
                    .Append($"&checkout_date={formattedCheckoutDate}")
                    .Append("&units=metric")
                    .Append($"&room_number={model.RoomsNumber}")
                    .Append("&dest_type=city");

                if (model.ChildrenNumber > 0)
                {
                    baseQuery.Append($"&children_number={model.ChildrenNumber}");
                }

                var results = new List<Hotel>();

                foreach (var id in destIds)
                {
                    var searchUri = $"{SearchUrl}{baseQuery}&dest_id={Uri.EscapeDataString(id)}";

                    using var searchResponse = await httpClient.GetAsync(searchUri);
                    if (!searchResponse.IsSuccessStatusCode) continue;

                    var json = await searchResponse.Content.ReadAsStringAsync();
                    results.AddRange(ParseHotels(json, model.MinPrice, model.MaxPrice, formattedCheckinDate, formattedCheckoutDate));
                }
                
                return results;
            }
        }

        private static IEnumerable<Hotel> ParseHotels(
            string json,
            double minPrice,
            double maxPrice,
            string startDate,
            string endDate)
        {
            if (string.IsNullOrWhiteSpace(json)) yield break;

            var token = JToken.Parse(json);
            var result = token["result"] as JArray;
            if (result == null || result.Count == 0) yield break;

            foreach (var h in result)
            {
                var hotelName = h.Value<string>("hotel_name");
                var photoUrl = h.Value<string>("main_photo_url");
                var price = h.Value<double?>("min_total_price");

                if (string.IsNullOrWhiteSpace(hotelName) || string.IsNullOrWhiteSpace(photoUrl)) continue;
                if (!price.HasValue) continue;

                var p = price.Value;
                if (!(p > minPrice && p <= maxPrice)) continue;

                yield return new Hotel
                {
                    Name = hotelName,
                    PhotoMainUrl = photoUrl,
                    ReviewScore = h.Value<double?>("review_score"),
                    ReviewScoreWord = h.Value<string>("review_score_word"),
                    Price = p,
                    ReviewsCount = h.Value<int?>("review_nr"),
                    StartAt = startDate,
                    EndAt = endDate,
                };
            }
        }

    }
}
