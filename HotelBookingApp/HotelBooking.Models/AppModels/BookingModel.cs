using HotelBooking.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Models.AppModels
{
    public class BookingModel:BaseModel
    {
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public int HotelId { get; set; }
        public HotelModel HotelModel { get; set; }
        public ICollection<UserBookingModel> UserBookingModels { get; set; }
    }
}
