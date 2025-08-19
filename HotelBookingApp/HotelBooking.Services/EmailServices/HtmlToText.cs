namespace HotelBooking.Services.EmailServices;

static class HtmlToText
{
    public static string Simple(string html)
    {
        var noTags = System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", " ");
        return System.Text.RegularExpressions.Regex.Replace(noTags, "\\s+", " ").Trim();
    }
}