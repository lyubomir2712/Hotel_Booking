using HotelBooking.Models.AppModels;
using HotelBooking.Services.Contracts.BookingApiConfigurationContracts;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace HotelBooking.Services.BookingApiConfiguration;

public class GetUnavailableBookingsService : IGetUnavailableBookingsService
{
    private const string RoomAvailabilityUrl = "/v1/hotels/room-list";
    private readonly IConfiguration _configuration;
    
    public GetUnavailableBookingsService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<List<BookingModel>> VerifyBookingsForAvailability(HttpClient httpClient, List<BookingModel> bookings)
    {
        var unavailableBookings = new List<BookingModel>();

        foreach(var booking in bookings)
        {

            if (booking.RoomsNumber > booking.AdultsNumber)
            {
                throw new ArgumentException(
                    "Cannot have more rooms than adults. Each room requires at least one adult.");
            }

            var (adultsPerRoom, childrenPerRoom) = DistributeGuestsAcrossRooms(
                booking.AdultsNumber,
                booking.ChildrenNumber ?? 0,
                booking.RoomsNumber
            );

            string childrenAges = GenerateChildrenAges(booking.ChildrenNumber ?? 0);

            string locale = _configuration["RapidApi:Locale"] ?? "en-us";
            string currency = _configuration["RapidApi:Currency"] ?? "BGN";


            var urlBuilder = new StringBuilder(RoomAvailabilityUrl);
            urlBuilder.Append($"?checkin_date={booking.StartAt:yyyy-MM-dd}");
            urlBuilder.Append($"&checkout_date={booking.EndAt:yyyy-MM-dd}");
            urlBuilder.Append($"&locale={locale}");
            urlBuilder.Append("&units=metric");
            urlBuilder.Append($"&adults_number_by_rooms={adultsPerRoom}");
            urlBuilder.Append($"&currency={currency}");
            urlBuilder.Append($"&children_number_by_rooms={childrenPerRoom}");

            if (!string.IsNullOrEmpty(childrenAges))
            {
                urlBuilder.Append($"&children_ages={childrenAges}");
            }

            urlBuilder.Append($"&hotel_id={booking.RapidApiHotelId}");

            var roomsAvailableUri = urlBuilder.ToString();

            using var roomsAvailableResponse = await httpClient.GetAsync(roomsAvailableUri);

            if (!roomsAvailableResponse.IsSuccessStatusCode)
            {
                unavailableBookings.Add(booking);
                continue;
            }
            
            var json = await roomsAvailableResponse.Content.ReadAsStringAsync();
            
            var data = JArray.Parse(json);
            
            var totalBlocks = data[0]?["total_blocks"]?.Value<int>();

            if (!totalBlocks.HasValue || totalBlocks.Value < booking.RoomsNumber)
            {
                unavailableBookings.Add(booking);
            }
            
            
        }
        return unavailableBookings; 
    }

    private (string adultsPerRoom, string childrenPerRoom) DistributeGuestsAcrossRooms(
        int totalAdults, 
        int totalChildren, 
        int numberOfRooms)
    {
        int adultsPerRoom = totalAdults / numberOfRooms;
        int extraAdults = totalAdults % numberOfRooms;
        
        int childrenPerRoom = totalChildren / numberOfRooms;
        int extraChildren = totalChildren % numberOfRooms;
        
        var adultsDistribution = new List<int>();
        var childrenDistribution = new List<int>();
        
        for (int i = 0; i < numberOfRooms; i++)
        {
            adultsDistribution.Add(adultsPerRoom + (i < extraAdults ? 1 : 0));
            
            childrenDistribution.Add(childrenPerRoom + (i < extraChildren ? 1 : 0));
        }
        
        string adultsResult = string.Join(",", adultsDistribution);
        string childrenResult = string.Join(",", childrenDistribution);
        
        return (adultsResult, childrenResult);
    }

    private string GenerateChildrenAges(int totalChildren)
    {
        if (totalChildren == 0)
        {
            return string.Empty;
        }

        var ages = Enumerable.Repeat(10, totalChildren);
        return string.Join(",", ages);
    }
}