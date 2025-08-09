using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Services.ViewModels
{
    public class ApiDataViewModel
    {
        
        public string City { get; set; }
        public DateTime CheckinDate { get; set; }
        public DateTime CheckoutDate { get; set; }
        
        public int AdultsNumber { get; set; }
        
        public int ChildrenNumber { get; set; }
        
        public int RoomsNumber { get; set; }
        public double MinPrice { get; set; } = 0;
        public double MaxPrice { get; set; } = 800;

        
    }
}
