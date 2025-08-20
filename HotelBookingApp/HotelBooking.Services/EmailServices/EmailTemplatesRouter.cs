namespace HotelBooking.Services.EmailServices;

public static class EmailTemplatesRouter
{
    private static readonly string BasePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "HotelBooking.Services", "EmailServices", "EmailTemplates"));

    public static string CheckoutBookingsEmailTemplatePath => Path.Combine(BasePath, "CheckoutBookingsEmailTemplate.html");
    
    public static string RegisterAccountEmailTemplatePath => Path.Combine(BasePath, "RegisterAccountEmailTemplate.html");
}