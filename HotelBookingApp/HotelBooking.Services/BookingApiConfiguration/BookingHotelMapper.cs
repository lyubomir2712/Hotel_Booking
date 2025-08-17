using System.Text.Json;
using HotelBooking.Services.ViewModels;

namespace HotelBooking.Services.BookingApiConfiguration;

internal static class BookingHotelMapper
{
    internal static IEnumerable<BookingViewModel> MapHotels(this 
        IEnumerable<BookingHotelJsonDto>? items,
        double minPrice,
        double maxPrice,
        string startDate,
        string endDate,
        int adultsNumber,
        int childrenNumber)
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

            yield return new BookingViewModel
            {
                HotelId = h.HotelId,
                Name = hotelName,
                PhotoMainUrl = photoUrl,
                ReviewScore = h.ReviewScore,
                ReviewScoreWord = h.ReviewScoreWord,
                Price = p,
                ReviewsCount = h.ReviewCount,
                StartAt = startDate,
                EndAt = endDate,
                AdultsNumber = adultsNumber,
                ChildrenNumber = childrenNumber,
                Country = h.Country,
                City = h.City,
                Address = h.Address,
                Latitude = h.Latitude,
                Longitude = h.Longitude,
                DistanceToCenter = h.DistanceToCenter,
                IsFreeCancellable = h.IsFreeCancellable,
                IsNoPrepayment = h.IsNoPrepayment,
                IncludesBreakfast = h.IncludesBreakfast,
                HasFreeParking = h.HasFreeParking,
                AccommodationType = h.AccommodationType,
                IsBeachFront = h.IsBeachFront,
                IsPreferred = h.IsPreferred,
                IsInBestDistrict = h.IsPreferred
            };
        }
    }
}