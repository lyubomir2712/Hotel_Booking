
using HotelBooking.Models.AppModels;
using Microsoft.AspNetCore.Identity;

namespace HotelBooking.Models.Identity
{
    public class UserModel:IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<UserBookingModel> UserBookingModels { get; set; }
    }
}
