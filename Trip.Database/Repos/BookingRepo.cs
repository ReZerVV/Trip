using Microsoft.EntityFrameworkCore;
using Trip.Domain;
using Trip.Repos;

namespace Trip.Database.Repos
{
    public class BookingRepo : IBookingRepo
    {
        private readonly AppDbContext appDbContext;

        public BookingRepo(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public void CreateBooking(Booking booking)
        {
            appDbContext.Bookings.Add(booking);
        }

        public void DeleteBooking(int bookingId)
        {
            var booking = appDbContext.Bookings.FirstOrDefault(booking => booking.BookingId == bookingId);
            if (booking == null)
            {
                throw new ArgumentException();
            }
            appDbContext.Remove(booking);
        }

        public IEnumerable<Booking> GetAllBookings()
        {
            return appDbContext.Bookings.ToList();
        }

        public Booking? GetBookingById(int bookingId)
        {
            return appDbContext.Bookings
                .Include(booking => booking.Passenger)
                .FirstOrDefault(booking => booking.BookingId == bookingId);
        }

        public IEnumerable<Booking> GetBookingsByPassengerId(int passengerId)
        {
            return appDbContext.Bookings
                .Where(booking => booking.PassengerId == passengerId)
                .ToList();
        }

        public IEnumerable<Booking> GetBookingsByTripId(int tripId)
        {
            return appDbContext.Bookings
                .Where(booking => booking.TripId == tripId)
                .ToList();
        }

        public void SaveChanges()
        {
            appDbContext.SaveChanges();
        }

        public void UpdateBooking(Booking bookingEntity)
        {
            var booking = appDbContext.Bookings.FirstOrDefault(booking => booking.BookingId == bookingEntity.BookingId);
            if (booking == null)
            {
                throw new ArgumentException();
            }
            booking.Status = bookingEntity.Status;
            appDbContext.Bookings.Update(booking);
        }
    }
}
