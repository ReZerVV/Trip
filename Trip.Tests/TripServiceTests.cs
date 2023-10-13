using Microsoft.EntityFrameworkCore;
using Trip.Database;
using Trip.Database.Repos;
using Trip.Domain;
using Trip.Services;
using Trip.Services.Dtos.Trips;

namespace Trip.Tests
{
    [TestClass]
    public class TripServiceTests
    {
        [TestMethod]
        public void CreateTripTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Trip.InMemory.Database")
                .Options;
            var appDbContext = new AppDbContext(options);
            {
                appDbContext.Users.Add(new Domain.AppUser
                {
                    UserName = "cyril",
                    Email = "cyril@gmail.com",
                    Password = "00000000",
                });
                appDbContext.SaveChanges();
            }
            var accountRepo = new AccountRepo(appDbContext);
            var bookingRepo = new BookingRepo(appDbContext);
            var tripRepo = new TripRepo(appDbContext);
            var tripService = new TripService(
                tripRepo,
                bookingRepo,
                accountRepo
            );

            var createTripDto = new CreateTripDto
            {
                DriverId = 1,
                Date = DateTime.Now.AddHours(1),
                From = "Zaporozhye",
                To = "Kyiv",
                NumberOfSeats = 1,
                Price = 10,
            };

            var result = tripService.CreateTrip(createTripDto);

            Assert.IsTrue(result.Succeed);
            var trip = appDbContext.Trips.FirstOrDefault(trip => trip.TripId == result.Data);
            Assert.IsNotNull(trip);
            Assert.AreEqual(1, trip.DriverId);
            Assert.AreEqual("Zaporozhye", trip.From);
            Assert.AreEqual("Kyiv", trip.To);
            Assert.AreEqual(1, trip.NumberOfSeats);
            Assert.AreEqual(10, trip.Price);
        }

        [TestMethod]
        public void GetTirpByIdTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Trip.InMemory.Database")
                .Options;
            var appDbContext = new AppDbContext(options);
            {
                appDbContext.Users.Add(new Domain.AppUser
                {
                    UserName = "cyril",
                    Email = "cyril@gmail.com",
                    Password = "00000000",
                });
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
            var tripService = new TripService(
                tripRepo,
                bookingRepo,
                accountRepo
            );

            var result = tripService.GetTirpById(1);

            Assert.IsTrue(result.Succeed);
            Assert.AreEqual(1, result.Data.DriverId);
            Assert.AreEqual("Zaporozhye", result.Data.From);
            Assert.AreEqual("Kyiv", result.Data.To);
            Assert.AreEqual(1, result.Data.NumberOfSeats);
            Assert.AreEqual(10, result.Data.Price);
        }

        [TestMethod]
        public void EditTripTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Trip.InMemory.Database")
                .Options;
            var appDbContext = new AppDbContext(options);
            {
                appDbContext.Users.Add(new AppUser
                {
                    UserName = "cyril",
                    Email = "cyril@gmail.com",
                    Password = "00000000",
                });
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
            var tripService = new TripService(
                tripRepo,
                bookingRepo,
                accountRepo
            );

            var trip = appDbContext.Trips.First(trip => trip.TripId == 1);
            var editTripDto = new EditTripDto 
            {
                TripId = trip.TripId,
                Date = trip.Date,
                From = trip.From,
                To = trip.To,
                NumberOfSeats = trip.NumberOfSeats,
                Price = 100,
            };

            var result = tripService.EditTrip(editTripDto);
            
            Assert.IsTrue(result.Succeed);
            trip = appDbContext.Trips.First(trip => trip.TripId == 1);
            Assert.AreEqual(100, trip.Price);
        }

        [TestMethod]
        public void CancelTripTest()
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
                        UserName = "kira",
                        Email = "kira@gmail.com",
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
                        PassengerId = 1,
                        Status = BookingStatus.Confirmed,
                    },
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
            var tripService = new TripService(
                tripRepo,
                bookingRepo,
                accountRepo
            );

            var result = tripService.CancelTrip(1);

            Assert.IsTrue(result.Succeed);
            var trip = appDbContext.Trips.First(trip => trip.TripId == 1);
            Assert.AreEqual(TripStatus.Canceled, trip.Status);
            CollectionAssert.AreEqual(
                new[] { BookingStatus.CanceledByDriver, BookingStatus.CanceledByDriver }, 
                appDbContext.Bookings.Where(booking => booking.TripId == trip.TripId)
                    .Select(booking => booking.Status)
                    .ToList()
            );
        }

        [TestMethod]
        public void CompleteTripTest()
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
                        UserName = "kira",
                        Email = "kira@gmail.com",
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
                        PassengerId = 1,
                        Status = BookingStatus.Confirmed,
                    },
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
            var tripService = new TripService(
                tripRepo,
                bookingRepo,
                accountRepo
            );

            var result = tripService.CompleteTrip(1);

            Assert.IsTrue(result.Succeed);
            var trip = appDbContext.Trips.First(trip => trip.TripId == 1);
            Assert.IsNotNull(trip);
            Assert.AreEqual(TripStatus.Completed, trip.Status);
            var booking_1 = appDbContext.Bookings.First(booking => booking.BookingId == 1);
            Assert.IsNotNull(booking_1);
            Assert.AreEqual(BookingStatus.Completed, booking_1.Status);
            var booking_2 = appDbContext.Bookings.First(booking => booking.BookingId == 2);
            Assert.IsNotNull(booking_2);
            Assert.AreEqual(BookingStatus.CanceledByDriver, booking_2.Status);
        }

        [TestMethod]
        public void GetHistoryTripByAppUserIdTest()
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
                        UserName = "kira",
                        Email = "kira@gmail.com",
                        Password = "00000000",
                    },
                    new AppUser
                    {
                        UserName = "kira1",
                        Email = "kira1@gmail.com",
                        Password = "00000000",
                    }
                );
                appDbContext.Trips.AddRange(
                    new Domain.Trip
                    {
                        Status = TripStatus.Waiting,
                        DriverId = 1,
                        Date = DateTime.Now.AddHours(1),
                        From = "Zaporozhye",
                        To = "Kyiv",
                        NumberOfSeats = 1,
                        Price = 10,
                    },
                    new Domain.Trip
                    {
                        Status = TripStatus.Waiting,
                        DriverId = 3,
                        Date = DateTime.Now.AddHours(1),
                        From = "Kyiv",
                        To = "Zaporozhye",
                        NumberOfSeats = 10,
                        Price = 100,
                    }
                );
                appDbContext.Bookings.AddRange(
                    new Booking
                    {
                        TripId = 1,
                        PassengerId = 2,
                        Status = BookingStatus.Confirmed,
                    },
                    new Booking
                    {
                        TripId = 2,
                        PassengerId = 1,
                        Status = BookingStatus.Waiting,
                    }
                );
                appDbContext.SaveChanges();
            }
            var accountRepo = new AccountRepo(appDbContext);
            var bookingRepo = new BookingRepo(appDbContext);
            var tripRepo = new TripRepo(appDbContext);
            var tripService = new TripService(
                tripRepo,
                bookingRepo,
                accountRepo
            );

            var result = tripService.GetHistoryTripByAppUserId(1);
            
            Assert.IsTrue(result.Succeed);
            CollectionAssert.AreEqual(new[] { 1, 2 }, result.Data.Select(trip => trip.TripId).ToList());
        }

        [TestMethod]
        public void SearchTrip()
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
                appDbContext.Trips.AddRange(
                    new Domain.Trip
                    {
                        Status = TripStatus.Waiting,
                        DriverId = 1,
                        Date = DateTime.Now.AddHours(1),
                        From = "Zaporozhye",
                        To = "Kyiv",
                        NumberOfSeats = 1,
                        Price = 10,
                    },
                    new Domain.Trip
                    {
                        Status = TripStatus.Waiting,
                        DriverId = 1,
                        Date = DateTime.Now.AddHours(1),
                        From = "Kyiv",
                        To = "Zaporozhye",
                        NumberOfSeats = 10,
                        Price = 100,
                    }
                );
                appDbContext.SaveChanges();
            }
            var accountRepo = new AccountRepo(appDbContext);
            var bookingRepo = new BookingRepo(appDbContext);
            var tripRepo = new TripRepo(appDbContext);
            var tripService = new TripService(
                tripRepo,
                bookingRepo,
                accountRepo
            );

            var searchQueryTripDto = new SearchQueryTripDto 
            {
                From = "Kyiv",
                To = "Zaporozhye",
            };

            var result = tripService.SearchTrip(searchQueryTripDto);

            Assert.IsTrue(result.Succeed);
            CollectionAssert.AreEqual(new[] { 2 }, result.Data.Select(trip => trip.TripId).ToList());

            searchQueryTripDto = new SearchQueryTripDto
            {
                From = "Zaporozhye",
                To = "Kyiv",
            };

            result = tripService.SearchTrip(searchQueryTripDto);

            Assert.IsTrue(result.Succeed);
            CollectionAssert.AreEqual(new[] { 1 }, result.Data.Select(trip => trip.TripId).ToList());
        }
    }
}
