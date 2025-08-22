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
using System.Text;

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
        private readonly IEmailSender _emailSender;
        private readonly IEmailTemplatePathProviderService _emailTemplatePathProviderService;
        private readonly IGetEmailTemplateFromPathService _getEmailTemplateFromPathService;
        private readonly IGetEmailTemplateHtmlWithParametersService _getEmailTemplateHtmlWithParametersService;

        public BookingsCartController(IUnitOfWork unitOfWork, UserManager<UserModel> userManager,
             IGetBookingsService getBookingsService, IAddToCartService addToCartService,
             IRemoveBookingService removeBookingService, ICheckoutBookingsService checkoutBookingsService,
             IEmailSender emailSender, IEmailTemplatePathProviderService emailTemplatePathProviderService,
             IGetEmailTemplateFromPathService getEmailTemplateFromPathService,
             IGetEmailTemplateHtmlWithParametersService getEmailTemplateHtmlWithParametersService)
        {
            _userManager = userManager;
            _getBookingsService = getBookingsService;
            _addToCartService = addToCartService;
            _removeBookingService = removeBookingService;
            _checkoutBookingsService = checkoutBookingsService;
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _emailTemplatePathProviderService = emailTemplatePathProviderService;
            _getEmailTemplateFromPathService = getEmailTemplateFromPathService;
            _getEmailTemplateHtmlWithParametersService = getEmailTemplateHtmlWithParametersService;
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

            if (currentUser == null|| bookings == null || bookings.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            
            string? emailReceiver = currentUser.Email;
                
            if (string.IsNullOrWhiteSpace(emailReceiver))
            {
                throw new InvalidOperationException("User does not have a valid email address.");
            }
            
            string subject = "Successfully Checked Out Bookings";
            
            string checkoutBookingsEmailTemplatePath = _emailTemplatePathProviderService.CheckoutBookingsEmailTemplatePath;
            
            string template = await _getEmailTemplateFromPathService.GetEmailTemplateFromPath(checkoutBookingsEmailTemplatePath);

            var sb = new StringBuilder();
            sb.AppendLine("<div style=\"font-family:Arial, sans-serif;\">");
            sb.AppendLine("  <h2>Booking summary</h2>");
            sb.AppendLine("  <div>");

            foreach (var booking in bookings)
            {
                var hotel = await _unitOfWork.Repository<HotelModel>()
                    .FirstOrDefaultAsync(hotel => hotel.Id == booking.HotelModelId);

                var snippet = _getEmailTemplateHtmlWithParametersService
                    .GetEmailTemplateHtmlWithParameters(template, currentUser, booking, hotel);

                // Append each booking's rendered template
                sb.AppendLine(snippet);
                sb.AppendLine("<hr style=\"margin:16px 0; border:none; border-top:1px solid #ddd;\">");
            }

            sb.AppendLine("  </div>");
            sb.AppendLine("  <p style=\"font-size:12px;color:#666;\">This is a consolidated email for your recent checkout.</p>");
            sb.AppendLine("</div>");

            var combinedBody = sb.ToString();

            // Send a single email containing all bookings
            await _emailSender.SendAsync(emailReceiver, subject, combinedBody);

            await _checkoutBookingsService.CheckoutBookingsAsync(_unitOfWork, currentUser, bookings);
            
            return RedirectToAction("GetBookedHotels", "BookingsCart");
        }
    }
}
