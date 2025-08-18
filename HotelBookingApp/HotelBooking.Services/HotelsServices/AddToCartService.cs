using HotelBooking.Data;
using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;
using HotelBooking.Services.Contracts.HotelsServicesContracts;
using HotelBooking.Services.ViewModels;

namespace HotelBooking.Services.HotelsServices;

public class AddToCartService : IAddToCartService
{
    public async Task AddToCartAsync(BookingDbContext bookingDbContext, AddToCartInput addToCartInput, UserModel currentUser)
    {
        if (bookingDbContext.Hotels != null)
        {
            var existingHotel = bookingDbContext.Hotels.FirstOrDefault(h => h.HotelName == addToCartInput.HotelName && h.HotelImg == addToCartInput.hotelImg);
            
            HotelModel newHotel;
            if (existingHotel != null)
            {
                newHotel = existingHotel;
            }
            else
            {
                newHotel = new HotelModel { HotelName = addToCartInput.HotelName,
                    HotelImg = addToCartInput.hotelImg,
                    City = addToCartInput.City,
                    Country = addToCartInput.Country,
                    Address = addToCartInput.Address,
                    ReviewScore = addToCartInput.ReviewScore,
                    ReviewsCount = addToCartInput.ReviewsCount,
                    ReviewScoreWord = addToCartInput.ReviewScoreWord
                };
                bookingDbContext.Hotels.Add(newHotel);
                await bookingDbContext.SaveChangesAsync();
            }

            BookingModel newBookingModel = new BookingModel
            {
                StartAt = Convert.ToDateTime(addToCartInput.StartAt),
                Price = Convert.ToDouble(addToCartInput.HotelPrice),
                EndAt = Convert.ToDateTime(addToCartInput.EndAt),
                HotelModel = newHotel,
                HotelModelId = newHotel.Id,
                AdultsNumber = addToCartInput.AdultsNumber,
                ChildrenNumber = addToCartInput.ChildrenNumber,
                RoomsNumber = addToCartInput.RoomsNumber,
            };

            if (bookingDbContext.Bookings != null) await bookingDbContext.Bookings.AddAsync(newBookingModel);
            await bookingDbContext.SaveChangesAsync();

            var newUserBookingModel = new UserBookingModel
            {
                BookingModelId = newBookingModel.Id,
                UserId         = currentUser.Id,
                UserModel      =  currentUser
            };
            if (bookingDbContext.UserBookings != null)
                await bookingDbContext.UserBookings.AddAsync(newUserBookingModel);
        }

        await bookingDbContext.SaveChangesAsync();
    }
}