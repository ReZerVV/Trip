namespace Trip.Services.Dtos.Trips
{
    public class EditTripDto
    {
        public int TripId { get; set; }
        public DateTime Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int NumberOfSeats { get; set; }
        public decimal Price { get; set; }
    }
}
