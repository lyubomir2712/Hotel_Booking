using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;
using HotelBooking.Services.Contracts.HotelsServicesContracts;
using HotelBooking.Services.ViewModels;
using HotelBooking.Services.Contracts.KafkaOperationsLoggerPublisherContracts;

using Microsoft.AspNetCore.Identity;

using System.Linq;

namespace HotelBooking.Services.HotelsServices;

public class AddToCartService : IAddToCartService
{
    private readonly IKafkaOperationsLoggerProducer _kafkaOperationsLoggerProducer;

    private readonly UserManager<UserModel> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    public AddToCartService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public AddToCartService(IKafkaOperationsLoggerProducer kafkaOperationsLoggerProducer, UserManager<UserModel> userManager)
    {
        _kafkaOperationsLoggerProducer = kafkaOperationsLoggerProducer;
        _userManager = userManager;
    }
    public async Task AddToCartAsync(AddToCartInput addToCartInput, UserModel currentUser)
    {
        
        var existingHotel = await _unitOfWork.Repository<HotelModel>().FirstOrDefaultAsync(h => h.HotelName == addToCartInput.HotelName && h.HotelImg == addToCartInput.hotelImg);
            
        HotelModel newHotel;
        if (existingHotel != null)
        {
            newHotel = existingHotel;
        }
        else
        {
            newHotel = new HotelModel 
            { HotelName = addToCartInput.HotelName, 
                HotelImg = addToCartInput.hotelImg,
                City = addToCartInput.City,
                Country = addToCartInput.Country,
                Address = addToCartInput.Address,
                ReviewScore = addToCartInput.ReviewScore,
                ReviewsCount = addToCartInput.ReviewsCount,
                ReviewScoreWord = addToCartInput.ReviewScoreWord,
                RapidApiHotelId = addToCartInput.RapidApiHotelId,
                DestinationId = addToCartInput.DestinationId,
            };
            await _unitOfWork.Repository<HotelModel>().AddAsync(newHotel);
            await _unitOfWork.SaveChangesAsync();
        }

            BookingModel newBookingModel = new BookingModel
            { 
                StartAt = Convert.ToDateTime(addToCartInput.StartAt),
                Price = ParseDoubleFlexible(addToCartInput.HotelPrice),
                EndAt = Convert.ToDateTime(addToCartInput.EndAt),
                HotelModel = newHotel,
                HotelModelId = newHotel.Id,
                AdultsNumber = addToCartInput.AdultsNumber,
                ChildrenNumber = addToCartInput.ChildrenNumber,
                RoomsNumber = addToCartInput.RoomsNumber,
                RapidApiHotelId = addToCartInput.RapidApiHotelId,
                DestinationId = addToCartInput.DestinationId,
            };

            await _unitOfWork.Repository<BookingModel>().AddAsync(newBookingModel);
            await _unitOfWork.SaveChangesAsync();

            var newUserBookingModel = new UserBookingModel
            {
                BookingModelId = newBookingModel.Id,
                UserId         = currentUser.Id,
                UserModel      =  currentUser
            };
            await _unitOfWork.Repository<UserBookingModel>().AddAsync(newUserBookingModel);

        await _unitOfWork.SaveChangesAsync();
        
        var roles = await _userManager.GetRolesAsync(currentUser);
        var actorRole = roles.FirstOrDefault() ?? "Unknown";

        await _kafkaOperationsLoggerProducer.LogAsync(
            entityType: nameof(BookingModel),
            entityId: newBookingModel.Id.ToString(),
            operation: nameof(AddToCartAsync),
            changes: new
            {
                Hotel = newHotel.HotelName,
                HotelModelId = newBookingModel.HotelModelId,
                StartAt = newBookingModel.StartAt,
                EndAt = newBookingModel.EndAt,
                Price = newBookingModel.Price,
                AdultsNumber = newBookingModel.AdultsNumber,
                ChildrenNumber = newBookingModel.ChildrenNumber,
                RoomsNumber = newBookingModel.RoomsNumber,
                RapidApiHotelId = newBookingModel.RapidApiHotelId,
                DestinationId = newBookingModel.DestinationId,
            },
            actorId: currentUser.Id.ToString(),
            actorType: actorRole,
            source: nameof(AddToCartService)
        );
    }
    
    
    private static double ParseDoubleFlexible(string? s)
    {
        if (string.IsNullOrWhiteSpace(s))
            return 0d;

        double result;
       
        var alt = s.Replace('.', ',');
        if (double.TryParse(alt, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out result))
            return result;

        return 0d;
    }
}