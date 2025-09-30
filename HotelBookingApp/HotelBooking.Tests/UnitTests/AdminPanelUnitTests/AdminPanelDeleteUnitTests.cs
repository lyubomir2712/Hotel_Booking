using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.Identity;
using HotelBooking.Services.Contracts.AdminPanelServicesContracts;
using HotelBooking.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HotelBooking.Tests.UnitTests.AdminPanelUnitTests;

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
            .Setup(s => s.AdminPanelDeleteBooking(unitOfWorkMock.Object, bookingId, It.IsAny<UserModel>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        var userManagerMock = new Mock<UserManager<UserModel>>(
            Mock.Of<IUserStore<UserModel>>(), null, null, null, null, null, null, null, null);

        var user = new UserModel { UserName = "admin@test.com" };
        userManagerMock
            .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(user);

        var adminPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
            new[] { new Claim(ClaimTypes.Role, "Admin") }, "TestAuth"));

        var controller = new AdminPanelController(
            unitOfWorkMock.Object,
            getCheckoutedHotelsServiceMock.Object,
            adminPanelDeleteBookingServiceMock.Object,
            userManagerMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = adminPrincipal }
            }
        };

        // Act
        var result = await controller.AdminPanelDeleteBooking(bookingId);

        // Assert
        adminPanelDeleteBookingServiceMock.Verify(
            s => s.AdminPanelDeleteBooking(unitOfWorkMock.Object, bookingId, It.IsAny<UserModel>()),
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
            .Setup(s => s.AdminPanelDeleteBooking(It.IsAny<IUnitOfWork>(), It.IsAny<int>(), It.IsAny<UserModel>()))
            .Returns(Task.CompletedTask);

        var userManagerMock = new Mock<UserManager<UserModel>>(
            Mock.Of<IUserStore<UserModel>>(), null, null, null, null, null, null, null, null);

        var user = new UserModel { UserName = "admin@test.com" };
        userManagerMock
            .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(user);

        var adminPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
            new[] { new Claim(ClaimTypes.Role, "Admin") }, "TestAuth"));

        var controller = new AdminPanelController(
            unitOfWorkMock.Object,
            getCheckoutedHotelsServiceMock.Object,
            adminPanelDeleteBookingServiceMock.Object,
            userManagerMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = adminPrincipal }
            }
        };

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