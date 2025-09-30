using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;
using HotelBooking.Services.Contracts.HotelsServicesContracts;

using HotelBooking.Services.Contracts.KafkaOperationsLoggerPublisherContracts;
using Microsoft.AspNetCore.Identity;
namespace HotelBooking.Services.HotelsServices;

public class CheckoutBookingsService : ICheckoutBookingsService
{
    private readonly IKafkaOperationsLoggerProducer _logger;
    private readonly UserManager<UserModel> _userManager;

    public CheckoutBookingsService(
        IKafkaOperationsLoggerProducer logger,
        UserManager<UserModel> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task CheckoutBookingsAsync(IUnitOfWork unitOfWork, UserModel currentUser, List<BookingModel>? bookings)
    {
        if (bookings is null || bookings.Count == 0)
            return;

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

        var roles = await _userManager.GetRolesAsync(currentUser);
        var actorRole = roles.FirstOrDefault() ?? "Unknown";

        foreach (var booking in adminPanelBookings)
        {
            await _logger.LogAsync(
                entityType: nameof(AdminPanelBooking),
                entityId: booking.Id.ToString(),
                operation: nameof(CheckoutBookingsAsync),
                changes: new
                {
                    booking.Id,
                    booking.StartAt,
                    booking.EndAt,
                    booking.Price,
                    booking.AdultsNumber,
                    booking.ChildrenNumber,
                    booking.RoomsNumber,
                    booking.HotelModelId,
                    booking.ClientEmail,
                    booking.ClientFirstName,
                    booking.ClientLastName
                },
                actorId: currentUser.Id.ToString(),
                actorType: actorRole,
                source: nameof(CheckoutBookingsService)
            );
        }
    }
}