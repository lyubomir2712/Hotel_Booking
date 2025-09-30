using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Services.AdminPanelServices;
using HotelBooking.Services.Contracts.AdminPanelServicesContracts;
using HotelBooking.Services.KafkaOperationsLoggerPublisher;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HotelBooking.Tests.IntegrationTests.AdminPanelIntegrationTests
{
    public class AdminPanelGetBookingsIntegrationTests
    {
        [Fact]
        public async Task GetCheckoutedHotels_ReturnsOnlyCheckouted()
        {
            var options = new DbContextOptionsBuilder<BookingDbContext>()
                .UseInMemoryDatabase(databaseName: $"GetCheckouted_{Guid.NewGuid()}")
                .Options;

            using var context = new BookingDbContext(options);

            context.AdminPanelBookings.AddRange(
                new AdminPanelBooking 
                { 
                    Id = 1, 
                    ClientId = 101,
                    ClientFirstName = "Ana", 
                    ClientLastName = "Ivanova", 
                    ClientEmail = "ana@example.com",
                    StartAt = DateTime.UtcNow.AddDays(-10),
                    EndAt = DateTime.UtcNow.AddDays(-5),
                    Price = 500,
                    AdultsNumber = 2,
                    ChildrenNumber = 0,
                    RoomsNumber = 1,
                    HotelModelId = 1,
                    HotelModel = new HotelModel
                    {
                        HotelName = "Sunrise Hotel",
                        HotelImg = "sunrise.jpg",
                        City = "Sofia",
                        Country = "Bulgaria",
                        Address = "123 Sunny St"
                    }
                },
                new AdminPanelBooking 
                { 
                    Id = 2, 
                    ClientId = 102,
                    ClientFirstName = "Boris", 
                    ClientLastName = "Petrov", 
                    ClientEmail = "boris@example.com",
                    StartAt = DateTime.UtcNow.AddDays(-20),
                    EndAt = DateTime.UtcNow.AddDays(-15),
                    Price = 750,
                    AdultsNumber = 1,
                    ChildrenNumber = 1,
                    RoomsNumber = 1,
                    HotelModelId = 2,
                    HotelModel = new HotelModel
                    {
                        HotelName = "Mountain View",
                        HotelImg = "mountainview.jpg",
                        City = "Plovdiv",
                        Country = "Bulgaria",
                        Address = "456 Hill Rd"
                    }
                },
                new AdminPanelBooking 
                { 
                    Id = 3, 
                    ClientId = 103,
                    ClientFirstName = "Viki", 
                    ClientLastName = "Georgieva", 
                    ClientEmail = "viki@example.com",
                    StartAt = DateTime.UtcNow.AddDays(-7),
                    EndAt = DateTime.UtcNow.AddDays(-3),
                    Price = 600,
                    AdultsNumber = 2,
                    ChildrenNumber = 2,
                    RoomsNumber = 2,
                    HotelModelId = 3,
                    HotelModel = new HotelModel
                    {
                        HotelName = "City Central",
                        HotelImg = "citycentral.jpg",
                        City = "Varna",
                        Country = "Bulgaria",
                        Address = "789 Center Ave"
                    }
                }
            );
            await context.SaveChangesAsync();

            IUnitOfWork uow = new UnitOfWork(context);
            IGetCheckoutedHotelsService service = new GetCheckoutedHotelsService(new KafkaKafkaOperationsLoggerProducer(new KafkaOptions()));
            
            // Act
            var result = service.GetCheckoutedHotels(uow);

            // Assert
            Assert.NotNull(result);
            var list = result.ToList();
            Assert.Equal(3, list.Count);
            Assert.Contains(list, b => b.Id == 0);
            Assert.Contains(list, b => b.Id == 1);
            Assert.Contains(list, b => b.Id == 2);
            Assert.DoesNotContain(list, b => b.Id == 3);
        }
    }
}