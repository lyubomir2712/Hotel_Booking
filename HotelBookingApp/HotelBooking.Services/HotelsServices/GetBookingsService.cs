using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Services.Contracts.HotelsServicesContracts;
using HotelBooking.Services.Contracts.KafkaOperationsLoggerPublisherContracts;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Services.HotelsServices;

public class GetBookingsService : IGetBookingsService
{
    private readonly IKafkaOperationsLoggerProducer _logger;

    public GetBookingsService(IKafkaOperationsLoggerProducer logger)
    {
        _logger = logger;
    }

    public List<BookingModel> GetBookings(IUnitOfWork unitOfWork ,string userId)
    {
            var bookings = unitOfWork.Repository<UserBookingModel>()
                .Where(b => b.UserId == Convert.ToInt32(userId))
                .Include(b => b.BookingModel)
                .ThenInclude(b => b.HotelModel)
                .Select(b => b.BookingModel)
                .ToList();

            foreach (var booking in bookings)
            {
                _logger.LogAsync(
                    entityType: nameof(BookingModel),
                    entityId: booking.Id.ToString(),
                    operation: "Get",
                    actorId: userId,
                    source: nameof(GetBookingsService));
            }

            return bookings;
    }
}