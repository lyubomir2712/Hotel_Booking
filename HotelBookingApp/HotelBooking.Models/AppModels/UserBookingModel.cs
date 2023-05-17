using HotelBooking.Models.BaseModels;
using HotelBooking.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Models.AppModels
{
    public class UserBookingModel:BaseModel
    {
        public int BookingModelId { get; set; }
        public BookingModel BookingModel { get; set; }
        public int UserId { get; set; }
        public UserModel UserModel { get; set; }
    }
}
