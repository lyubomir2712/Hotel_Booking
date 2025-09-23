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

namespace HotelBooking.Tests.UnitTests;

public class AdminPanelUnitTests
{
    private class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }
        public DbSet<AdminPanelBooking> AdminPanelBookings { get; set; }
        public DbSet<HotelModel> Hotels { get; set; }
    }
    
    private sealed class FakeRepository<T> : IRepository<T>, IQueryable<T> where T : class
    {
        private readonly List<T> _data = new();

        public Type ElementType => _data.AsQueryable().ElementType;
        public Expression Expression => _data.AsQueryable().Expression;
        public IQueryProvider Provider => _data.AsQueryable().Provider;

        public IEnumerator<T> GetEnumerator() => _data.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IQueryable<T> Query() => _data.AsQueryable();

        public IQueryable<T> Include(params Expression<Func<T, object>>[] includes) => _data.AsQueryable();

        public Task<T?> GetByIdAsync(object id, CancellationToken ct = default)
            => Task.FromResult(_data.FirstOrDefault());

        public Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
            => Task.FromResult(_data.AsQueryable().FirstOrDefault(predicate));

        public Task<T?> FindAsync(int id, CancellationToken ct = default)
            => Task.FromResult(_data.FirstOrDefault());

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
            => _data.AsQueryable().Where(predicate);

        public Task<List<T>> ListAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default)
            => Task.FromResult(predicate is null ? _data.ToList() : _data.AsQueryable().Where(predicate).ToList());

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
            => Task.FromResult(_data.AsQueryable().Any(predicate));

        public Task AddAsync(T entity, CancellationToken ct = default)
        {
            _data.Add(entity);
            return Task.CompletedTask;
        }

        public Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default)
        {
            _data.AddRange(entities);
            return Task.CompletedTask;
        }

        public void Update(T entity)
        {
            if (entity == null) return;

            var idProp = typeof(T).GetProperty("Id");
            if (idProp != null)
            {
                var idValue = idProp.GetValue(entity);
                var indexById = _data.FindIndex(e =>
                {
                    var otherId = idProp.GetValue(e);
                    return Equals(otherId, idValue);
                });

                if (indexById >= 0)
                {
                    _data[indexById] = entity; 
                    return;
                }
            }

            var existingIndex = _data.FindIndex(e => ReferenceEquals(e, entity) || Equals(e, entity));
            if (existingIndex >= 0)
            {
                _data[existingIndex] = entity;
            }
        }

        public void Remove(T entity) => _data.Remove(entity);

        public Task RemoveAsync(T entity, CancellationToken ct = default)
        {
            _data.Remove(entity);
            return Task.CompletedTask;
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            foreach (var e in entities) _data.Remove(e);
        }
    }
    
    private static (Mock<IUnitOfWork> uowMock, TestDbContext testDbContext) CreateUnitOfWorkBackedByInMemoryDb(string dbName)
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        var testDbContext = new TestDbContext(options);
        
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
                Task.FromResult((pred == null ? seed : seed.AsQueryable().Where(pred)).ToList())
            );
        
        var repo = new FakeRepository<AdminPanelBooking>();
        uowMock
            .Setup(u => u.Repository<AdminPanelBooking>())
            .Returns(repo);

        return (uowMock, testDbContext);
    }

    [Fact]
    public async Task GetCheckoutedHotelsReturnsAllBookingsWithHotelJoin()
    {
        // Arrange
        var (uowMock, ctx) = CreateUnitOfWorkBackedByInMemoryDb(nameof(GetCheckoutedHotelsReturnsAllBookingsWithHotelJoin));

        var hotelA = new HotelModel { Id = 1, HotelName = "Hotel A", Address = "123 A St", City = "A City", Country = "A Country" };
        var hotelB = new HotelModel { Id = 2, HotelName = "Hotel B", Address = "456 B Ave", City = "B City", Country = "B Country" };

        var seed = new List<AdminPanelBooking>
        {
            new AdminPanelBooking { Id = 10, HotelModel = hotelA, ClientEmail = "alice@example.com", ClientFirstName = "Alice", ClientLastName = "Anderson",  },
            new AdminPanelBooking { Id = 20, HotelModel = hotelB, ClientEmail = "bob@example.com", ClientFirstName = "Bob", ClientLastName = "Brown", },
            new AdminPanelBooking { Id = 30, HotelModel = hotelA, ClientEmail = "carol@example.com", ClientFirstName = "Carol", ClientLastName = "Clark",  }
        };
        var repo = uowMock.Object.Repository<AdminPanelBooking>();
        await repo.AddRangeAsync(seed);

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
        var (uowMock, _) = CreateUnitOfWorkBackedByInMemoryDb(nameof(GetCheckoutedHotelsWhenNoDataReturnsEmptyList));
        var getCheckoutedHotelsService = new GetCheckoutedHotelsService();

        // Act
        var result = getCheckoutedHotelsService.GetCheckoutedHotels(uowMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetCheckoutedHotelsUsesRepository_Once()
    {
        // Arrange
        var (uowMock, ctx) = CreateUnitOfWorkBackedByInMemoryDb(nameof(GetCheckoutedHotelsUsesRepository_Once));
        
        var repo = uowMock.Object.Repository<AdminPanelBooking>();
        
        await repo.AddAsync(new AdminPanelBooking {
            Id = 1,
            ClientEmail = "test.user@example.com",
            ClientFirstName = "Test",
            ClientLastName = "User",
            HotelModel = new HotelModel { Id = 1, HotelName = "H", Address = "1 Test St", City = "Test City", Country = "Testland" }
        });
        
        uowMock.Invocations.Clear();
        
        var getCheckoutedHotelsService = new GetCheckoutedHotelsService();

        // Act
        getCheckoutedHotelsService.GetCheckoutedHotels(uowMock.Object);

        // Assert
        uowMock.Verify(u => u.Repository<AdminPanelBooking>(), Times.Once);
    }
}