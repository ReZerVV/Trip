using Trip.Domain;
using Trip.Repos;
using Trip.Service;
using Trip.Services.Dtos.Notifications;
using Trip.Services.Dtos.Reviews;

namespace Trip.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepo _reviewRepo;
        private readonly IAccountRepo _accountRepo;
        private readonly ITripRepo _tripRepo;
        private readonly INotificationService _notificationService;

        public ReviewService(
            IReviewRepo reviewRepo,
            IAccountRepo accountRepo,
            ITripRepo tripRepo,
            INotificationService notificationService)
        {
            _reviewRepo = reviewRepo;
            _accountRepo = accountRepo;
            _tripRepo = tripRepo;
            _notificationService = notificationService;
        }

        public Result<int> CreateReview(CreateReviewDto createReviewDto)
        {
            var trip = _tripRepo.GetTripById(createReviewDto.TripId);
            if (trip == null || trip.DriverId == createReviewDto.ReviewerId)
            {
                return Result<int>.Fail("Invalid trip id");
            }
            if (_accountRepo.GetAppUserById(createReviewDto.ReviewerId) == null)
            {
                return Result<int>.Fail("Invalid reviewer id");
            }
            if (string.IsNullOrEmpty(createReviewDto.Description))
            {
                return Result<int>.Fail("Invalid description value");
            }
            if (createReviewDto.Grade <= 0 && createReviewDto.Grade > 5)
            {
                return Result<int>.Fail("Invalid grade value");
            }
            var review = new Review
            {
                TripId = createReviewDto.TripId,
                ReviewerId = createReviewDto.ReviewerId,
                DateOfCreation = DateTime.Now,
                IsDeleted = false,
                Grade = createReviewDto.Grade,
                Description = createReviewDto.Description,
            };
            _reviewRepo.CreateReview(review);
            _reviewRepo.SaveChanges();
            var notification = new SendNotificationDto 
            {
                ReciverId = review.Trip.DriverId,
                Title = "New review",
                Description = "A new review has been left for your trip",
            };
            _notificationService.SendNotification(notification);
            return Result<int>.Success(review.ReviewerId);
        }

        public Result DeleteReview(int reviewId)
        {
            var review = _reviewRepo.GetReviewById(reviewId);
            if (review == null)
            {
                return Result.Fail("There is no review with this id");
            }
            review.IsDeleted = true;
            _reviewRepo.UpdateReview(review);
            _reviewRepo.SaveChanges();
            return Result.Success();
        }

        public Result EditReview(EditReviewDto editReviewDto)
        {
            var review = _reviewRepo.GetReviewById(editReviewDto.ReviewId);
            if (review == null)
            {
                return Result.Fail("There is no review with this id");
            }
            if (string.IsNullOrEmpty(editReviewDto.Description))
            {
                return Result.Fail("Invalid description value");
            }
            if (editReviewDto.Grade <= 0 && editReviewDto.Grade > 10)
            {
                return Result.Fail("Invalid grade value");
            }
            review.Grade = editReviewDto.Grade;
            review.Description = editReviewDto.Description;
            _reviewRepo.UpdateReview(review);
            _reviewRepo.SaveChanges();
            return Result.Success();
        }

        public Result<ReviewDto> GetReviewById(int reviewId)
        {
            var review = _reviewRepo.GetReviewById(reviewId);
            if (review == null)
            {
                return Result<ReviewDto>.Fail("There is no review with this id");
            }
            return Result<ReviewDto>.Success(new ReviewDto
            {
                ReviewId = review.ReviewId,
                TripId = review.TripId,
                ReviewerId = review.ReviewerId,
                ReviewerName = review.Reviewer.UserName,
                DateOfCreation = review.DateOfCreation,
                Grade = review.Grade,
                Description = review.Description,
            });
        }

        public Result<IEnumerable<ReviewDto>> GetReviewsByAppUserId(int appUserId)
        {
            return Result<IEnumerable<ReviewDto>>.Success(_reviewRepo.GetReviewsByAppUserId(appUserId)
                .Select(review => new ReviewDto
                {
                    ReviewId = review.ReviewId,
                    TripId = review.TripId,
                    ReviewerId = review.ReviewerId,
                    ReviewerName = review.Reviewer.UserName,
                    DateOfCreation = review.DateOfCreation,
                    Grade = review.Grade,
                    Description = review.Description,
                })
                .ToList()
            );
        }

        public Result<IEnumerable<ReviewDto>> GetReviewsByTripId(int tripId)
        {
            return Result<IEnumerable<ReviewDto>>.Success(_reviewRepo.GetReviewsByTripId(tripId)
                .Select(review => new ReviewDto
                {
                    ReviewId = review.ReviewId,
                    TripId = review.TripId,
                    ReviewerId = review.ReviewerId,
                    ReviewerName = review.Reviewer.UserName,
                    DateOfCreation = review.DateOfCreation,
                    Grade = review.Grade,
                    Description = review.Description,
                })
                .ToList()
            );
        }
    }
}
