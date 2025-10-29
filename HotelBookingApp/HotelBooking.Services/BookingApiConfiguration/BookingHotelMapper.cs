using System.Text.Json;
using HotelBooking.Services.ViewModels;

namespace HotelBooking.Services.BookingApiConfiguration;

internal static class BookingHotelMapper
{
    internal static IEnumerable<BookingViewModel> MapHotels(this 
        IEnumerable<BookingHotelJsonDto>? items,
        string startDate,
        string endDate,
        int adultsNumber,
        int childrenNumber,
        int roomsNumber,
        string destinationId)
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
                RoomsNumber = roomsNumber,
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
                DestinationId = destinationId,
            };
        }
    }
}