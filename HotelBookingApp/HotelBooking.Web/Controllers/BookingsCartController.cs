using HotelBooking.Data;
using Microsoft.AspNetCore.Mvc;
using HotelBooking.Services.ViewModels;
using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using HotelBooking.Services.HotelAddService;

namespace HotelBooking.Web.Controllers
{
    public class BookingsCartController : Controller
    {
        private readonly BookingDbContext _dbContext;
        private readonly UserManager<UserModel> _userManager;
        private readonly HotelAddService _hotelAddService;

        public BookingsCartController(BookingDbContext dbContext, UserManager<UserModel> userManager, HotelAddService hotelAddService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _hotelAddService = hotelAddService;
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(string HotelName, string HotelImg, string HotelPrice, string StartAt, string EndAt) 
        {
            await _dbContext.FixBookingsIdentityAsync();

            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Retrieve the UserModel based on the current user's ID
            UserModel userModel = await _userManager.FindByIdAsync(userId.ToString());

            var existingHotel = _dbContext.Hotels.FirstOrDefault(h => h.HotelName == HotelName && h.HotelImg == HotelImg);
            
            HotelModel newHotel;
            if (existingHotel != null)
            {
                newHotel = existingHotel;
            }
            else
            {
                newHotel = new HotelModel { HotelName = HotelName, HotelImg = HotelImg };
                _dbContext.Hotels.Add(newHotel);
                await _dbContext.SaveChangesAsync();
            }

            BookingModel newBookingModel = new BookingModel
            {
                StartAt = Convert.ToDateTime(StartAt),
                Price = Convert.ToDouble(HotelPrice),
                EndAt = Convert.ToDateTime(EndAt),
                HotelModel = newHotel,
                HotelModelId = newHotel.Id
            };

            await _dbContext.Bookings.AddAsync(newBookingModel);
            await _dbContext.SaveChangesAsync();     // get generated BookingModel.Id

            var newUserBookingModel = new UserBookingModel
            {
                BookingModelId = newBookingModel.Id,   // now valid
                UserId         = userId,
                UserModel      = userModel
            };
            await _dbContext.UserBookings.AddAsync(newUserBookingModel);
            await _dbContext.SaveChangesAsync();


            return RedirectToAction("Index", "Home");

        }
        
        public IActionResult RemoveHotel(int BookingId)
        {
            _dbContext.Bookings.Remove(_dbContext.Bookings.First(b=>b.Id == BookingId));
            _dbContext.SaveChanges();
            return RedirectToAction("BookedHotels");
        }
        
        public IActionResult BookedHotels()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<BookingModel> bookings = _hotelAddService.GetBookedHotels(userId);
            List<HotelModel> hotels = _hotelAddService.GetHotels(bookings);

            UserBookedHotels userBookedHotels = new UserBookedHotels 
            {
                Hotels = hotels,
                Bookings = bookings
            };
            
            return View("~/Views/Home/BookedHotels.cshtml", userBookedHotels);

        }
    }
}
