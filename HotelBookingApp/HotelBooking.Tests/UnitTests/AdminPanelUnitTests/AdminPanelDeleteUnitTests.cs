using System.Reflection;
using System.Threading.Tasks;
using HotelBooking.Data.SeedWork;
using HotelBooking.Services.Contracts.AdminPanelContracts;
using HotelBooking.Services.Contracts.AdminPanelServicesContracts;
using HotelBooking.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HotelBooking.Tests.UnitTests;

public class AdminPanelDeleteUnitTests
{
    [Fact]
    public async Task AdminPanelDeleteBookingCallsServiceWithCorrectParameters()
    {
        // Arrange
        var bookingId = 123;

        var unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var getCheckoutedHotelsServiceMock = new Mock<IGetCheckoutedHotelsService>(MockBehavior.Strict);
        var adminPanelDeleteBookingServiceMock = new Mock<IAdminPanelDeleteBookingService>(MockBehavior.Strict);

        adminPanelDeleteBookingServiceMock
            .Setup(s => s.AdminPanelDeleteBooking(unitOfWorkMock.Object, bookingId))
            .Returns(Task.CompletedTask)
            .Verifiable();

        var controller = new AdminPanelController(
            unitOfWorkMock.Object,
            getCheckoutedHotelsServiceMock.Object,
            adminPanelDeleteBookingServiceMock.Object);

        // Act
        var result = await controller.AdminPanelDeleteBooking(bookingId);

        // Assert
        adminPanelDeleteBookingServiceMock.Verify(
            s => s.AdminPanelDeleteBooking(unitOfWorkMock.Object, bookingId),
            Times.Once);

        Assert.IsType<RedirectToActionResult>(result);
    }

    [Fact]
    public async Task AdminPanelDeleteBookingRedirectsToGetCheckoutedHotels()
    {
        // Arrange
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var getCheckoutedHotelsServiceMock = new Mock<IGetCheckoutedHotelsService>();
        var adminPanelDeleteBookingServiceMock = new Mock<IAdminPanelDeleteBookingService>();

        adminPanelDeleteBookingServiceMock
            .Setup(s => s.AdminPanelDeleteBooking(It.IsAny<IUnitOfWork>(), It.IsAny<int>()))
            .Returns(Task.CompletedTask);

        var controller = new AdminPanelController(
            unitOfWorkMock.Object,
            getCheckoutedHotelsServiceMock.Object,
            adminPanelDeleteBookingServiceMock.Object);

        // Act
        var result = await controller.AdminPanelDeleteBooking(42);

        // Assert
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(AdminPanelController.GetCheckoutedHotels), redirect.ActionName);
    }
    
    
    [Fact]
    public void GetCheckoutedHotelsHasAuthorizeAdminRole()
    {
        // Arrange
        var method = typeof(AdminPanelController)
            .GetMethod(nameof(AdminPanelController.GetCheckoutedHotels));

        Assert.NotNull(method);

        // Act
        var authorizeAttr = method!.GetCustomAttribute<AuthorizeAttribute>();

        // Assert
        Assert.NotNull(authorizeAttr);
        Assert.Equal("Admin", authorizeAttr!.Roles);
    }
}