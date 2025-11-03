using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;
using HotelBooking.Services.Contracts.AdminPanelServicesContracts;
using HotelBooking.Services.Contracts.KafkaOperationsLoggerPublisherContracts;
using Microsoft.AspNetCore.Identity;

namespace HotelBooking.Services.AdminPanelServices;
public class AdminPanelOrderByService : IAdminPanelOrderByService
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IKafkaOperationsLoggerProducer _opsLogger;
    private readonly UserManager<UserModel> _userManager;

    
    public AdminPanelOrderByService(IUnitOfWork unitOfWork, IKafkaOperationsLoggerProducer opslogger,
        UserManager<UserModel> userManager)
    {
        _unitOfWork = unitOfWork;
        _opsLogger = opslogger;
        _userManager = userManager;
    }
    
    public async Task<List<AdminPanelBooking>> OrderAdminPanelBookings(
        string orderBy, 
        string orderDirection, 
        UserModel currentUser)
    {
        var bookings = _unitOfWork.Repository<AdminPanelBooking>()
            .Include(x => x.HotelModel)
            .ToList();

        var property = typeof(AdminPanelBooking).GetProperty(orderBy);
        var hotelProperty = typeof(HotelModel).GetProperty(orderBy);

        if (string.IsNullOrWhiteSpace(orderBy) || (property == null && hotelProperty == null))
            return bookings;

        IEnumerable<AdminPanelBooking> orderedBookings;

        if (property != null)
        {
            orderedBookings = orderDirection?.Equals("asc", StringComparison.OrdinalIgnoreCase) == true
                ? bookings.OrderBy(b => property.GetValue(b))
                : bookings.OrderByDescending(b => property.GetValue(b));
        }
        else
        {
            orderedBookings = orderDirection?.Equals("asc", StringComparison.OrdinalIgnoreCase) == true
                ? bookings.OrderBy(b => hotelProperty.GetValue(b.HotelModel))
                : bookings.OrderByDescending(b => hotelProperty.GetValue(b.HotelModel));
        }

        var roles = await _userManager.GetRolesAsync(currentUser);
        var actorRole = roles.FirstOrDefault() ?? "Unknown";

        await _opsLogger.LogAsync(
            entityType: nameof(AdminPanelBooking),
            entityId: "Multiple",
            operation: nameof(OrderAdminPanelBookings),
            changes: bookings.Select(booking => new
            {
                ClientId = booking.ClientId,
                ClientFirstName = booking.ClientFirstName,
                ClientLastName = booking.ClientLastName,
                ClientEmail = booking.ClientEmail,
                StartAt = booking.StartAt,
                EndAt = booking.EndAt,
                Price = booking.Price,
                AdultsNumber = booking.AdultsNumber,
                ChildrenNumber = booking.ChildrenNumber,
                RoomsNumber = booking.RoomsNumber,
                HotelModelId = booking.HotelModelId,
                HotelName = booking.HotelModel?.HotelName
            }).ToList(),
            actorId: currentUser.Id.ToString(),
            actorType: actorRole,
            source: nameof(AdminPanelOrderByService)
        );

        return orderedBookings.ToList();
    }
}