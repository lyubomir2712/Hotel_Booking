using System.Globalization;
using HotelBooking.Data.SeedWork;
using HotelBooking.Data.SeedWork.RepositoriesInterfaces;
using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;
using HotelBooking.Services.HotelsServices;
using HotelBooking.Services.KafkaOperationsLoggerPublisher;
using HotelBooking.Services.ViewModels;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Identity;

namespace HotelBooking.Tests.UnitTests.BookingCartUnitTests;

public class BookingCartAddUnitTests
{
    public BookingCartAddUnitTests()
    {
        var ci = CultureInfo.GetCultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentCulture = ci;
        CultureInfo.DefaultThreadCurrentUICulture = ci;
    }

    private static (Mock<IUnitOfWork> uow,
        Mock<IRepository<HotelModel>> hotelRepo,
        Mock<IRepository<BookingModel>> bookingRepo,
        Mock<IRepository<UserBookingModel>> userBookingRepo) CreateUowWithRepos()
    {
        var uow = new Mock<IUnitOfWork>();
        var hotelRepo = new Mock<IRepository<HotelModel>>();
        var bookingRepo = new Mock<IRepository<BookingModel>>();
        var userBookingRepo = new Mock<IRepository<UserBookingModel>>();

        uow.Setup(x => x.Repository<HotelModel>())
            .Returns(hotelRepo.Object);
        uow.Setup(x => x.Repository<BookingModel>())
            .Returns(bookingRepo.Object);
        uow.Setup(x => x.Repository<UserBookingModel>())
            .Returns(userBookingRepo.Object);

        uow.Setup(x => x.SaveChanges()).Returns(0);
        uow.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
        return (uow, hotelRepo, bookingRepo, userBookingRepo);
    }

    private static AddToCartInput MakeInput(string hotelName = "Test Hotel", string hotelImg = "img.jpg")
    {
        return new AddToCartInput
        {
            HotelName = hotelName,
            hotelImg = hotelImg,
            City = "Veliko Tarnovo",
            Country = "Bulgaria",
            Address = "ul. Test 1",
            ReviewScore = 9.1,
            ReviewsCount = 123,
            ReviewScoreWord = "Superb",
            StartAt = "2025-10-01",
            EndAt = "2025-10-05",
            HotelPrice = "199.99",
            AdultsNumber = 2,
            ChildrenNumber = 1,
            RoomsNumber = 1
        };
    }

    [Theory]
    [InlineData("Test Hotel", "img.jpg")]
    public async Task AddToCartAsyncCreatesBookingAndUserBookingWhenHotelAlreadyExists(string hotelName, string hotelImg)
    {
        // Arrange
        var (uow, hotelRepo, bookingRepo, userBookingRepo) = CreateUowWithRepos();
        var existingHotel = new HotelModel { Id = 42, HotelName = hotelName, HotelImg = hotelImg };

        hotelRepo
            .Setup(r => r.FirstOrDefaultAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<HotelModel, bool>>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(existingHotel);

        BookingModel? capturedBooking = null;
            
        bookingRepo
            .Setup(r => r.AddAsync(It.IsAny<BookingModel>(), It.IsAny<CancellationToken>()))
            .Callback<BookingModel, CancellationToken>((b, _) => capturedBooking = b)
            .Returns(Task.CompletedTask);

        UserBookingModel? capturedUserBooking = null;
        
        userBookingRepo
            .Setup(r => r.AddAsync(It.IsAny<UserBookingModel>(), It.IsAny<CancellationToken>()))
            .Callback<UserBookingModel, CancellationToken>((ub, _) => capturedUserBooking = ub)
            .Returns(Task.CompletedTask);

        var userManagerMock = new Mock<UserManager<UserModel>>(
            Mock.Of<IUserStore<UserModel>>(), null, null, null, null, null, null, null, null);
        userManagerMock.Setup(um => um.GetRolesAsync(It.IsAny<UserModel>()))
            .ReturnsAsync(new List<string> { "User" });

        var addToCartService = new AddToCartService(
            new KafkaKafkaOperationsLoggerProducer(new KafkaOptions()),
            userManagerMock.Object,
            uow.Object);
        var input = MakeInput(hotelName, hotelImg);
        var user = new UserModel { Id = 1 };

        // Act
        await addToCartService.AddToCartAsync(input, user);

        // Assert
        hotelRepo.Verify(r => r.FirstOrDefaultAsync(
            It.IsAny<System.Linq.Expressions.Expression<Func<HotelModel, bool>>>(),
            It.IsAny<CancellationToken>()
        ), Times.Once);
        hotelRepo.Verify(r => r.AddAsync(It.IsAny<HotelModel>(), It.IsAny<CancellationToken>()), Times.Never, "Hotel must not be created when it already exists");

        bookingRepo.Verify(r => r.AddAsync(It.IsAny<BookingModel>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(capturedBooking);
        Assert.Equal(existingHotel.Id, capturedBooking!.HotelModelId);
        Assert.Equal(existingHotel, capturedBooking.HotelModel);
        Assert.Equal(DateTime.Parse(input.StartAt), capturedBooking.StartAt);
        Assert.Equal(DateTime.Parse(input.EndAt), capturedBooking.EndAt);
        Assert.Equal(Convert.ToDouble(input.HotelPrice), capturedBooking.Price);
        Assert.Equal(input.AdultsNumber, capturedBooking.AdultsNumber);
        Assert.Equal(input.ChildrenNumber, capturedBooking.ChildrenNumber);
        Assert.Equal(input.RoomsNumber, capturedBooking.RoomsNumber);

        userBookingRepo.Verify(r => r.AddAsync(It.IsAny<UserBookingModel>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(capturedUserBooking);
        Assert.Equal(1, capturedUserBooking!.UserId);
        Assert.Equal(capturedBooking.Id, capturedUserBooking.BookingModelId);
        Assert.Equal(user, capturedUserBooking.UserModel);
        
        uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
    }
    
    
    
    [Fact]
    public async Task AddToCartAsyncCreatesHotelThenBookingAndUserBookingWhenHotelDoesNotExist()
    {
        // Arrange
        var (uow, hotelRepo, bookingRepo, userBookingRepo) = CreateUowWithRepos();

        hotelRepo
            .Setup(r => r.FirstOrDefaultAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<HotelModel, bool>>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((HotelModel?)null);

        HotelModel? createdHotel = null;
        hotelRepo
            .Setup(r => r.AddAsync(It.IsAny<HotelModel>(), It.IsAny<CancellationToken>()))
            .Callback<HotelModel, CancellationToken>((h, _) =>
            {
                h.Id = 100;
                createdHotel = h;
            })
            .Returns(Task.CompletedTask);

        BookingModel? createdBooking = null;
        bookingRepo
            .Setup(r => r.AddAsync(It.IsAny<BookingModel>(), It.IsAny<CancellationToken>()))
            .Callback<BookingModel, CancellationToken>((b, _) => createdBooking = b)
            .Returns(Task.CompletedTask);

        UserBookingModel? createdUserBooking = null;
        userBookingRepo
            .Setup(r => r.AddAsync(It.IsAny<UserBookingModel>(), It.IsAny<CancellationToken>()))
            .Callback<UserBookingModel, CancellationToken>((ub, _) => createdUserBooking = ub)
            .Returns(Task.CompletedTask);

        var userManagerMock = new Mock<UserManager<UserModel>>(
            Mock.Of<IUserStore<UserModel>>(), null, null, null, null, null, null, null, null);
        userManagerMock.Setup(um => um.GetRolesAsync(It.IsAny<UserModel>()))
            .ReturnsAsync(new List<string> { "User" });

        var svc = new AddToCartService(
            new KafkaKafkaOperationsLoggerProducer(new KafkaOptions()),
            userManagerMock.Object,
            uow.Object);
        var input = MakeInput(hotelName: "Brand New", hotelImg: "new.jpg");
        var user = new UserModel { Id = 2 };

        // Act
        await svc.AddToCartAsync(input, user);

        // Assert
        hotelRepo.Verify(r => r.AddAsync(It.IsAny<HotelModel>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(createdHotel);
        Assert.Equal("Brand New", createdHotel!.HotelName);
        Assert.Equal("new.jpg", createdHotel.HotelImg);
        Assert.Equal(input.City, createdHotel.City);
        Assert.Equal(input.Country, createdHotel.Country);
        Assert.Equal(input.Address, createdHotel.Address);
        Assert.Equal(input.ReviewScore, createdHotel.ReviewScore);
        Assert.Equal(input.ReviewsCount, createdHotel.ReviewsCount);
        Assert.Equal(input.ReviewScoreWord, createdHotel.ReviewScoreWord);

        bookingRepo.Verify(r => r.AddAsync(It.IsAny<BookingModel>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(createdBooking);
        Assert.Equal(100, createdBooking!.HotelModelId);
        Assert.Equal(createdHotel, createdBooking.HotelModel);

        userBookingRepo.Verify(r => r.AddAsync(It.IsAny<UserBookingModel>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(createdUserBooking);
        Assert.Equal(2, createdUserBooking!.UserId);
        Assert.Equal(createdBooking.Id, createdUserBooking.BookingModelId);

        uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(3));
    }
}