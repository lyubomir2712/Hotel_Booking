using System.Globalization;
using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;
using HotelBooking.Services.Contracts.EmailServicesContracts;

namespace HotelBooking.Services.EmailServices;

public class GetEmailTemplateHtmlWithParametersService : IGetEmailTemplateHtmlWithParametersService
{
    public string GetEmailTemplateHtmlWithParameters(string template,UserModel receiver, BookingModel booking, HotelModel hotel)
    {
        return template
            .Replace("{{GuestFirstName}}", receiver.FirstName)
            .Replace("{{GuestFullName}}", $"{receiver.FirstName} {receiver.LastName}")
            .Replace("{{BookingId}}", booking.Id.ToString())
            .Replace("{{FullName}}", receiver.UserName)
            .Replace("{{HotelName}}", hotel.HotelName)
            .Replace("{{HotelAddress}}", hotel.Address)
            .Replace("{{RoomsNumber}}", booking.RoomsNumber.ToString())
            .Replace("{{AdultsNumber}}", booking.AdultsNumber.ToString())
            .Replace("{{ChildrenNumber}}", booking.ChildrenNumber.ToString())
            .Replace("{{CheckInDate}}", booking.StartAt.ToString("d", CultureInfo.CurrentCulture))
            .Replace("{{CheckOutDate}}", booking.EndAt.Date.ToString("d",CultureInfo.CurrentUICulture))
            .Replace("{{CheckInTime}}", "12:00 PM")
            .Replace("{{CheckOutTime}}", "10:00 AM")
            .Replace("{{Price}}", booking.Price.ToString("F2", CultureInfo.InvariantCulture))
            .Replace("{{Currency}}", "BGN")
            .Replace("{{Year}}", DateTime.Now.Year.ToString());
    }
}