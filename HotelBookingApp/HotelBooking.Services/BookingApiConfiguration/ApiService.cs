using System.Text;
using HotelBooking.Services.ViewModels;
using Newtonsoft.Json;
using HotelBooking.Services.Contracts.BookingApiConfigurationContracts;

namespace HotelBooking.Services.BookingApiConfiguration
{
    public class ApiService : IApiService
    {
        private const string LocationsUrl = "v1/hotels/locations";
        private const string SearchUrl = "v1/hotels/search";
        private readonly HttpClient httpClient;

        public ApiService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<BookingViewModel>?> GetHotelsByLocation( ApiDataViewModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (string.IsNullOrWhiteSpace(model.City)) return new List<BookingViewModel>();
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

                if (destIds.Count == 0) return new List<BookingViewModel>();

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

                var results = new List<BookingViewModel>();

                foreach (var destinationId in destIds)
                {
                    var searchUri = $"{SearchUrl}{baseQuery}&dest_id={Uri.EscapeDataString(destinationId)}";

                    using var searchResponse = await httpClient.GetAsync(searchUri);
                    if (!searchResponse.IsSuccessStatusCode) continue;

                    var json = await searchResponse.Content.ReadAsStringAsync();
                   
                    var dto = JsonConvert.DeserializeObject<BookingSearchJsonResponse>(json);
                    if (dto?.Result != null && dto.Result.Count > 0)
                    {
                        results.AddRange(dto.Result.MapBookings(formattedCheckinDate, formattedCheckoutDate,
                            model.AdultsNumber, model.ChildrenNumber, model.RoomsNumber, destinationId));
                    }
                }
                
                return results;
            }
        }
    }
}
