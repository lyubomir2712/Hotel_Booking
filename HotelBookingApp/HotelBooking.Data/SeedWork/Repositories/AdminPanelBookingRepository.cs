using HotelBooking.Models.AppModels;

namespace HotelBooking.Data.SeedWork.Repositories;

public class AdminPanelBookingRepository : Repository<AdminPanelBooking>, IAdminPanelBookingRepository
{
    public AdminPanelBookingRepository(BookingDbContext bookingDbContext) : base(bookingDbContext)
    {
    }
}