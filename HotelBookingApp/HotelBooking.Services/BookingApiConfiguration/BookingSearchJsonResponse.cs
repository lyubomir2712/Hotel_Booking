using Newtonsoft.Json;

namespace HotelBooking.Services.BookingApiConfiguration;

internal class BookingSearchJsonResponse
{
    [JsonProperty("result")]
    public List<BookingHotelJsonDto>? Result { get; set; }
}