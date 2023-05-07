namespace HotelBooking.Data.Entities.Request
{
    public class CheckInAndCheckOut
    {
        public CheckInAndCheckOut(DateOnly checkIn, DateOnly checkOut)
        {
            CheckIn = checkIn;
            CheckOut = checkOut;
        }

        public DateOnly CheckIn;
        public DateOnly CheckOut;
    }
}
