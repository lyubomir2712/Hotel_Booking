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
        public ICollection<BookingModel> BookingModels { get; set; }
        
        public ICollection<AdminPanelBookings> AdminPanelBookings { get; set; } = new List<AdminPanelBookings>();
    }
}
