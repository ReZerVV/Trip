using Microsoft.EntityFrameworkCore;
using Trip.Domain;
using Trip.Repos;

namespace Trip.Database.Repos
{
    public class TripRepo : ITripRepo
    {
        private readonly AppDbContext appDbContext;

        public TripRepo(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public void CreateTrip(Domain.Trip trip)
        {
            appDbContext.Trips.Add(trip);
        }

        public void DeleteTrip(int tripId)
        {
            var trip = appDbContext.Trips.FirstOrDefault(trip => trip.TripId == tripId);
            if (trip == null)
            {
                throw new ArgumentException();
            }
            appDbContext.Trips.Remove(trip);
        }

        public IEnumerable<Domain.Trip> GetAllTrips()
        {
            return appDbContext.Trips.Include(trip => trip.Driver).ToList();
        }

        public Domain.Trip? GetTripById(int tripId)
        {
            return appDbContext.Trips
                .Include(trip => trip.Driver)
                .Include(trip => trip.Bookings)
                .Include(trip => trip.Reviews)
                .FirstOrDefault(trip => trip.TripId == tripId);
        }

        public IEnumerable<Domain.Trip> GetTripsByDrivereId(int driverId)
        {
            return appDbContext.Trips
                .Include(trip => trip.Driver)
                .Where(trip => trip.DriverId == driverId)
                .ToList();
        }

        public IEnumerable<Domain.Trip> GetTripsByPassengerId(int passengerId)
        {
            return appDbContext.Trips
                .Include(trip => trip.Driver)
                .Include(trip => trip.Bookings)
                .Where(trip => trip.Bookings
                    .Select(booking => booking.PassengerId)
                    .Contains(passengerId)
                )
                .ToList();
        }

        public void SaveChanges()
        {
            appDbContext.SaveChanges();
        }

        public void UpdateTrip(Domain.Trip tripEntity)
        {
            var trip = appDbContext.Trips.FirstOrDefault(trip => trip.TripId == tripEntity.TripId);
            if (trip == null)
            {
                throw new ArgumentException();
            }
            trip.Status = tripEntity.Status;
            trip.Date = tripEntity.Date;
            trip.From = tripEntity.From;
            trip.To = tripEntity.To;
            trip.NumberOfSeats = tripEntity.NumberOfSeats;
            trip.Price = tripEntity.Price;
            appDbContext.Trips.Update(trip);
        }
    }
}
