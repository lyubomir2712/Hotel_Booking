using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;
using HotelBooking.Services.Contracts.AdminPanelServicesContracts;
using HotelBooking.Services.Contracts.KafkaOperationsLoggerPublisherContracts;
using Microsoft.AspNetCore.Identity;

namespace HotelBooking.Services.AdminPanelServices;

public class AdminPanelDeleteBookingsService : IAdminPanelDeleteBookingService
{
    private readonly IKafkaOperationsLoggerProducer _opsLogger;
    private readonly UserManager<UserModel> _userManager;

    public AdminPanelDeleteBookingsService(IKafkaOperationsLoggerProducer opsLogger, UserManager<UserModel> userManager)
    {
        _opsLogger = opsLogger;
        _userManager = userManager;

    }

    public async Task AdminPanelDeleteBooking(IUnitOfWork unitOfWork, int bookingId, UserModel currentUser)
    {
        var adminPanelBooking = await unitOfWork.Repository<AdminPanelBooking>().FindAsync(bookingId);
        if (adminPanelBooking != null)
        {
            await unitOfWork.Repository<AdminPanelBooking>().RemoveAsync(adminPanelBooking);
            await unitOfWork.SaveChangesAsync();
            
            var roles = await _userManager.GetRolesAsync(currentUser);
            var actorRole = roles.FirstOrDefault() ?? "Unknown";

            await _opsLogger.LogAsync(
                entityType: nameof(AdminPanelBooking),
                entityId: bookingId.ToString(),
                operation: nameof(AdminPanelDeleteBooking),
                changes: new
            {
                HotelId = adminPanelBooking.HotelModelId,
                ClientId = adminPanelBooking.ClientId,
                ClientFirstName = adminPanelBooking.ClientFirstName,
                ClientLastName = adminPanelBooking.ClientLastName,
                ClientEmail = adminPanelBooking.ClientEmail,
                CheckInDate = adminPanelBooking.StartAt,
                CheckOutDate = adminPanelBooking.EndAt,
                Price = adminPanelBooking.Price,
                AdultsNumber = adminPanelBooking.AdultsNumber,
                ChildrenNumber = adminPanelBooking.ChildrenNumber,
                RoomsNumber = adminPanelBooking.RoomsNumber,
                DeletedAt = DateTimeOffset.UtcNow
            },
                actorId: currentUser.Id.ToString(),
                actorType: actorRole,
                source: nameof(AdminPanelDeleteBookingsService)
            );
        }
    }
}