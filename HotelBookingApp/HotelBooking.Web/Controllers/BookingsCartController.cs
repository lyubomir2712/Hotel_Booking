using HotelBooking.Data;
using Microsoft.AspNetCore.Mvc;
using HotelBooking.Services.ViewModels;
using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using HotelBooking.Data.SeedWork;
using HotelBooking.Services.Contracts.EmailServicesContracts;
using HotelBooking.Services.Contracts.HotelsServicesContracts;
using HotelBooking.Services.EmailServices;
using Microsoft.AspNetCore.SignalR;
using HotelBooking.Web.Hubs;
using System.Threading.Tasks;
using HotelBooking.Services.Contracts.BookingApiConfigurationContracts;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Web.Controllers
{
    public class BookingsCartController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IGetBookingsService _getBookingsService;
        private readonly IAddToCartService _addToCartService;
        private readonly IRemoveBookingService _removeBookingService;
        private readonly ICheckoutBookingsService _checkoutBookingsService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICheckoutEmailService _checkoutEmailService;
        private readonly IHubContext<AdminNotificationsHub> _adminNotificationsHubContext;
        private readonly IHubContext<HotelRecommendationsHub> _hotelRecommendationsHubContext;
        private readonly IGetUnavailableBookingsService _getUnavailableBookingsService;
        private readonly IGetBookingRecommendationsService _getBookingRecommendationsService;
        private readonly IGetUnavailableBookingHotelNamesFromUserCartService
            _getUnavailableBookingHotelNamesFromUserCartService;

        public BookingsCartController(IUnitOfWork unitOfWork, UserManager<UserModel> userManager,
             IGetBookingsService getBookingsService, IAddToCartService addToCartService,
             IRemoveBookingService removeBookingService, ICheckoutBookingsService checkoutBookingsService,
             ICheckoutEmailService checkoutEmailService, IHubContext<AdminNotificationsHub> adminNotificationsHubContext,
             IGetUnavailableBookingsService getUnavailableBookingsService, IGetBookingRecommendationsService getBookingRecommendationsService,
             IHubContext<HotelRecommendationsHub> hotelRecommendationsHubContext,
             IGetUnavailableBookingHotelNamesFromUserCartService getUnavailableBookingHotelNamesFromUserCartService)
        {
            _userManager = userManager;
            _getBookingsService = getBookingsService;
            _addToCartService = addToCartService;
            _removeBookingService = removeBookingService;
            _checkoutBookingsService = checkoutBookingsService;
            _unitOfWork = unitOfWork;
            _checkoutEmailService = checkoutEmailService;
            _adminNotificationsHubContext = adminNotificationsHubContext;
            _getUnavailableBookingsService = getUnavailableBookingsService;
            _getBookingRecommendationsService = getBookingRecommendationsService;
            _hotelRecommendationsHubContext = hotelRecommendationsHubContext;
            _getUnavailableBookingHotelNamesFromUserCartService = getUnavailableBookingHotelNamesFromUserCartService;
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(AddToCartInput addToCartInput) 
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser is null) return Challenge();

            await _addToCartService.AddToCartAsync(_unitOfWork, addToCartInput, currentUser);
            
            return NoContent();
        }
        
        [HttpPost]
        public async Task<IActionResult> RemoveHotel(int bookingId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser is null) return Challenge();
            await _removeBookingService.RemoveHotelAsync(_unitOfWork, bookingId, currentUser);
            return RedirectToAction(nameof(GetBookedHotels));
        }
        
        [HttpGet]
        public async Task<IActionResult> GetBookedHotels()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != null)
            {
                var bookings = await _getBookingsService.GetBookings(_unitOfWork, userId);

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
                return NoContent();

            var unavailableBookings = await _getUnavailableBookingsService.VerifyBookingsForAvailability(bookings);

            if (unavailableBookings.Any())
            {
                var unavailableBookingHotelNames =
                    _getUnavailableBookingHotelNamesFromUserCartService.GetUnavailableBookingHotelNamesFromUserCart(
                        unavailableBookings, currentUser);
                
                var recommendedBookings = await _getBookingRecommendationsService.GetBookingRecomendations(unavailableBookings);
                
                if (recommendedBookings.Any())
                {
                    var hotelPayload = new
                    {
                        RecommendedHotels = recommendedBookings,
                        unavailableBookingHotelNames = unavailableBookingHotelNames
                    };

                    await _hotelRecommendationsHubContext.Clients.User(currentUser.Id.ToString())
                        .SendAsync("ReceiveUnavailableBookingsAndBookingRecommendation", hotelPayload);
                    return NoContent();
                }
                
                await _hotelRecommendationsHubContext.Clients.User(currentUser.Id.ToString())
                        .SendAsync("ReceiveUnavailableBookings", unavailableBookingHotelNames);
                return NoContent();
            }

            await _checkoutBookingsService.CheckoutBookingsAsync(_unitOfWork, currentUser, bookings);
            
            await _checkoutEmailService.SendCheckoutSummaryAsync(_unitOfWork, currentUser, bookings);

            await _adminNotificationsHubContext.Clients.All
                .SendAsync("sendAdminNotificationForCheckout",
                    $"A customer has made {bookings.Count} new {(bookings.Count > 1 ? "bookings" : "booking")}");

            return RedirectToAction("GetBookedHotels", "BookingsCart");
        }
    }
}
