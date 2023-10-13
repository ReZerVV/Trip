namespace Trip.Services.Dtos.Trips
{
    public class CreateTripDto
    {
        public int DriverId { get; set; }
        public DateTime Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int NumberOfSeats { get; set; }
        public decimal Price { get; set; }
    }
}
