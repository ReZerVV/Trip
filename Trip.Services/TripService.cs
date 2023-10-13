using System.Text.RegularExpressions;
using Trip.Domain;
using Trip.Repos;
using Trip.Service;
using Trip.Services.Dtos.Trips;

namespace Trip.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepo tripRepo;
        private readonly IBookingRepo bookingRepo;
        private readonly IAccountRepo accountRepo;

        public TripService(
            ITripRepo tripRepo,
            IBookingRepo bookingRepo,
            IAccountRepo accountRepo)
        {
            this.tripRepo = tripRepo;
            this.bookingRepo = bookingRepo;
            this.accountRepo = accountRepo;
        }

        public Result CancelTrip(int tripId)
        {
            var trip = tripRepo.GetTripById(tripId);
            if (trip == null)
            {
                return Result.Fail("There is no trip with this id");
            }
            trip.Status = TripStatus.Canceled;
            tripRepo.UpdateTrip(trip);
            foreach (var booking in bookingRepo.GetBookingsByTripId(trip.TripId))
            {
                booking.Status = BookingStatus.CanceledByDriver;
                bookingRepo.UpdateBooking(booking);
            }
            tripRepo.SaveChanges();
            return Result.Success();
        }

        public Result CompleteTrip(int tripId)
        {
            var trip = tripRepo.GetTripById(tripId);
            if (trip == null)
            {
                return Result.Fail("There is no trip with this id");
            }
            trip.Status = TripStatus.Completed;
            tripRepo.UpdateTrip(trip);
            foreach (var booking in bookingRepo.GetBookingsByTripId(trip.TripId))
            {
                if (booking.Status == BookingStatus.Waiting) 
                {
                    booking.Status = BookingStatus.CanceledByDriver;
                }
                if (booking.Status == BookingStatus.Confirmed)
                {
                    booking.Status = BookingStatus.Completed;
                }
                bookingRepo.UpdateBooking(booking);
            }
            tripRepo.SaveChanges();
            return Result.Success();
        }

        public Result<int> CreateTrip(CreateTripDto createTripDto)
        {
            if (accountRepo.GetAppUserById(createTripDto.DriverId) == null)
            {
                return Result<int>.Fail("Invalid driver id");
            }
            if (createTripDto.Date <= DateTime.Now.AddMinutes(30))
            {
                return Result<int>.Fail("Invalid the trip start time");
            }
            if (!Regex.IsMatch(createTripDto.From, @"^[a-zA-Z0-9]+$") ||
                !Regex.IsMatch(createTripDto.To, @"^[a-zA-Z0-9]+$"))
            {
                return Result<int>.Fail("Invalid origin or destination name");
            }
            if (createTripDto.NumberOfSeats == 0)
            {
                return Result<int>.Fail("Unacceptable number of seats");
            }
            if (createTripDto.Price < 0)
            {
                return Result<int>.Fail("Unacceptable travel price");
            }
            var trip = new Domain.Trip
            {
                DriverId = createTripDto.DriverId,
                Status = TripStatus.Waiting,
                Date = createTripDto.Date,
                From = createTripDto.From,
                To = createTripDto.To,
                NumberOfSeats = createTripDto.NumberOfSeats,
                Price = createTripDto.Price,
            };
            tripRepo.CreateTrip(trip);
            tripRepo.SaveChanges();
            return Result<int>.Success(trip.TripId);
        }

        public Result EditTrip(EditTripDto editTripDto)
        {
            var trip = tripRepo.GetTripById(editTripDto.TripId);
            if (trip == null)
            {
                return Result.Fail("There is no trip with this id");
            }
            if (editTripDto.Date <= DateTime.Now.AddMinutes(30))
            {
                return Result.Fail("Invalid the trip start time");
            }
            if (!Regex.IsMatch(editTripDto.From, @"^[a-zA-Z0-9]+$") ||
                !Regex.IsMatch(editTripDto.To, @"^[a-zA-Z0-9]+$"))
            {
                return Result.Fail("Invalid origin or destination name");
            }
            if (editTripDto.NumberOfSeats == 0)
            {
                return Result.Fail("Unacceptable number of seats");
            }
            if (editTripDto.Price < 0)
            {
                return Result.Fail("Unacceptable travel price");
            }
            // TODO: Use automapper
            trip.Date = editTripDto.Date;
            trip.From = editTripDto.From;
            trip.To = editTripDto.To;
            trip.NumberOfSeats = editTripDto.NumberOfSeats;
            trip.Price = editTripDto.Price;
            tripRepo.UpdateTrip(trip);
            tripRepo.SaveChanges();
            return Result.Success();
        }

        public Result<IEnumerable<TripDto>> GetHistoryTripByAppUserId(int appUserId)
        {
            var trips = tripRepo.GetTripsByDrivereId(appUserId).ToList();
            trips.AddRange(tripRepo.GetTripsByPassengerId(appUserId));
            return Result<IEnumerable<TripDto>>.Success(
                trips.Select(trip => new TripDto
                {
                    TripId = trip.TripId,
                    DriverId = trip.DriverId,
                    DriverName = trip.Driver.UserName,
                    NumberOfOccupiedSeats = accountRepo.GetAppUserById(trip.DriverId).Bookings
                        .Where(booking => booking.Status == BookingStatus.Confirmed)
                        .Count(),
                    Status = trip.Status,
                    Date = trip.Date,
                    From = trip.From,
                    To = trip.To,
                    NumberOfSeats = trip.NumberOfSeats,
                    Price = trip.Price,
                })
                .ToList()
           );
        }

        public Result<IEnumerable<TripDto>> GetTripsByAppUserId(int appUserId)
        {
            return Result<IEnumerable<TripDto>>.Success(
                tripRepo.GetTripsByDrivereId(appUserId)
                .Where(trip => trip.Status == TripStatus.Waiting)
                .Select(trip => new TripDto
                {
                    TripId = trip.TripId,
                    DriverId = trip.DriverId,
                    DriverName = trip.Driver.UserName,
                    NumberOfOccupiedSeats = accountRepo.GetAppUserById(trip.DriverId).Bookings
                        .Where(booking => booking.Status == BookingStatus.Confirmed)
                        .Count(),
                    Status = trip.Status,
                    Date = trip.Date,
                    From = trip.From,
                    To = trip.To,
                    NumberOfSeats = trip.NumberOfSeats,
                    Price = trip.Price,
                })
                .ToList()
           );
        }

        public Result<TripDto> GetTirpById(int tripId)
        {
            var trip = tripRepo.GetTripById(tripId);
            if (trip == null)
            {
                return Result<TripDto>.Fail("There is no trip with this id");
            }
            return Result<TripDto>.Success(new TripDto
            {
                TripId = trip.TripId,
                DriverId = trip.DriverId,
                NumberOfOccupiedSeats = accountRepo.GetAppUserById(trip.DriverId).Bookings
                    .Where(booking => booking.Status == BookingStatus.Confirmed)
                    .Count(),
                Status = trip.Status,
                Date = trip.Date,
                From = trip.From,
                To = trip.To,
                NumberOfSeats = trip.NumberOfSeats,
                Price = trip.Price,
            });
        }

        public Result<IEnumerable<TripDto>> SearchTrip(SearchQueryTripDto searchQueryTripDto)
        {
            if (searchQueryTripDto.Date == null)
            {
                return Result<IEnumerable<TripDto>>.Fail("Invalid date value");
            }
            if (searchQueryTripDto.Date < DateTime.Now)
            {
                return Result<IEnumerable<TripDto>>.Fail("Invalid date value");
            }
            if (searchQueryTripDto.MinPrice > searchQueryTripDto.MaxPrice)
            {
                return Result<IEnumerable<TripDto>>.Fail("Invalid max price or min price value");
            }
            return Result<IEnumerable<TripDto>>.Success(tripRepo.GetAllTrips()
                .Where(trip => trip.Status == TripStatus.Waiting)
                .Where(trip => (trip.From == searchQueryTripDto.From && trip.To == searchQueryTripDto.To)
                            && ((searchQueryTripDto.MinPrice == null || searchQueryTripDto.MaxPrice == null) || (trip.Price >= searchQueryTripDto.MinPrice && trip.Price < searchQueryTripDto.MaxPrice))
                            && (trip.Date >= searchQueryTripDto.Date)
                )
                .Select(trip => new TripDto
                {
                    TripId = trip.TripId,
                    DriverId = trip.DriverId,
                    DriverName = trip.Driver.UserName,
                    NumberOfOccupiedSeats = accountRepo.GetAppUserById(trip.DriverId).Bookings
                        .Where(booking => booking.Status == BookingStatus.Confirmed)
                        .Count(),
                    Status = trip.Status,
                    Date = trip.Date,
                    From = trip.From,
                    To = trip.To,
                    NumberOfSeats = trip.NumberOfSeats,
                    Price = trip.Price,
                })
                .ToList()
            );
        }
    }
}
