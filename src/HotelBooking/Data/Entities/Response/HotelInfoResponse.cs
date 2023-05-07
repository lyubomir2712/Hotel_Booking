using HotelBooking.Controllers;

namespace HotelBooking.Data.Entities.Response
{
    public class HotelInfoResponse
    {
        public string Name { get; set; }
        public HotelImageResponse HotelMainImage { get; set; }
        public PriceForHoliday[] Rooms { get; set; }
        public FacilityResponse[] Facilities { get; set; }
    }
}
