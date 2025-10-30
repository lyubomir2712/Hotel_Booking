using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;
using HotelBooking.Services.BookingApiConfiguration;
using Microsoft.AspNetCore.Identity;

namespace HotelBooking.Services.Contracts.HotelsServicesContracts;

public interface IGetUnavailableBookingHotelNamesFromUserCartService
{
    public List<string> GetUnavailableBookingHotelNamesFromUserCart(List<BookingModel> unavailableBookings, UserModel user);
}