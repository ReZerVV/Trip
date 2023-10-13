using Trip.Service;
using Trip.Services.Dtos.Reviews;

namespace Trip.Services
{
    public interface IReviewService
    {
        Result<int> CreateReview(CreateReviewDto createReviewDto);
        Result EditReview(EditReviewDto editReviewDto);
        Result<IEnumerable<ReviewDto>> GetReviewsByTripId(int tripId);
        Result<IEnumerable<ReviewDto>> GetReviewsByAppUserId(int appUserId);
        Result<ReviewDto> GetReviewById(int reviewId);
        Result DeleteReview(int reviewId);
    }
}
