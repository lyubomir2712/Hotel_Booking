using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Services.ViewModels
{
        public class BookingViewModel
        {
            public int HotelId { get; set; } 
            public string Name { get; set; }
            
            public string Country { get; set; }
            
            public string City { get; set; }
            
            public string Address { get; set; }
            
            public double? Latitude { get; set; }
            
            public double? Longitude { get; set; }
            
            public double? DistanceToCenter { get; set; }
            public string PhotoMainUrl { get; set; }       
            public double Price { get; set; }
            public double? ReviewScore { get; set; } = 0;
            public string? ReviewScoreWord { get; set; }           
            public string StartAt { get; set; }
            public string EndAt { get; set; }
            public int? ReviewsCount { get; set; }
            public int AdultsNumber { get; set; }
            public int? ChildrenNumber { get; set; }
            
            public bool? IsFreeCancellable { get; set; }
            
            public bool? IsNoPrepayment { get; set; }
            
            public bool? IncludesBreakfast { get; set; }
            
            public bool? HasFreeParking { get; set; }
            
            public string? AccommodationType { get; set; }
            
            public bool? IsBeachFront { get; set; }
            
        }  
}
