using Microsoft.EntityFrameworkCore;
using Trip.Database;
using Trip.Database.Repos;
using Trip.Domain;
using Trip.Services;
using Trip.Services.Dtos.Reviews;

namespace Trip.Tests
{
    [TestClass]
    public class ReviewServiceTest
    {
        [TestMethod]
        public void CreateReviewTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Trip.InMemory.Database")
                .Options;
            var appDbContext = new AppDbContext(options);
            {
                appDbContext.Users.AddRange(
                    new AppUser
                    {
                        UserName = "cyril",
                        Email = "cyril@gmail.com",
                        Password = "00000000",
                    },
                    new AppUser
                    {
                        UserName = "cyril1",
                        Email = "cyril1@gmail.com",
                        Password = "00000000",
                    }
                );
                appDbContext.Trips.Add(new Domain.Trip
                {
                    Status = TripStatus.Waiting,
                    DriverId = 1,
                    Date = DateTime.Now.AddHours(1),
                    From = "Zaporozhye",
                    To = "Kyiv",
                    NumberOfSeats = 1,
                    Price = 10,
                });
                appDbContext.SaveChanges();
            }
            var accountRepo = new AccountRepo(appDbContext);
            var reviewRepo = new ReviewRepo(appDbContext);
            var tripRepo = new TripRepo(appDbContext);
            var notificationRepo = new NotificationRepo(appDbContext);
            var notificationService = new NotificationService(notificationRepo, accountRepo);
            var reviewService = new ReviewService(
                reviewRepo,
                accountRepo,
                tripRepo,
                notificationService
            );

            var createReviewDto = new CreateReviewDto 
            {
                TripId = 1,
                ReviewerId = 2,
                Grade = 5,
                Description = "...",
            };

            var result = reviewService.CreateReview(createReviewDto);

            Assert.IsTrue(result.Succeed);
            var review = appDbContext.Reviews.FirstOrDefault(review => review.ReviewId == 1 && review.IsDeleted == false);
            Assert.IsNotNull(review);
            Assert.AreEqual(5, review.Grade);
            Assert.AreEqual("...", review.Description);
        }

        [TestMethod]
        public void EditReviewTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Trip.InMemory.Database")
                .Options;
            var appDbContext = new AppDbContext(options);
            {
                appDbContext.Users.AddRange(
                    new AppUser
                    {
                        UserName = "cyril",
                        Email = "cyril@gmail.com",
                        Password = "00000000",
                    }
                );
                appDbContext.Trips.Add(new Domain.Trip
                {
                    Status = TripStatus.Waiting,
                    DriverId = 1,
                    Date = DateTime.Now.AddHours(1),
                    From = "Zaporozhye",
                    To = "Kyiv",
                    NumberOfSeats = 1,
                    Price = 10,
                });
                appDbContext.Reviews.AddRange(
                   new Review 
                   {
                       TripId = 1,
                       ReviewerId = 1,
                       DateOfCreation = DateTime.Now,
                       IsDeleted = false,
                       Grade = 5,
                       Description = "...",
                   }
                );
                appDbContext.SaveChanges();
            }
            var accountRepo = new AccountRepo(appDbContext);
            var reviewRepo = new ReviewRepo(appDbContext);
            var tripRepo = new TripRepo(appDbContext);
            var notificationRepo = new NotificationRepo(appDbContext);
            var notificationService = new NotificationService(notificationRepo, accountRepo);
            var reviewService = new ReviewService(
                reviewRepo,
                accountRepo,
                tripRepo,
                notificationService
            );

            var editReviewDto = new EditReviewDto
            {
                ReviewId = 1,
                Grade = 3,
                Description = "...",
            };

            var result = reviewService.EditReview(editReviewDto);

            Assert.IsTrue(result.Succeed);
            var review = appDbContext.Reviews.First(review => review.ReviewId == 1);
            Assert.IsNotNull(review);
            Assert.AreEqual(3, review.Grade);
        }

        [TestMethod]
        public void DeleteReviewTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Trip.InMemory.Database")
                .Options;
            var appDbContext = new AppDbContext(options);
            {
                appDbContext.Users.AddRange(
                    new AppUser
                    {
                        UserName = "cyril",
                        Email = "cyril@gmail.com",
                        Password = "00000000",
                    },
                    new AppUser
                    {
                         UserName = "cyril1",
                         Email = "cyril1@gmail.com",
                         Password = "00000000",
                    }
                );
                appDbContext.Trips.Add(new Domain.Trip
                {
                    Status = TripStatus.Waiting,
                    DriverId = 1,
                    Date = DateTime.Now.AddHours(1),
                    From = "Zaporozhye",
                    To = "Kyiv",
                    NumberOfSeats = 1,
                    Price = 10,
                });
                appDbContext.Reviews.AddRange(
                   new Review
                   {
                       TripId = 1,
                       ReviewerId = 2,
                       DateOfCreation = DateTime.Now,
                       IsDeleted = false,
                       Grade = 5,
                       Description = "...",
                   }
                );
                appDbContext.SaveChanges();
            }
            var accountRepo = new AccountRepo(appDbContext);
            var reviewRepo = new ReviewRepo(appDbContext);
            var tripRepo = new TripRepo(appDbContext);
            var notificationRepo = new NotificationRepo(appDbContext);
            var notificationService = new NotificationService(notificationRepo, accountRepo);
            var reviewService = new ReviewService(
                reviewRepo,
                accountRepo,
                tripRepo,
                notificationService
            );

            var result = reviewService.DeleteReview(1);

            Assert.IsTrue(result.Succeed);
            Assert.IsNull(appDbContext.Reviews.FirstOrDefault(review => review.ReviewId == 1 && review.IsDeleted == false));
        }

        [TestMethod]
        public void GetReviewsByTripIdTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Trip.InMemory.Database")
                .Options;
            var appDbContext = new AppDbContext(options);
            {
                appDbContext.Users.AddRange(
                    new AppUser
                    {
                        UserName = "cyril",
                        Email = "cyril@gmail.com",
                        Password = "00000000",
                    },
                    new AppUser
                    {
                        UserName = "cyril1",
                        Email = "cyril1@gmail.com",
                        Password = "00000000",
                    }
                );
                appDbContext.Trips.Add(new Domain.Trip
                {
                    Status = TripStatus.Waiting,
                    DriverId = 1,
                    Date = DateTime.Now.AddHours(1),
                    From = "Zaporozhye",
                    To = "Kyiv",
                    NumberOfSeats = 1,
                    Price = 10,
                });
                appDbContext.Reviews.AddRange(
                   new Review
                   {
                       TripId = 1,
                       ReviewerId = 2,
                       DateOfCreation = DateTime.Now,
                       IsDeleted = false,
                       Grade = 5,
                       Description = "...",
                   }
                );
                appDbContext.SaveChanges();
            }
            var accountRepo = new AccountRepo(appDbContext);
            var reviewRepo = new ReviewRepo(appDbContext);
            var tripRepo = new TripRepo(appDbContext);
            var notificationRepo = new NotificationRepo(appDbContext);
            var notificationService = new NotificationService(notificationRepo, accountRepo);
            var reviewService = new ReviewService(
                reviewRepo,
                accountRepo,
                tripRepo,
                notificationService
            );

            var result = reviewService.GetReviewsByTripId(1);

            Assert.IsTrue(result.Succeed);
            CollectionAssert.AreEqual(new[] { 1 }, result.Data.Select(review => review.ReviewId).ToList());
        }

        [TestMethod]
        public void GetReviewsByAppUserIdTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Trip.InMemory.Database")
                .Options;
            var appDbContext = new AppDbContext(options);
            {
                appDbContext.Users.AddRange(
                    new AppUser
                    {
                        UserName = "cyril",
                        Email = "cyril@gmail.com",
                        Password = "00000000",
                    },
                    new AppUser
                    {
                        UserName = "cyril1",
                        Email = "cyril1@gmail.com",
                        Password = "00000000",
                    }
                );
                appDbContext.Trips.Add(new Domain.Trip
                {
                    Status = TripStatus.Waiting,
                    DriverId = 1,
                    Date = DateTime.Now.AddHours(1),
                    From = "Zaporozhye",
                    To = "Kyiv",
                    NumberOfSeats = 1,
                    Price = 10,
                });
                appDbContext.Reviews.AddRange(
                   new Review
                   {
                       TripId = 1,
                       ReviewerId = 2,
                       DateOfCreation = DateTime.Now,
                       IsDeleted = false,
                       Grade = 5,
                       Description = "...",
                   }
                );
                appDbContext.SaveChanges();
            }
            var accountRepo = new AccountRepo(appDbContext);
            var reviewRepo = new ReviewRepo(appDbContext);
            var tripRepo = new TripRepo(appDbContext);
            var notificationRepo = new NotificationRepo(appDbContext);
            var notificationService = new NotificationService(notificationRepo, accountRepo);
            var reviewService = new ReviewService(
                reviewRepo,
                accountRepo,
                tripRepo,
                notificationService
            );

            var result = reviewService.GetReviewsByAppUserId(2);

            Assert.IsTrue(result.Succeed);
            CollectionAssert.AreEqual(new[] { 1 }, result.Data.Select(review => review.ReviewId).ToList());
        }

        [TestMethod]
        public void GetReviewByIdTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Trip.InMemory.Database")
                .Options;
            var appDbContext = new AppDbContext(options);
            {
                appDbContext.Users.AddRange(
                    new AppUser
                    {
                        UserName = "cyril",
                        Email = "cyril@gmail.com",
                        Password = "00000000",
                    },
                    new AppUser
                    {
                        UserName = "cyril1",
                        Email = "cyril1@gmail.com",
                        Password = "00000000",
                    }
                );
                appDbContext.Trips.Add(new Domain.Trip
                {
                    Status = TripStatus.Waiting,
                    DriverId = 1,
                    Date = DateTime.Now.AddHours(1),
                    From = "Zaporozhye",
                    To = "Kyiv",
                    NumberOfSeats = 1,
                    Price = 10,
                });
                appDbContext.Reviews.AddRange(
                   new Review
                   {
                       TripId = 1,
                       ReviewerId = 2,
                       DateOfCreation = DateTime.Now,
                       IsDeleted = false,
                       Grade = 5,
                       Description = "...",
                   }
                );
                appDbContext.SaveChanges();
            }
            var accountRepo = new AccountRepo(appDbContext);
            var reviewRepo = new ReviewRepo(appDbContext);
            var tripRepo = new TripRepo(appDbContext);
            var notificationRepo = new NotificationRepo(appDbContext);
            var notificationService = new NotificationService(notificationRepo, accountRepo);
            var reviewService = new ReviewService(
                reviewRepo,
                accountRepo,
                tripRepo,
                notificationService
            );

            var result = reviewService.GetReviewById(1);

            Assert.IsTrue(result.Succeed);
            Assert.AreEqual(1, result.Data.ReviewId);
        }
    }
}
