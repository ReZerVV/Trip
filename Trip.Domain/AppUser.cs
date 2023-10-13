namespace Trip.Domain
{
    public class AppUser
    {
        public int AppUserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IEnumerable<Review> Reviews { get; set; } = new List<Review>();
        public IEnumerable<Trip> Trips { get; set; } = new List<Trip>();
        public IEnumerable<Booking> Bookings { get; set; } = new List<Booking>();
        public IEnumerable<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
