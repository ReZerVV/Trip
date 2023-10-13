namespace Trip.Domain
{
    public class Trip
    {
        public int TripId { get; set; }
        public int DriverId { get; set; }
        public AppUser Driver { get; set; }
        public TripStatus Status { get; set; }
        public DateTime Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int NumberOfSeats { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<Booking> Bookings { get; set; } = new List<Booking>();
        public IEnumerable<Review> Reviews { get; set; } = new List<Review>();
    }
}
