using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;
using HotelBooking.Services.Contracts.HotelsServicesContracts;

namespace HotelBooking.Services.HotelsServices;

public class CheckoutBookingsService : ICheckoutBookingsService
{
    public async Task CheckoutBookingsAsync(IUnitOfWork unitOfWork, UserModel currentUser, List<BookingModel>? bookings)
    {
        if (bookings != null)
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

            await unitOfWork.Repository<AdminPanelBooking>().AddRangeAsync(adminPanelBookings);
            
            unitOfWork.Repository<BookingModel>().RemoveRange(bookings);
            
            await unitOfWork.SaveChangesAsync();   
        }
    }
}