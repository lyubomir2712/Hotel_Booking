using HotelBooking.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Models.AppModels
{
    public class BookingModel : BaseModel
    {
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public double Price { get; set; }

        public int AdultsNumber { get; set; } = 1;

        public int? ChildrenNumber { get; set; } = 0;

        public int RoomsNumber { get; set; } = 1;
        
        public int RapidApiHotelId { get; set; }
        
        public int HotelModelId  {get; set;}
        public HotelModel HotelModel { get; set; }
        public ICollection<UserBookingModel> UserBookingModels { get; set; }
    }
}
