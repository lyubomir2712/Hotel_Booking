using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Services.Contracts.KafkaOperationsLoggerPublisherContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
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

        _ = _opsLogger.LogAsync(
            entityType: nameof(AdminPanelBooking),
            entityId: "list",
            operation: nameof(GetCheckoutedHotels),
            changes: new { Count = bookings.Count, OccurredAt = DateTimeOffset.UtcNow },
            actorId: currentUser.Id.ToString(),
            actorType: actorRole,
            source: nameof(GetCheckoutedHotelsService)
        );

        return bookings;
    }
}