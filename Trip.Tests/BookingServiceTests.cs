using Microsoft.EntityFrameworkCore;
using Trip.Database;
using Trip.Database.Repos;
using Trip.Domain;
using Trip.Services;
using Trip.Services.Dtos.Bookings;

namespace Trip.Tests
{
    [TestClass]
    public class BookingServiceTests
    {
        [TestMethod]
        public void CreateBookingTest()
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
            var bookingRepo = new BookingRepo(appDbContext);
            var tripRepo = new TripRepo(appDbContext);
            var notificationRepo = new NotificationRepo(appDbContext);
            var notificationService = new NotificationService(notificationRepo, accountRepo);
            var bookingService = new BookingService(
                bookingRepo,
                accountRepo,
                tripRepo,
                notificationService
            );

            var createBookingDto = new CreateBookingDto 
            {
                TripId = 1,
                PassengerId = 2,
            };

            var result = bookingService.CreateBooking(createBookingDto);

            Assert.IsTrue(result.Succeed);
            
            var booking = appDbContext.Bookings.First(booking => booking.BookingId == result.Data);
            Assert.IsNotNull(booking);
            Assert.AreEqual(1, booking.TripId);
            Assert.AreEqual(2, booking.PassengerId);
        }

        [TestMethod]
        public void CancelBookingTest_1()
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
                appDbContext.Bookings.AddRange(
                    new Booking 
                    {
                        TripId = 1,
                        PassengerId = 2,
                        Status = BookingStatus.Waiting,
                    }
                );
                appDbContext.SaveChanges();
            }
            var accountRepo = new AccountRepo(appDbContext);
            var bookingRepo = new BookingRepo(appDbContext);
            var tripRepo = new TripRepo(appDbContext);
            var notificationRepo = new NotificationRepo(appDbContext);
            var notificationService = new NotificationService(notificationRepo, accountRepo);
            var bookingService = new BookingService(
                bookingRepo,
                accountRepo,
                tripRepo,
                notificationService
            );

            var cancelBookingDto = new CancelBookingDto 
            {
                AppUserId = 1,
                BookingId = 1,
            };

            var result = bookingService.CancelBooking(cancelBookingDto);

            Assert.IsTrue(result.Succeed);
            var booking = appDbContext.Bookings.First(booking => booking.BookingId == 1);
            Assert.IsNotNull(booking);
            Assert.AreEqual(BookingStatus.CanceledByDriver, booking.Status);
        }

        [TestMethod]
        public void CancelBookingTest_2()
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
                appDbContext.Bookings.AddRange(
                    new Booking
                    {
                        TripId = 1,
                        PassengerId = 2,
                        Status = BookingStatus.Waiting,
                    }
                );
                appDbContext.SaveChanges();
            }
            var accountRepo = new AccountRepo(appDbContext);
            var bookingRepo = new BookingRepo(appDbContext);
            var tripRepo = new TripRepo(appDbContext);
            var notificationRepo = new NotificationRepo(appDbContext);
            var notificationService = new NotificationService(notificationRepo, accountRepo);
            var bookingService = new BookingService(
                bookingRepo,
                accountRepo,
                tripRepo,
                notificationService
            );

            var cancelBookingDto = new CancelBookingDto
            {
                AppUserId = 2,
                BookingId = 1,
            };

            var result = bookingService.CancelBooking(cancelBookingDto);

            Assert.IsTrue(result.Succeed);
            var booking = appDbContext.Bookings.First(booking => booking.BookingId == 1);
            Assert.IsNotNull(booking);
            Assert.AreEqual(BookingStatus.CanceledByPassenger, booking.Status);
        }

        [TestMethod]
        public void ConfirmBookingTest()
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
                appDbContext.Bookings.AddRange(
                    new Booking
                    {
                        TripId = 1,
                        PassengerId = 2,
                        Status = BookingStatus.Waiting,
                    }
                );
                appDbContext.SaveChanges();
            }
            var accountRepo = new AccountRepo(appDbContext);
            var bookingRepo = new BookingRepo(appDbContext);
            var tripRepo = new TripRepo(appDbContext);
            var notificationRepo = new NotificationRepo(appDbContext);
            var notificationService = new NotificationService(notificationRepo, accountRepo);
            var bookingService = new BookingService(
                bookingRepo,
                accountRepo,
                tripRepo,
                notificationService
            );

            var confirmBookingDto = new ConfirmBookingDto
            {
                AppUserId = 1,
                BookingId = 1,
            };

            var result = bookingService.ConfirmBooking(confirmBookingDto);

            Assert.IsTrue(result.Succeed);
            var booking = appDbContext.Bookings.First(booking => booking.BookingId == 1);
            Assert.IsNotNull(booking);
            Assert.AreEqual(BookingStatus.Confirmed, booking.Status);
        }

        [TestMethod]
        public void GetByTripIdTest()
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
                appDbContext.Bookings.AddRange(
                    new Booking
                    {
                        TripId = 1,
                        PassengerId = 2,
                        Status = BookingStatus.Waiting,
                    }
                );
                appDbContext.SaveChanges();
            }
            var accountRepo = new AccountRepo(appDbContext);
            var bookingRepo = new BookingRepo(appDbContext);
            var tripRepo = new TripRepo(appDbContext);
            var notificationRepo = new NotificationRepo(appDbContext);
            var notificationService = new NotificationService(notificationRepo, accountRepo);
            var bookingService = new BookingService(
                bookingRepo,
                accountRepo,
                tripRepo,
                notificationService
            );

            var result = bookingService.GetByTripId(1);

            Assert.IsTrue(result.Succeed);
            CollectionAssert.AreEqual(new[] { 1 }, result.Data.Select(booking => booking.BookingId).ToList());
        }
    }
}
