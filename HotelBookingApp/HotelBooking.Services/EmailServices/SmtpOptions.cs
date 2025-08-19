namespace HotelBooking.Services.EmailServices;

public sealed class SmtpOptions
{
    public string Host { get; init; } = "";
    public int Port { get; init; } = 587;
    public bool UseStartTls { get; init; } = true;
    public string User { get; init; } = "";
    public string Password { get; init; } = "";
    public string FromName { get; init; } = "";
    public string FromAddress { get; init; } = "";
}

