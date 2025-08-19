using HotelBooking.Models.AppModels;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data.SeedWork.Repositories;


public class HotelRepository : Repository<HotelModel>, IHotelRepository
{
    public HotelRepository(BookingDbContext bookingDbContext) : base(bookingDbContext)
    {
    }
    
}