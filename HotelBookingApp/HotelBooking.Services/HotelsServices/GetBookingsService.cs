using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Services.Contracts.HotelsServicesContracts;
using HotelBooking.Services.Contracts.KafkaOperationsLoggerPublisherContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelBooking.Models.Identity;

namespace HotelBooking.Services.HotelsServices;

public class GetBookingsService : IGetBookingsService
{
    private readonly IKafkaOperationsLoggerProducer _logger;
    private readonly UserManager<UserModel> _userManager;

    public GetBookingsService(IKafkaOperationsLoggerProducer logger, UserManager<UserModel> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<List<BookingModel>> GetBookings(IUnitOfWork unitOfWork, string userId)
    {
            var bookings = unitOfWork.Repository<UserBookingModel>()
                .Where(b => b.UserId == Convert.ToInt32(userId))
                .Include(b => b.BookingModel)
                .ThenInclude(b => b.HotelModel)
                .Select(b => b.BookingModel)
                .ToList();

            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            var actorRole = roles.FirstOrDefault() ?? "Unknown";

            foreach (var booking in bookings)
            {
                await _logger.LogAsync(
                    entityType: nameof(BookingModel),
                    entityId: booking.Id.ToString(),
                    operation: nameof(GetBookings),
                    changes: new
                    {
                        Hotel = booking.HotelModel.HotelName,
                        HotelModelId = booking.HotelModelId,
                        StartAt = booking.StartAt,
                        EndAt = booking.EndAt,
                        Price = booking.Price,
                        AdultsNumber = booking.AdultsNumber,
                        ChildrenNumber = booking.ChildrenNumber,
                        RoomsNumber = booking.RoomsNumber
                    },
                    actorId: userId,
                    actorType: actorRole,
                    source: nameof(GetBookingsService));
            }

            return bookings;
    }
}