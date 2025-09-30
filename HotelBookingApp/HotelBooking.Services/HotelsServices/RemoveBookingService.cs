using HotelBooking.Services.Contracts.KafkaOperationsLoggerPublisherContracts;
using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Services.Contracts.HotelsServicesContracts;

namespace HotelBooking.Services.HotelsServices;

public class RemoveBookingService : IRemoveBookingService
{
    private readonly IKafkaOperationsLoggerProducer _logger;

    public RemoveBookingService(IKafkaOperationsLoggerProducer logger)
    {
        _logger = logger;
    }

    public async Task RemoveHotelAsync(IUnitOfWork unitOfWork, int bookingId)
    {
        var hotel = await unitOfWork.Repository<BookingModel>().FirstOrDefaultAsync(b => b.Id == bookingId);
        if (hotel != null)
        {
            await unitOfWork.Repository<BookingModel>().RemoveAsync(hotel);
            await unitOfWork.SaveChangesAsync();
            await _logger.LogAsync(
                entityType: nameof(BookingModel),
                entityId: hotel.Id.ToString(),
                operation: "Remove",
                changes: new { hotel.Id},
                source: nameof(RemoveBookingService)
            );
        }
    }
}