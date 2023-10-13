namespace Trip.Services.Dtos.Reviews
{
    public class CreateReviewDto
    {
        public int TripId { get; set; }
        public int ReviewerId { get; set; }
        public int Grade { get; set; }
        public string Description { get; set; }
    }
}
