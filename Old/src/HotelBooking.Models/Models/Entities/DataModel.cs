using HotelBooking.Models.Entities.Request;

namespace HotelBooking.Models.Entities
{
    public class DataModel
    {
        //public DataModel(string city, CheckInAndCheckOut checkinDate,
        //CheckInAndCheckOut checkoutDate, PriceRangePerNight price, bool hasPool, bool hasParking,
        //    bool hasFitness, bool hasInternet, bool hasRestaurant)
        //{
        //    City = city;
        //    CheckinDate = checkinDate;
        //    CheckoutDate = checkoutDate;
        //    Price = price;
        //    HasPool = hasPool;
        //    HasParking = hasParking;
        //    HasFitness = hasFitness;
        //    HasInternet = hasInternet;
        //    HasRestaurant = hasRestaurant;
        //}

        public string City { get; set; }
        public DateTime CheckinDate { get; set; }
        public DateTime CheckoutDate { get; set; }
        public decimal MinPrice { get; set; } = 50;
        public decimal MaxPrice { get; set; }
        public bool? HasPool { get; set; }
        public bool? HasParking { get; set; }
        public bool? HasFitness { get; set; }
        public bool? HasInternet { get; set; }
        public bool? HasRestaurant { get; set; }
    }
}
