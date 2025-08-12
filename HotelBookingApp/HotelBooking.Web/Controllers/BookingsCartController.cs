using HotelBooking.Data;
using Microsoft.AspNetCore.Mvc;
using HotelBooking.Services.ViewModels;
using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using HotelBooking.Services.Contracts.HotelsServicesContracts;

namespace HotelBooking.Web.Controllers
{
    public class BookingsCartController : Controller
    {
        private readonly BookingDbContext _bookingDbContext;
        private readonly UserManager<UserModel> _userManager;
        private readonly IGetHotelsService _getHotelsService;
        private readonly IGetBookedHotelsService _getBookedHotelsService;
        private readonly IAddToCartService _addToCartService;
        private readonly IRemoveBookingService _removeBookingService;

        public BookingsCartController(BookingDbContext bookingDbContext, UserManager<UserModel> userManager,
             IGetHotelsService getHotelsService, IGetBookedHotelsService getBookedHotelsService,
             IAddToCartService addToCartService, IRemoveBookingService removeBookingService)
        {
            _bookingDbContext = bookingDbContext;
            _userManager = userManager;
            _getHotelsService = getHotelsService;
            _getBookedHotelsService = getBookedHotelsService;
            _addToCartService = addToCartService;
            _removeBookingService = removeBookingService;
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(AddToCartInput addToCartInput) 
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser is null) return Challenge();

            await _addToCartService.AddToCartAsync(_bookingDbContext, addToCartInput, currentUser);
            
            return RedirectToAction(nameof(GetBookedHotels));
        }
        
        [HttpPost]
        public async Task<IActionResult> RemoveHotel(int bookingId)
        {
            await _removeBookingService.RemoveHotelAsync(_bookingDbContext, bookingId);
            return RedirectToAction(nameof(GetBookedHotels));
        }
        
        public IActionResult GetBookedHotels()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            
            var bookings = _getBookedHotelsService.GetBookedHotels(_bookingDbContext, userId);
            
            
            var hotels = _getHotelsService.GetHotels(_bookingDbContext,bookings);

            UserBookedHotels userBookedHotels = new UserBookedHotels 
            {
                Hotels = hotels,
                Bookings = bookings
            };
            
            return View("~/Views/Hotels/BookedHotels.cshtml", userBookedHotels);

        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckoutHotels(List<BookingModel> bookings)
        {
            // 1. Вземаме текущия потребител
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || bookings == null || bookings.Count == 0)
                return RedirectToAction("Index", "Home");

            // 2. Конвертираме всеки BookingModel в AdminPanelBookings
            var adminPanelBookings = bookings.Select(b => new AdminPanelBookings
            {
                ClientId = currentUser.Id,
                ClientFirstName = currentUser.FirstName,   
                ClientLastName = currentUser.LastName, 
                ClientEmail = currentUser.Email,
                StartAt = b.StartAt,
                EndAt = b.EndAt,
                Price = b.Price,
                
                HotelModelId = b.HotelModelId
            }).ToList();

            await _bookingDbContext.AdminPanelBookings.AddRangeAsync(adminPanelBookings);

            _bookingDbContext.Bookings.RemoveRange(bookings);

            await _bookingDbContext.SaveChangesAsync();

            return RedirectToAction("GetBookedHotels", "BookingsCart");
        }
    }
}
