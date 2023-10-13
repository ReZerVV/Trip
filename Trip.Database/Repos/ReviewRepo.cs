using Microsoft.EntityFrameworkCore;
using Trip.Domain;
using Trip.Repos;

namespace Trip.Database.Repos
{
    public class ReviewRepo : IReviewRepo
    {
        private readonly AppDbContext appDbContext;

        public ReviewRepo(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public void CreateReview(Review review)
        {
            review.IsDeleted = false;
            appDbContext.Reviews.Add(review);
        }

        public void DeleteReview(int reviewId)
        {
            var review = appDbContext.Reviews.FirstOrDefault(review => review.ReviewId == reviewId);
            if (review == null)
            {
                throw new ArgumentException();
            }
            appDbContext.Reviews.Remove(review);
        }

        public Review? GetReviewById(int reviewId)
        {
            return appDbContext.Reviews
                .Include(review => review.Trip)
                .Include(review => review.Reviewer)
                .FirstOrDefault(review => review.IsDeleted == false && review.ReviewId == reviewId);
        }

        public IEnumerable<Review> GetReviewsByAppUserId(int appUserId)
        {
            return appDbContext.Reviews
                .Where(review => review.IsDeleted == false && review.ReviewerId == appUserId)
                .ToList();
        }

        public IEnumerable<Review> GetReviewsByTripId(int tripId)
        {
            return appDbContext.Reviews
                .Where(review => review.IsDeleted == false && review.TripId == tripId)
                .ToList();
        }

        public void SaveChanges()
        {
            appDbContext.SaveChanges();
        }

        public void UpdateReview(Review reviewEntity)
        {
            var review = appDbContext.Reviews.FirstOrDefault(review => review.ReviewId == reviewEntity.ReviewId);
            if (review == null)
            {
                throw new ArgumentException();
            }
            review.IsDeleted = reviewEntity.IsDeleted;
            review.Grade = reviewEntity.Grade;
            review.Description = reviewEntity.Description;
            appDbContext.Reviews.Update(review);
        }
    }
}
