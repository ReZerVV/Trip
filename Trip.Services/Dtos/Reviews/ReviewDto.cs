namespace Trip.Services.Dtos.Reviews
{
    public class ReviewDto
    {
        public int ReviewId { get; set; }
        public int TripId { get; set; }
        public int ReviewerId { get; set; }
        public string ReviewerName { get; set; }
        public DateTime DateOfCreation { get; set; }
        public int Grade { get; set; }
        public string Description { get; set; }
    }
}
