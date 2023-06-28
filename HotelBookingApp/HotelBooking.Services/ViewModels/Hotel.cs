using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Services.ViewModels
{
    
       

        public class Hotel
        {
            public string Name { get; set; }
            public string PhotoMainUrl { get; set; }       
            public double Price { get; set; }
            public string Currency { get; set; }
            public double? ReviewScore { get; set; } = 0;
            public string ReviewScoreWord { get; set; }           
            public string Stars { get; set; }
            public string Facilities { get; set; }
            public string StartAt { get; set; }
            public string EndAt { get; set; }
    }  
}
