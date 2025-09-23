using FluentAssertions;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Services.AdminPanelServices;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace HotelBooking.Tests.UnitTests;

public class AdminPanelGetBookingsUnitTests
{
    private static Mock<IUnitOfWork> CreateUnitOfWorkBackedByInMemoryDb(List<AdminPanelBooking> seed)
    {
        var uowMock = new Mock<IUnitOfWork>();
        
        var mockedAdminPanelBookingRepo = new Mock<IRepository<AdminPanelBooking>>();
 
        mockedAdminPanelBookingRepo
            .Setup(r => r.Query())
            .Returns(seed.AsQueryable());

        mockedAdminPanelBookingRepo
            .Setup(r => r.Include(It.IsAny<Expression<Func<AdminPanelBooking, object>>[]>()))
            .Returns(seed.AsQueryable());

        mockedAdminPanelBookingRepo
            .Setup(r => r.ListAsync(It.IsAny<Expression<Func<AdminPanelBooking, bool>>?>(), It.IsAny<CancellationToken>()))
            .Returns<Expression<Func<AdminPanelBooking, bool>>?, CancellationToken>((pred, _) =>
            {
                var query = seed.AsQueryable();
                return Task.FromResult((pred == null ? query : query.Where(pred)).ToList());
            });
        
        uowMock
            .Setup(u => u.Repository<AdminPanelBooking>())
            .Returns(mockedAdminPanelBookingRepo.Object);

        return uowMock;
    }

    [Fact]
    public void GetCheckoutedHotelsReturnsAllBookingsWithHotelJoin()
    {
        // Arrange
        var hotelA = new HotelModel { Id = 1, HotelName = "Hotel A", Address = "123 A St", City = "A City", Country = "A Country" };
        var hotelB = new HotelModel { Id = 2, HotelName = "Hotel B", Address = "456 B Ave", City = "B City", Country = "B Country" };

        var seed = new List<AdminPanelBooking>
        {
            new AdminPanelBooking { Id = 10, HotelModel = hotelA, ClientEmail = "alice@example.com", ClientFirstName = "Alice", ClientLastName = "Anderson",  },
            new AdminPanelBooking { Id = 20, HotelModel = hotelB, ClientEmail = "bob@example.com", ClientFirstName = "Bob", ClientLastName = "Brown", },
            new AdminPanelBooking { Id = 30, HotelModel = hotelA, ClientEmail = "carol@example.com", ClientFirstName = "Carol", ClientLastName = "Clark",  }
        };
        var uowMock = CreateUnitOfWorkBackedByInMemoryDb(seed);

        var getCheckoutedHotelsService = new GetCheckoutedHotelsService();

        // Act
        var result = getCheckoutedHotelsService.GetCheckoutedHotels(uowMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result.Select(b => b.Id).Should().BeEquivalentTo(new[] { 10, 20, 30 });
        result.Select(b => b.HotelModel?.HotelName).Should().Contain(new[] { "Hotel A", "Hotel B" });
    }

    [Fact]
    public void GetCheckoutedHotelsWhenNoDataReturnsEmptyList()
    {
        // Arrange
        var uowMock = CreateUnitOfWorkBackedByInMemoryDb(new List<AdminPanelBooking>());
        var getCheckoutedHotelsService = new GetCheckoutedHotelsService();

        // Act
        var result = getCheckoutedHotelsService.GetCheckoutedHotels(uowMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetCheckoutedHotelsUsesRepositoryOnce()
    {
        // Arrange
        var uowMock = CreateUnitOfWorkBackedByInMemoryDb(new List<AdminPanelBooking>
        {
            new AdminPanelBooking {
                Id = 1,
                ClientEmail = "test.user@example.com",
                ClientFirstName = "Test",
                ClientLastName = "User",
                HotelModel = new HotelModel { Id = 1, HotelName = "H", Address = "1 Test St", City = "Test City", Country = "Testland" }
            }
        });
        
        uowMock.Invocations.Clear();
        
        var getCheckoutedHotelsService = new GetCheckoutedHotelsService();

        // Act
        getCheckoutedHotelsService.GetCheckoutedHotels(uowMock.Object);

        // Assert
        uowMock.Verify(u => u.Repository<AdminPanelBooking>(), Times.Once);
    }
}