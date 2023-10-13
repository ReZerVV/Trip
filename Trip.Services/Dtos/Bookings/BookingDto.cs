using Trip.Domain;

namespace Trip.Services.Dtos.Bookings
{
    public class BookingDto
    {
        public int BookingId { get; set; }
        public int TripId { get; set; }
        public int PassengerId { get; set; }
        public string PassengerName { get; set; }
        public BookingStatus Status { get; set; }
    }
}
