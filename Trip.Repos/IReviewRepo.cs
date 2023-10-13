using Trip.Domain;

namespace Trip.Repos
{
    public interface IReviewRepo : IRepo
    {
        void CreateReview(Review review);
        void DeleteReview(int reviewId);
        Review GetReviewById(int reviewId);
        IEnumerable<Review> GetReviewsByAppUserId(int appUserId);
        IEnumerable<Review> GetReviewsByTripId(int tripId);
        void UpdateReview(Review review);
    }
}
