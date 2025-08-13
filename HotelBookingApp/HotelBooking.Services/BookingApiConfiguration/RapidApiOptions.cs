namespace HotelBooking.Services.BookingApiConfiguration;

public class RapidApiOptions
{
    public const string SectionName = "RapidApi";

    public string Key { get; set; } = string.Empty;
    public string Host { get; set; } = "booking-com.p.rapidapi.com";
    public string BaseUrl { get; set; } = "https://booking-com.p.rapidapi.com/";
    
    public string Locale { get; set; } = "en-us";
    public string Currency { get; set; } = "USD";
}