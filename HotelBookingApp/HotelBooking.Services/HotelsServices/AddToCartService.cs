using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;
using HotelBooking.Services.Contracts.HotelsServicesContracts;
using HotelBooking.Services.ViewModels;
using HotelBooking.Services.Contracts.KafkaOperationsLoggerPublisherContracts;

namespace HotelBooking.Services.HotelsServices;

public class AddToCartService : IAddToCartService
{
    private readonly IOperationsLoggerProducer _operationsLoggerProducer;

    public AddToCartService(IOperationsLoggerProducer operationsLoggerProducer)
    {
        _operationsLoggerProducer = operationsLoggerProducer;
    }
    public async Task AddToCartAsync(IUnitOfWork unitOfWork, AddToCartInput addToCartInput, UserModel currentUser)
    {
        
        var existingHotel = await unitOfWork.Repository<HotelModel>().FirstOrDefaultAsync(h => h.HotelName == addToCartInput.HotelName && h.HotelImg == addToCartInput.hotelImg);
            
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
                ReviewScoreWord = addToCartInput.ReviewScoreWord
            };
            await unitOfWork.Repository<HotelModel>().AddAsync(newHotel);
            await unitOfWork.SaveChangesAsync();
        }

            BookingModel newBookingModel = new BookingModel
            { 
                StartAt = Convert.ToDateTime(addToCartInput.StartAt),
                Price = Convert.ToDouble(addToCartInput.HotelPrice),
                EndAt = Convert.ToDateTime(addToCartInput.EndAt),
                HotelModel = newHotel,
                HotelModelId = newHotel.Id,
                AdultsNumber = addToCartInput.AdultsNumber,
                ChildrenNumber = addToCartInput.ChildrenNumber,
                RoomsNumber = addToCartInput.RoomsNumber,
            };

            await unitOfWork.Repository<BookingModel>().AddAsync(newBookingModel);
            await unitOfWork.SaveChangesAsync();

            var newUserBookingModel = new UserBookingModel
            {
                BookingModelId = newBookingModel.Id,
                UserId         = currentUser.Id,
                UserModel      =  currentUser
            };
            await unitOfWork.Repository<UserBookingModel>().AddAsync(newUserBookingModel);

        await unitOfWork.SaveChangesAsync();
        
        await _operationsLoggerProducer.LogAsync(
            entityType: "Booking", 
            entityId: newBookingModel.Id.ToString(), 
            operation: "AddToCart", 
            changes: new
            {
                Hotel = newHotel.HotelName,
                Star = newBookingModel.StartAt,
                EndAt = newBookingModel.EndAt,
                Adults = newBookingModel.AdultsNumber,
                Children = newBookingModel.ChildrenNumber,
                Rooms = newBookingModel.RoomsNumber
            },
                actorId: currentUser.Id.ToString()
            );
    }
}