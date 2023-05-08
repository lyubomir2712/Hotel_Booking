namespace HotelBooking.Data.Entities
{
    public class Hotel
    {
        public string Name { get; set; }
        public double PricePerNight { get; set; }
        public double PriceForStay { get; set; }
        public int Popularity { get; set; }
        public List<string> Facilities { get; set; }
    }
}
