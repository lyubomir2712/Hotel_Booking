using HotelBooking.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Models.AppModels
{
    public class HotelModel : BaseModel
    {
        
        public string HotelName { get; set; } = string.Empty;
        public string HotelImg { get; set; } = string.Empty;
        
        public string City { get; set; }
        
        public string Country { get; set; }
        
        public string Address { get; set; }
        
        public double? ReviewScore { get; set; } = 0;
        
        public int? ReviewsCount { get; set; }
        
        public string? ReviewScoreWord { get; set; }           

        public ICollection<BookingModel> BookingModels { get; set; }
        
        public ICollection<AdminPanelBooking> AdminPanelBookings { get; set; } = new List<AdminPanelBooking>();
    }
}
