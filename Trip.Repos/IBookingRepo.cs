using Trip.Domain;

namespace Trip.Repos
{
    public interface IBookingRepo : IRepo
    {
        void CreateBooking(Booking booking);
        void UpdateBooking(Booking booking);
        IEnumerable<Booking> GetAllBookings();
        Booking GetBookingById(int bookingId);
        IEnumerable<Booking> GetBookingsByTripId(int tripId);
        IEnumerable<Booking> GetBookingsByPassengerId(int passengerId);
        void DeleteBooking(int bookingId);
    }
}
