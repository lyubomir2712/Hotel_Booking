namespace HotelBooking.Data.Entities.Request
{
    public class PriceRangePerNight
    {
        public PriceRangePerNight(int maxPrice)
        {
            MaxPrice = maxPrice;
        }
        public int MinPrice { get; } = 50;
        public int MaxPrice { get; set; }
    }
}
