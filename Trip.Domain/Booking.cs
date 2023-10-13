namespace Trip.Domain
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int TripId { get; set; }
        public Trip Trip { get; set; }
        public int PassengerId { get; set; }
        public AppUser Passenger { get; set; }
        public BookingStatus Status { get; set; }
    }
}
