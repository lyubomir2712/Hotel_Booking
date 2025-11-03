using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;
using HotelBooking.Services.Contracts.AdminPanelServicesContracts;
using HotelBooking.Services.Contracts.KafkaOperationsLoggerPublisherContracts;
using Microsoft.AspNetCore.Identity;

namespace HotelBooking.Services.AdminPanelServices;

public class AdminPanelDeleteBookingService : IAdminPanelDeleteBookingService
{
    private readonly IKafkaOperationsLoggerProducer _opsLogger;
    private readonly UserManager<UserModel> _userManager;
    private readonly IUnitOfWork _unitOfWork;


    public AdminPanelDeleteBookingService(IKafkaOperationsLoggerProducer opsLogger, UserManager<UserModel> userManager,
        IUnitOfWork unitOfWork)
    {
        _opsLogger = opsLogger;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task AdminPanelDeleteBooking(int bookingId, UserModel currentUser)
    {
        var adminPanelBooking = await _unitOfWork.Repository<AdminPanelBooking>().FindAsync(bookingId);
        if (adminPanelBooking != null)
        {
            await _unitOfWork.Repository<AdminPanelBooking>().RemoveAsync(adminPanelBooking);
            await _unitOfWork.SaveChangesAsync();
            
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
                source: nameof(AdminPanelDeleteBookingService)
            );
        }
    }
}