using System.Text;
using HotelBooking.Data.Migrations;
using HotelBooking.Models.AppModels;
using HotelBooking.Services.Contracts.HotelsServicesContracts;
using HotelBooking.Services.ViewModels;
using Newtonsoft.Json;

namespace HotelBooking.Services.BookingApiConfiguration;

public class GetBookingRecommendationsService : IGetBookingRecommendationsService
{
    private const string SearchUrl = "v1/hotels/search";
    private readonly HttpClient httpClient;

    public GetBookingRecommendationsService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    
    public async Task<List<BookingViewModel>> GetBookingRecomendations(List<BookingModel> unavailableBookings)
    {
        var recommendedBookings = new List<BookingViewModel>();
        
        foreach (var unavailableBooking in unavailableBookings)
        {
            string formattedCheckinDate = unavailableBooking.StartAt.ToString("yyyy-MM-dd");
            string formattedCheckoutDate = unavailableBooking.EndAt.ToString("yyyy-MM-dd");

            var baseQuery = new StringBuilder()
                .Append("?order_by=price")
                .Append($"&adults_number={unavailableBooking.AdultsNumber}")
                .Append($"&checkin_date={formattedCheckinDate}")
                .Append("&filter_by_currency=BGN")
                .Append("&locale=en-us")
                .Append($"&checkout_date={formattedCheckoutDate}")
                .Append("&units=metric")
                .Append($"&room_number={unavailableBooking.RoomsNumber}")
                .Append("&dest_type=city");

            if (unavailableBooking.ChildrenNumber > 0)
            {
                baseQuery.Append($"&children_number={unavailableBooking.ChildrenNumber}");
            }

            var searchUri = $"{SearchUrl}{baseQuery}&dest_id={Uri.EscapeDataString(unavailableBooking.DestinationId)}";

            using var searchResponse = await httpClient.GetAsync(searchUri);
            if (!searchResponse.IsSuccessStatusCode) continue;

            var json = await searchResponse.Content.ReadAsStringAsync();

            var dto = JsonConvert.DeserializeObject<BookingSearchJsonResponse>(json);
            
            if (dto?.Result != null && dto.Result.Count > 0)
            {
                var dtoBooking = dto.Result
                    .FirstOrDefault(b =>
                        !string.IsNullOrEmpty(b.HotelName) &&
                        !string.IsNullOrEmpty(b.MainPhotoUrl) &&
                        b.MinTotalPrice.HasValue &&
                        b.MinTotalPrice.Value >= unavailableBooking.Price - 100 &&
                        b.MinTotalPrice.Value <= unavailableBooking.Price + 100);
                
                if (dtoBooking != null)
                {
                    var recommendedBooking = dtoBooking.MapBooking(formattedCheckinDate, formattedCheckoutDate,
                        unavailableBooking.AdultsNumber, unavailableBooking.ChildrenNumber,
                        unavailableBooking.RoomsNumber, unavailableBooking.DestinationId);
                    recommendedBookings.Add(recommendedBooking);
                }
            }
            
        }

        return recommendedBookings;
    }
}
