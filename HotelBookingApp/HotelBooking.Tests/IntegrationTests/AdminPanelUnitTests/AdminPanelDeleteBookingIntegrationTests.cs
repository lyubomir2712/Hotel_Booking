using HotelBooking.Data.SeedWork;
using System.Threading;
using HotelBooking.Data.SeedWork.Repositories;
using HotelBooking.Models.AppModels;
using HotelBooking.Services.AdminPanelServices;
using HotelBooking.Services.Contracts.AdminPanelContracts;
using HotelBooking.Services.Contracts.AdminPanelServicesContracts;
using HotelBooking.Web.Controllers;

namespace HotelBooking.Tests.IntegrationTests;

using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using HotelBooking.Models;

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
            IGetCheckoutedHotelsService getCheckoutedHotelsService = new GetCheckoutedHotelsService();
            IAdminPanelDeleteBookingService deleteBookingService = new AdminPanelDeleteBookingsService();

            var controller = new AdminPanelController(
                unitOfWork,
                getCheckoutedHotelsService,
                deleteBookingService
            );

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