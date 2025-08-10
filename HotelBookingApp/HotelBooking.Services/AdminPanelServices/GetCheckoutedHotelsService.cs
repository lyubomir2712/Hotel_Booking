using HotelBooking.Data;
using HotelBooking.Models.AppModels;
using HotelBooking.Services.Contracts.AdminPanelContracts;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Services.AdminPanelServices;

public class GetCheckoutedHotelsService : IGetCheckoutedHotelsService
{
    public List<AdminPanelBookings> GetCheckoutedHotels(BookingDbContext bookingDbContext)
    {
        var bookings = bookingDbContext.AdminPanelBookings
            .Include(x => x.HotelModel)
            .ToList();
        
        return bookings;
    }
}