namespace Trip.Repos
{
    public interface ITripRepo : IRepo
    {
        void CreateTrip(Domain.Trip trip);
        void UpdateTrip(Domain.Trip trip);
        Domain.Trip GetTripById(int tripId);
        IEnumerable<Domain.Trip> GetAllTrips();
        IEnumerable<Domain.Trip> GetTripsByDrivereId(int driverId);
        IEnumerable<Domain.Trip> GetTripsByPassengerId(int passengerId);
        void DeleteTrip(int tripId);
    }
}
