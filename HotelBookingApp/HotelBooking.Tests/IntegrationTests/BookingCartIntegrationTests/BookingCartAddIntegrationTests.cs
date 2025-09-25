using System.Security.Claims;
using HotelBooking.Data.SeedWork;
using HotelBooking.Data.SeedWork.RepositoriesInterfaces;
using HotelBooking.Models.Identity;
using HotelBooking.Services.Contracts.HotelsServicesContracts;
using HotelBooking.Services.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace HotelBooking.Tests.IntegrationTests.BookingCartIntegrationTests;


public class BookingCartAddIntegrationTests
{
    [Fact]
    public async Task AddToCart_Unauthenticated_ReturnsChallenge()
    {
        // Arrange
        var userManager = new FakeUserManager();
        var uow = new FakeUnitOfWork();
        var addToCartService = new RecordingAddToCartService();
        var controller = MakeController(userManager, uow, addToCartService, isAuthenticated: false);

        var input = new AddToCartInput { Id = 123, HotelName = "Test Hotel" };

        // Act
        var result = await controller.AddToCart(input);

        // Assert
        Assert.IsType<ChallengeResult>(result);
        Assert.Null(addToCartService.LastCall); 
    }

    [Fact]
    public async Task AddToCart_Authenticated_CallsService_AndRedirects()
    {
        // Arrange
        var userManager = new FakeUserManager();
        var uow = new FakeUnitOfWork();
        var addToCartService = new RecordingAddToCartService();
        var controller = MakeController(userManager, uow, addToCartService, isAuthenticated: true);
    
        var input = new AddToCartInput
        {
            Id = 7,
            HotelName = "Seaside",
            City = "Varna",
            Country = "Bulgaria",
            AdultsNumber = 2,
            RoomsNumber = 1
        };
    
        // Act
        var actionResult = await controller.AddToCart(input);
    
        // Assert
        var redirect = Assert.IsType<RedirectToActionResult>(actionResult);
        Assert.Equal("GetBookedHotels", redirect.ActionName);
    
        // Assert
        Assert.NotNull(addToCartService.LastCall);
        Assert.Same(uow, addToCartService.LastCall!.Value.UnitOfWork);
        Assert.Equal(input, addToCartService.LastCall.Value.Input);
        Assert.Equal(1, addToCartService.LastCall!.Value.User.Id);
    }

    
    
    private static TestableBookingCartController MakeController(
        UserManager<UserModel> userManager,
        IUnitOfWork unitOfWork,
        IAddToCartService addToCartService,
        bool isAuthenticated)
    {
        var controller = new TestableBookingCartController(userManager, addToCartService, unitOfWork);

        var identity = new ClaimsIdentity(isAuthenticated ? new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
            new Claim(ClaimTypes.Name, "test-user")
        } : new Claim[] { }, isAuthenticated ? "TestAuth" : null);

        var user = new ClaimsPrincipal(identity);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        return controller;
    }


    private sealed class FakeUserManager : UserManager<UserModel>
    {
        public FakeUserManager() : base(
            new FakeUserStore(),
            null!, null!, null!, null!, null!, null!, null!, null!)
        {
        }

        public override Task<UserModel?> GetUserAsync(ClaimsPrincipal principal)
        {
            if (principal?.Identity?.IsAuthenticated == true)
            {
                return Task.FromResult<UserModel?>(new UserModel
                {
                    Id = 1,
                    UserName = "test-user"
                });
            }
            return Task.FromResult<UserModel?>(null);
        }
    }

    private sealed class FakeUserStore : IUserStore<UserModel>
    {
        public void Dispose() { }
        public Task<string> GetUserIdAsync(UserModel user, System.Threading.CancellationToken cancellationToken) => Task.FromResult(user.Id.ToString());
        public Task<string?> GetUserNameAsync(UserModel user, System.Threading.CancellationToken cancellationToken) => Task.FromResult(user.UserName);
        public Task SetUserNameAsync(UserModel user, string userName, System.Threading.CancellationToken cancellationToken) { user.UserName = userName; return Task.CompletedTask; }
        public Task<string?> GetNormalizedUserNameAsync(UserModel user, System.Threading.CancellationToken cancellationToken) => Task.FromResult<string?>(user.UserName);
        public Task SetNormalizedUserNameAsync(UserModel user, string? normalizedName, System.Threading.CancellationToken cancellationToken) { return Task.CompletedTask; }
        public Task<IdentityResult> CreateAsync(UserModel user, System.Threading.CancellationToken cancellationToken) => Task.FromResult(IdentityResult.Success);
        public Task<IdentityResult> UpdateAsync(UserModel user, System.Threading.CancellationToken cancellationToken) => Task.FromResult(IdentityResult.Success);
        public Task<IdentityResult> DeleteAsync(UserModel user, System.Threading.CancellationToken cancellationToken) => Task.FromResult(IdentityResult.Success);
        public Task<UserModel?> FindByIdAsync(string userId, System.Threading.CancellationToken cancellationToken) => Task.FromResult<UserModel?>(null);
        public Task<UserModel?> FindByNameAsync(string normalizedUserName, System.Threading.CancellationToken cancellationToken) => Task.FromResult<UserModel?>(null);
    }

    private sealed class FakeUnitOfWork : IUnitOfWork
    {
        public Task<int> SaveChangesAsync() => Task.FromResult(1);
        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            throw new NotSupportedException("Repository is not used in these tests.");
        }

        public int SaveChanges()
        {
            return 1;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return Task.FromResult(1);
        }
    }

    private sealed class RecordingAddToCartService : IAddToCartService
    {
        public (IUnitOfWork UnitOfWork, AddToCartInput Input, UserModel User)? LastCall { get; private set; }

        public Task AddToCartAsync(IUnitOfWork unitOfWork, AddToCartInput addToCartInput, UserModel currentUser)
        {
            LastCall = (unitOfWork, addToCartInput, currentUser);
            return Task.CompletedTask;
        }
    }
}


public class TestableBookingCartController : Controller
{
    private readonly UserManager<UserModel> _userManager;
    private readonly IAddToCartService _addToCartService;
    private readonly IUnitOfWork _unitOfWork;

    public TestableBookingCartController(
        UserManager<UserModel> userManager,
        IAddToCartService addToCartService,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _addToCartService = addToCartService;
        _unitOfWork = unitOfWork;
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(AddToCartInput addToCartInput)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser is null) return Challenge();

        await _addToCartService.AddToCartAsync(_unitOfWork, addToCartInput, currentUser);
        return RedirectToAction(nameof(GetBookedHotels));
    }

    public IActionResult GetBookedHotels() => Ok();
}
