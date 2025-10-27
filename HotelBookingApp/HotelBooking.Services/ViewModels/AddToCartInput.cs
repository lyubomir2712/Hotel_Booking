using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Services.ViewModels
{
    public class AddToCartInput
    {
        public int Id { get; set; }
        public string HotelName { get; set; }
        public string hotelImg { get; set; }
        public string HotelPrice { get; set;}
        public string StartAt { get; set;}
        public string EndAt { get; set;}
        
        public string City { get; set; }
        
        public string Country { get; set; }
        
        public string Address { get; set; }
        
        public double? ReviewScore { get; set; } = 0;
        
        public string? ReviewScoreWord { get; set; }           
        
        public int? ReviewsCount { get; set; }

        public int AdultsNumber { get; set; } = 1;

        public int? ChildrenNumber { get; set; } = 0;

        public int RoomsNumber { get; set; } = 1;
        
        public int RapidApiHotelId { get; set; }
    }
}
