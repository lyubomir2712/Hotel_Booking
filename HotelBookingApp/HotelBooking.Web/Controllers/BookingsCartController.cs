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

        public BookingsCartController(BookingDbContext bookingDbContext, UserManager<UserModel> userManager,
             IGetHotelsService getHotelsService, IGetBookedHotelsService getBookedHotelsService)
        {
            _bookingDbContext = bookingDbContext;
            _userManager = userManager;
            _getHotelsService = getHotelsService;
            _getBookedHotelsService = getBookedHotelsService;
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(string HotelName, string HotelImg, string HotelPrice, string StartAt, string EndAt) 
        {

            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Retrieve the UserModel based on the current user's ID
            UserModel userModel = await _userManager.FindByIdAsync(userId.ToString());

            var existingHotel = _bookingDbContext.Hotels.FirstOrDefault(h => h.HotelName == HotelName && h.HotelImg == HotelImg);
            
            HotelModel newHotel;
            if (existingHotel != null)
            {
                newHotel = existingHotel;
            }
            else
            {
                newHotel = new HotelModel { HotelName = HotelName, HotelImg = HotelImg };
                _bookingDbContext.Hotels.Add(newHotel);
                await _bookingDbContext.SaveChangesAsync();
            }

            BookingModel newBookingModel = new BookingModel
            {
                StartAt = Convert.ToDateTime(StartAt),
                Price = Convert.ToDouble(HotelPrice),
                EndAt = Convert.ToDateTime(EndAt),
                HotelModel = newHotel,
                HotelModelId = newHotel.Id
            };

            await _bookingDbContext.Bookings.AddAsync(newBookingModel);
            await _bookingDbContext.SaveChangesAsync();     // get generated BookingModel.Id

            var newUserBookingModel = new UserBookingModel
            {
                BookingModelId = newBookingModel.Id,   // now valid
                UserId         = userId,
                UserModel      = userModel
            };
            await _bookingDbContext.UserBookings.AddAsync(newUserBookingModel);
            await _bookingDbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
        
        public IActionResult RemoveHotel(int BookingId)
        {
            _bookingDbContext.Bookings.Remove(_bookingDbContext.Bookings.First(b=>b.Id == BookingId));
            _bookingDbContext.SaveChanges();
            return RedirectToAction("GetBookedHotels");
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
