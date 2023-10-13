using Trip.Domain;

namespace Trip.Services.Dtos.Trips
{
    public class TripDto
    {
        public int TripId { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public TripStatus Status { get; set; }
        public DateTime Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int NumberOfSeats { get; set; }
        public int NumberOfOccupiedSeats { get; set; }
        public decimal Price { get; set; }
    }
}
