using FluentAssertions;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Services.AdminPanelServices;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace HotelBooking.Tests.UnitTests;

public class AdminPanelUnitTests
{
    private class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }
        public DbSet<AdminPanelBooking> AdminPanelBookings => Set<AdminPanelBooking>();
        public DbSet<HotelModel> Hotels => Set<HotelModel>();
    }
    
    private static (Mock<IUnitOfWork> uowMock, TestDbContext ctx) CreateUnitOfWorkBackedByInMemoryDb(string dbName)
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        var testDbContext = new TestDbContext(options);
        
        
        var uowMock = new Mock<IUnitOfWork>();

        var backingQueryable = testDbContext.Set<AdminPanelBooking>().AsQueryable();
        
        var mockedAdminPanelBookingRepo = new Mock<IRepository<AdminPanelBooking>>();

        mockedAdminPanelBookingRepo.As<IQueryable<AdminPanelBooking>>()
            .Setup(m => m.Provider)
            .Returns(backingQueryable.Provider);
        mockedAdminPanelBookingRepo.As<IQueryable<AdminPanelBooking>>()
            .Setup(m => m.Expression)
            .Returns(backingQueryable.Expression);
        mockedAdminPanelBookingRepo.As<IQueryable<AdminPanelBooking>>()
            .Setup(m => m.ElementType)
            .Returns(backingQueryable.ElementType);
        mockedAdminPanelBookingRepo.As<IQueryable<AdminPanelBooking>>()
            .Setup(m => m.GetEnumerator())
            .Returns(() => backingQueryable.GetEnumerator());

        uowMock
            .Setup(u => u.Repository<AdminPanelBooking>())
            .Returns(mockedAdminPanelBookingRepo.Object);

        return (uowMock, testDbContext);
    }

    [Fact]
    public void GetCheckoutedHotels_Returns_All_Bookings_With_Hotel_Navigation()
    {
        // Arrange
        var (uowMock, ctx) = CreateUnitOfWorkBackedByInMemoryDb(nameof(GetCheckoutedHotels_Returns_All_Bookings_With_Hotel_Navigation));

        var hotelA = new HotelModel { Id = 1, HotelName = "Hotel A", Address = "123 A St", City = "A City", Country = "A Country" };
        var hotelB = new HotelModel { Id = 2, HotelName = "Hotel B", Address = "456 B Ave", City = "B City", Country = "B Country" };
        ctx.Add(hotelA);
        ctx.Add(hotelB);

        var seed = new List<AdminPanelBooking>
        {
            new AdminPanelBooking { Id = 10, HotelModel = hotelA, ClientEmail = "alice@example.com", ClientFirstName = "Alice", ClientLastName = "Anderson",  },
            new AdminPanelBooking { Id = 20, HotelModel = hotelB, ClientEmail = "bob@example.com", ClientFirstName = "Bob", ClientLastName = "Brown", },
            new AdminPanelBooking { Id = 30, HotelModel = hotelA, ClientEmail = "carol@example.com", ClientFirstName = "Carol", ClientLastName = "Clark",  }
        };
        ctx.AddRange(seed);
        ctx.SaveChanges();

        var sut = new GetCheckoutedHotelsService();

        // Act
        var result = sut.GetCheckoutedHotels(uowMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result.Select(b => b.Id).Should().BeEquivalentTo(new[] { 10, 20, 30 });
        result.Select(b => b.HotelModel?.HotelName).Should().Contain(new[] { "Hotel A", "Hotel B" });
    }

    [Fact]
    public void GetCheckoutedHotels_When_No_Data_Returns_Empty_List()
    {
        // Arrange
        var (uowMock, _) = CreateUnitOfWorkBackedByInMemoryDb(nameof(GetCheckoutedHotels_When_No_Data_Returns_Empty_List));
        var sut = new GetCheckoutedHotelsService();

        // Act
        var result = sut.GetCheckoutedHotels(uowMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetCheckoutedHotels_Uses_Repository_Once()
    {
        // Arrange
        var (uowMock, ctx) = CreateUnitOfWorkBackedByInMemoryDb(nameof(GetCheckoutedHotels_Uses_Repository_Once));
        ctx.Add(new AdminPanelBooking {
            Id = 1,
            ClientEmail = "test.user@example.com",
            ClientFirstName = "Test",
            ClientLastName = "User",
            HotelModel = new HotelModel { Id = 1, HotelName = "H", Address = "1 Test St", City = "Test City", Country = "Testland" }
        });
        ctx.SaveChanges();
        var sut = new GetCheckoutedHotelsService();

        // Act
        var _ = sut.GetCheckoutedHotels(uowMock.Object);

        // Assert
        uowMock.Verify(u => u.Repository<AdminPanelBooking>(), Times.Once);
    }
}