using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Services.Contracts.AdminPanelContracts;
using HotelBooking.Services.Contracts.KafkaOperationsLoggerPublisherContracts;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Services.AdminPanelServices;

public class GetCheckoutedHotelsService : IGetCheckoutedHotelsService
{
    private readonly IKafkaOperationsLoggerProducer _opsLogger;

    public GetCheckoutedHotelsService(IKafkaOperationsLoggerProducer opsLogger)
    {
        _opsLogger = opsLogger;
    }
    public List<AdminPanelBooking> GetCheckoutedHotels(IUnitOfWork unitOfWork)
    {
        var bookings = unitOfWork.Repository<AdminPanelBooking>()
            .Include(x => x.HotelModel)
            .ToList();

        _ = _opsLogger.LogAsync(
            entityType: "AdminPanelBooking",
            entityId: "list",
            operation: "GetCheckoutedHotels",
            changes: new { Count = bookings.Count, OccurredAt = DateTimeOffset.UtcNow },
            actorId: null,
            source: "GetCheckoutedHotelsService"
        );

        return bookings;
    }
}