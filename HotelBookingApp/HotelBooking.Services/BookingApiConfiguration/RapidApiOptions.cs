using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Services.BookingApiConfiguration;

public class RapidApiOptions
{
    public const string SectionName = "RapidApi";

    [Required] public string Key { get; init; } = default!;
    [Required] public string Host { get; init; } = default!;
    [Required, Url]public string BaseUrl { get; init; } = default!;
    
    public string Locale { get; init; } = "en-us";
    public string Currency { get; init; } = "EUR";
}