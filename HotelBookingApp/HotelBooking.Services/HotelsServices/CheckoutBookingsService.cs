using HotelBooking.Data;
using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;
using HotelBooking.Services.Contracts.HotelsServicesContracts;

namespace HotelBooking.Services.HotelsServices;

public class CheckoutBookingsService : ICheckoutBookingsService
{
    public async Task CheckoutBookingsAsync(BookingDbContext bookingDbContext, UserModel currentUser, List<BookingModel>? bookings)
    {
        if (bookings != null && bookingDbContext.Bookings != null)
        {
            var adminPanelBookings = bookings.Select(b => new AdminPanelBooking
            {
                ClientId = currentUser.Id,
                ClientFirstName = currentUser.FirstName,   
                ClientLastName = currentUser.LastName, 
                ClientEmail = currentUser.Email,
                StartAt = b.StartAt,
                EndAt = b.EndAt,
                Price = b.Price,
                AdultsNumber = b.AdultsNumber,
                ChildrenNumber = b.ChildrenNumber,
                RoomsNumber = b.RoomsNumber,
                HotelModelId = b.HotelModelId
            }).ToList();

            await bookingDbContext.AdminPanelBookings.AddRangeAsync(adminPanelBookings);
            
            bookingDbContext.Bookings.RemoveRange(bookings);
            
            await bookingDbContext.SaveChangesAsync();   
        }
    }
}