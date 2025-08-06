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
        
        public string HotelName { get; set; }
        public string HotelImg { get; set; }
        public ICollection<BookingModel> BookingModels { get; set; }
        
        public ICollection<AdminPanelBookings> AdminPanelBookings { get; set; } = new List<AdminPanelBookings>();
    }
}
