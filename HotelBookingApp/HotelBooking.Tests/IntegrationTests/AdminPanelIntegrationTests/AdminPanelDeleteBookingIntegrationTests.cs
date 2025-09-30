using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Services.AdminPanelServices;
using HotelBooking.Services.Contracts.AdminPanelServicesContracts;
using HotelBooking.Services.KafkaOperationsLoggerPublisher;
using HotelBooking.Web.Controllers;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Microsoft.AspNetCore.Identity;
using HotelBooking.Models.Identity;
using Moq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace HotelBooking.Tests.IntegrationTests.AdminPanelIntegrationTests;

public class AdminPanelDeleteBookingIntegrationTests
{
    [Fact]
    public async Task AdminPanelDeleteBookingRemovesBookingById()
    {
        var options = new DbContextOptionsBuilder<BookingDbContext>()
            .UseInMemoryDatabase(databaseName: $"DeleteBookingTestDb_{Guid.NewGuid()}")
            .Options;

        using (var context = new BookingDbContext(options))
        {
            var hotelA = new HotelModel
            {
                Id = 1,
                HotelName = "Hotel Alpha",
                Address = "123 Main St",
                City = "Metropolis",
                Country = "CountryA"
            };
            var hotelB = new HotelModel
            {
                Id = 2,
                HotelName = "Hotel Bravo",
                Address = "456 Side St",
                City = "Gotham",
                Country = "CountryB"
            };

            var booking1 = new AdminPanelBooking
            {
                Id = 1,
                ClientEmail = "client1@example.com",
                ClientFirstName = "Alice",
                ClientLastName = "Smith",
                HotelModel = hotelA
            };
            var booking2 = new AdminPanelBooking
            {
                Id = 2,
                ClientEmail = "client2@example.com",
                ClientFirstName = "Bob",
                ClientLastName = "Jones",
                HotelModel = hotelB
            };

            context.AdminPanelBookings.Add(booking1);
            context.AdminPanelBookings.Add(booking2);
            await context.SaveChangesAsync();
            
            var unitOfWork = new UnitOfWork(context);
            // Mock UserManager<UserModel>
            var userManagerMock = new Mock<UserManager<UserModel>>(
                Mock.Of<IUserStore<UserModel>>(), null, null, null, null, null, null, null, null);
            userManagerMock
                .Setup(um => um.GetRolesAsync(It.IsAny<UserModel>()))
                .ReturnsAsync(new List<string> { "Admin" });
            userManagerMock
                .Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new UserModel { Id = 1 });

            IGetCheckoutedHotelsService getCheckoutedHotelsService = new GetCheckoutedHotelsService(
                new KafkaKafkaOperationsLoggerProducer(new KafkaOptions()),
                userManagerMock.Object);
            IAdminPanelDeleteBookingService deleteBookingService = new AdminPanelDeleteBookingsService(
                new KafkaKafkaOperationsLoggerProducer(new KafkaOptions()),
                userManagerMock.Object);

            var controller = new AdminPanelController(
                unitOfWork,
                getCheckoutedHotelsService,
                deleteBookingService,
                userManagerMock.Object
            );
            // Set up fake authenticated user for controller context
            controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, "1"),
                        new Claim(ClaimTypes.Name, "test.user@example.com")
                    }, "TestAuth"))
                }
            };

            var actionResult = await controller.AdminPanelDeleteBooking(1);

            var deletedBooking = await context.AdminPanelBookings.FindAsync(1);
            var remainingBooking = await context.AdminPanelBookings.FindAsync(2);

            Assert.Null(deletedBooking);
            Assert.NotNull(remainingBooking);
            Assert.IsType<Microsoft.AspNetCore.Mvc.RedirectToActionResult>(actionResult);
            var redirect = (Microsoft.AspNetCore.Mvc.RedirectToActionResult)actionResult;
            Assert.Equal("GetCheckoutedHotels", redirect.ActionName);
        }
    }
}