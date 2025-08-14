using HotelBooking.Services.ViewModels;

namespace HotelBooking.Services.BookingApiConfiguration;

internal static class BookingHotelMapper
{
    internal static IEnumerable<Hotel> MapHotels(this 
        IEnumerable<BookingHotelJsonDto>? items,
        double minPrice,
        double maxPrice,
        string startDate,
        string endDate)
    {
        if (items == null) yield break;

        foreach (var h in items)
        {
            var hotelName = h.HotelName;
            var photoUrl = h.MainPhotoUrl;
            var price = h.MinTotalPrice;

            if (string.IsNullOrWhiteSpace(hotelName) || string.IsNullOrWhiteSpace(photoUrl)) continue;
            if (!price.HasValue) continue;

            var p = price.Value;
            if (!(p > minPrice && p <= maxPrice)) continue;

            yield return new Hotel
            {
                Name = hotelName,
                PhotoMainUrl = photoUrl,
                ReviewScore = h.ReviewScore,
                ReviewScoreWord = h.ReviewScoreWord,
                Price = p,
                ReviewsCount = h.ReviewCount,
                StartAt = startDate,
                EndAt = endDate,
            };
        }
    }
}