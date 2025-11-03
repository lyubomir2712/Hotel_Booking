using HotelBooking.Services.Contracts.KafkaOperationsLoggerPublisherContracts;
using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Services.Contracts.HotelsServicesContracts;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using HotelBooking.Models.Identity;

namespace HotelBooking.Services.HotelsServices;

public class RemoveBookingService : IRemoveBookingService
{
    private readonly IKafkaOperationsLoggerProducer _logger;
    private readonly UserManager<UserModel> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveBookingService(IKafkaOperationsLoggerProducer logger, UserManager<UserModel> userManager, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task RemoveHotelAsync(int bookingId, UserModel currentUser)
    {
        var hotel = await _unitOfWork.Repository<BookingModel>().FirstOrDefaultAsync(b => b.Id == bookingId);
        if (hotel != null)
        {
            await _unitOfWork.Repository<BookingModel>().RemoveAsync(hotel);
            await _unitOfWork.SaveChangesAsync();

            var hotelModel = await _unitOfWork.Repository<HotelModel>().FirstOrDefaultAsync(h => h.Id == hotel.HotelModelId);

            var roles = await _userManager.GetRolesAsync(currentUser);
            var actorRole = roles.FirstOrDefault() ?? "Unknown";

            await _logger.LogAsync(
                entityType: nameof(BookingModel),
                entityId: hotel.Id.ToString(),
                operation: nameof(RemoveHotelAsync),
                changes: new
                {
                    Hotel = hotelModel?.HotelName,
                    HotelModelId = hotel.HotelModelId,
                    StartAt = hotel.StartAt,
                    EndAt = hotel.EndAt,
                    Price = hotel.Price,
                    AdultsNumber = hotel.AdultsNumber,
                    ChildrenNumber = hotel.ChildrenNumber,
                    RoomsNumber = hotel.RoomsNumber
                },
                actorId: currentUser.Id.ToString(),
                actorType: actorRole,
                source: nameof(RemoveBookingService)
            );
        }
    }
}