using HotelBooking.Data;
using Microsoft.AspNetCore.Mvc;
using HotelBooking.Services.ViewModels;
using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using HotelBooking.Data.SeedWork;
using HotelBooking.Services.Contracts.HotelsServicesContracts;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HotelBooking.Web.Controllers
{
    public class BookingsCartController : Controller
    {
        private readonly BookingDbContext _bookingDbContext;
        private readonly UserManager<UserModel> _userManager;
        private readonly IGetBookingsService _getBookingsService;
        private readonly IAddToCartService _addToCartService;
        private readonly IRemoveBookingService _removeBookingService;
        private readonly ICheckoutBookingsService _checkoutBookingsService;
        private readonly IUnitOfWork _unitOfWork;

        public BookingsCartController(BookingDbContext bookingDbContext,IUnitOfWork unitOfWork, UserManager<UserModel> userManager,
             IGetBookingsService getBookingsService,
             IAddToCartService addToCartService, IRemoveBookingService removeBookingService,
             ICheckoutBookingsService checkoutBookingsService)
        {
            _bookingDbContext = bookingDbContext;
            _userManager = userManager;
            _getBookingsService = getBookingsService;
            _addToCartService = addToCartService;
            _removeBookingService = removeBookingService;
            _checkoutBookingsService = checkoutBookingsService;
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
        
        [HttpPost]
        public async Task<IActionResult> RemoveHotel(int bookingId)
        {
            await _removeBookingService.RemoveHotelAsync(_unitOfWork, bookingId);
            return RedirectToAction(nameof(GetBookedHotels));
        }
        
        [HttpGet]
        public IActionResult GetBookedHotels()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != null)
            {
                var bookings = _getBookingsService.GetBookings(_unitOfWork, userId);

                var userBookedHotels = new UserBookedHotels
                {
                    Bookings = bookings
                };

                return View("~/Views/Hotels/BookedHotels.cshtml", userBookedHotels);
            }

            return Challenge();
        }
        
        [HttpPost]
        public async Task<IActionResult> CheckoutHotels(List<BookingModel>? bookings)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || bookings == null || bookings.Count == 0)
                return RedirectToAction("Index", "Home");

            await _checkoutBookingsService.CheckoutBookingsAsync(_bookingDbContext, currentUser, bookings);

            return RedirectToAction("GetBookedHotels", "BookingsCart");
        }
    }
}
