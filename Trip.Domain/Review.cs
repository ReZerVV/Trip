namespace Trip.Domain
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int TripId { get; set; }
        public Trip Trip { get; set; }
        public int ReviewerId { get; set; }
        public AppUser Reviewer { get; set; }
        public DateTime DateOfCreation { get; set; }
        public bool IsDeleted { get; set; }
        public int Grade { get; set; }
        public string Description { get; set; }
    }
}
