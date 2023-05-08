using HotelBooking.Data.Entities.Response;

namespace HotelBooking.Data.Entities.UsersRooms
{
    public class Room
    {
        public string Name { get; set; }
        public HotelImageResponse HotelMainImage { get; set; }
        public PriceForHolidayResponse[] Rooms { get; set; }
        public FacilityResponse[] Facilities { get; set; }

    }
}
