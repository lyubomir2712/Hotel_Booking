namespace HotelBooking.Services.BookingApiConfiguration;

public class RapidApiOptions
{
    public const string SectionName = "RapidApi";

    public string Key { get; } = "b067df4ec3msh7add19d4e4747fbp12bc39jsna54fc447bbf1";
    public string Host { get; } = "booking-com.p.rapidapi.com";
    public string BaseUrl { get; } = "https://booking-com.p.rapidapi.com/";
    
    public string Locale { get; } = "en-us";
    public string Currency { get; } = "BGN";
}