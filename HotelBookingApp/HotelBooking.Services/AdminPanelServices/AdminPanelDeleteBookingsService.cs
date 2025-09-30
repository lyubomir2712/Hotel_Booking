using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Services.Contracts.AdminPanelContracts;
using HotelBooking.Services.Contracts.AdminPanelServicesContracts;
using HotelBooking.Services.Contracts.KafkaOperationsLoggerPublisherContracts;

namespace HotelBooking.Services.AdminPanelServices;

public class AdminPanelDeleteBookingsService : IAdminPanelDeleteBookingService
{
    private readonly IKafkaOperationsLoggerProducer _opsLogger;

    public AdminPanelDeleteBookingsService(IKafkaOperationsLoggerProducer opsLogger)
    {
        _opsLogger = opsLogger;
    }

    public async Task AdminPanelDeleteBooking(IUnitOfWork unitOfWork, int bookingId)
    {
        var adminPanelBooking = await unitOfWork.Repository<AdminPanelBooking>().FindAsync(bookingId);
        if (adminPanelBooking != null)
        {
            await unitOfWork.Repository<AdminPanelBooking>().RemoveAsync(adminPanelBooking);
            await unitOfWork.SaveChangesAsync();

            _ = _opsLogger.LogAsync(
                entityType: "AdminPanelBooking",
                entityId: bookingId.ToString(),
                operation: "Delete",
                changes: new { Deleted = true, OccurredAt = DateTimeOffset.UtcNow },
                tenantId: null,
                actorId: null,
                source: "AdminPanelDeleteBookingsService"
            );
        }
    }
}