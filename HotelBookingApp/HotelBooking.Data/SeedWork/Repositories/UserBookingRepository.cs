using HotelBooking.Models.AppModels;

namespace HotelBooking.Data.SeedWork.Repositories;

public class UserBookingRepository : Repository<UserBookingModel>, IUserBookingRepository
{
    public UserBookingRepository(BookingDbContext bookingDbContext) : base(bookingDbContext)
    {
    }
}