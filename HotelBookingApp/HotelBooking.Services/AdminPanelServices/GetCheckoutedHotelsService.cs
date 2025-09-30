using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Services.Contracts.KafkaOperationsLoggerPublisherContracts;
using Microsoft.AspNetCore.Identity;
using HotelBooking.Models.Identity;
using HotelBooking.Services.Contracts.AdminPanelServicesContracts;

namespace HotelBooking.Services.AdminPanelServices;

public class GetCheckoutedHotelsService : IGetCheckoutedHotelsService
{
    private readonly IKafkaOperationsLoggerProducer _opsLogger;
    private readonly UserManager<UserModel> _userManager;

    public GetCheckoutedHotelsService(IKafkaOperationsLoggerProducer opsLogger, UserManager<UserModel> userManager)
    {
        _opsLogger = opsLogger;
        _userManager = userManager;
    }
    public async Task<List<AdminPanelBooking>> GetCheckoutedHotels(IUnitOfWork unitOfWork, UserModel currentUser)
    {
        var bookings = unitOfWork.Repository<AdminPanelBooking>()
            .Include(x => x.HotelModel)
            .ToList();

        var roles = await _userManager.GetRolesAsync(currentUser);
        var actorRole = roles.FirstOrDefault() ?? "Unknown";

        await _opsLogger.LogAsync(
            entityType: nameof(AdminPanelBooking),
            entityId: "Multiple",
            operation: nameof(GetCheckoutedHotels),
            changes: bookings.Select(booking => new
            {
                Hotel = booking.HotelModel.HotelName,
                HotelModelId = booking.HotelModelId,
                StartAt = booking.StartAt,
                EndAt = booking.EndAt,
                Price = booking.Price,
                AdultsNumber = booking.AdultsNumber,
                ChildrenNumber = booking.ChildrenNumber,
                RoomsNumber = booking.RoomsNumber
            }).ToList(),
            actorId: currentUser.Id.ToString(),
            actorType: actorRole,
            source: nameof(GetCheckoutedHotelsService)
        );

        return bookings;
    }
}