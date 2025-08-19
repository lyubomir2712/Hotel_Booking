using HotelBooking.Models.AppModels;

namespace HotelBooking.Data.SeedWork.Repositories;

public class BookingRepository : Repository<BookingModel>, IBookingRepository
{
    public BookingRepository(BookingDbContext bookingDbContext) : base(bookingDbContext)
    {
    }
}