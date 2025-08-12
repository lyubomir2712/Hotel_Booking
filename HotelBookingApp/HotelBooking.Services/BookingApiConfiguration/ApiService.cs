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
        private const string LocationsUrl = "https://booking-com.p.rapidapi.com/v1/hotels/locations";
        private const string SearchUrl = "https://booking-com.p.rapidapi.com/v1/hotels/search";
        private const string RapidApiHost = "booking-com.p.rapidapi.com";
        private const string RapidApiKey = "b067df4ec3msh7add19d4e4747fbp12bc39jsna54fc447bbf1";

        // Reuse a single HttpClient instance to avoid socket exhaustion
        private static readonly HttpClient Http = new HttpClient();

        public async Task<List<Hotel>?> GetHotelsByLocation(ApiDataViewModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (string.IsNullOrWhiteSpace(model.City)) return new List<Hotel>();
            if (model.CheckinDate > model.CheckoutDate)
                throw new ArgumentException("Check-in date must be on or before check-out date.");

            EnsureHeaders();

            var city = Uri.EscapeDataString(model.City.Trim());
            var locationsUri = $"{LocationsUrl}?name={city}&locale=en-us";

            using (var response = await Http.GetAsync(locationsUri).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
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
                    .Append("&filter_by_currency=USD")
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

                    using var searchResponse = await Http.GetAsync(searchUri).ConfigureAwait(false);
                    if (!searchResponse.IsSuccessStatusCode) continue;

                    var json = await searchResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
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

        private static void EnsureHeaders()
        {
            // Avoid duplicate headers if method is called multiple times
            if (!Http.DefaultRequestHeaders.Contains("X-RapidAPI-Key"))
            {
                Http.DefaultRequestHeaders.Add("X-RapidAPI-Key", RapidApiKey);
            }

            if (!Http.DefaultRequestHeaders.Contains("X-RapidAPI-Host"))
            {
                Http.DefaultRequestHeaders.Add("X-RapidAPI-Host", RapidApiHost);
            }
        }
    }
}
