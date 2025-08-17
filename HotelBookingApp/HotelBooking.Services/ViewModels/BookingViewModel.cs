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
            
            public string RoomName { get; set; }
            
            public string BedConfiguration { get; set; }
            
            public double? RoomSurface { get; set; }
            
            public string DistanceToCityCenter { get; set; }
            public string PhotoMainUrl { get; set; }       
            public double Price { get; set; }
            public string Currency { get; set; }
            public double? ReviewScore { get; set; } = 0;
            public string? ReviewScoreWord { get; set; }           
            public string Stars { get; set; }
            public string StartAt { get; set; }
            public string EndAt { get; set; }
            public int? ReviewsCount { get; set; }

            public int AdultsNumber { get; set; }
            
            public int? ChildrenNumber { get; set; }
        }  
}
