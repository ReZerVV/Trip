using Trip.Domain;
using Trip.Repos;
using Trip.Service;
using Trip.Services.Dtos.Bookings;
using Trip.Services.Dtos.Notifications;

namespace Trip.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepo _bookingRepo;
        private readonly ITripRepo _tripRepo;
        private readonly INotificationService _notificationService;
        private readonly IAccountRepo _accountRepo;

        public BookingService(
            IBookingRepo bookingRepo,
            IAccountRepo accountRepo,
            ITripRepo tripRepo,
            INotificationService notificationService)
        {
            _bookingRepo = bookingRepo;
            _tripRepo = tripRepo;
            _notificationService = notificationService;
            _accountRepo = accountRepo;
        }

        public Result CancelBooking(CancelBookingDto cancelBookingDto)
        {
            var booking = _bookingRepo.GetBookingById(cancelBookingDto.BookingId);
            if (booking == null)
            {
                return Result.Fail("No such booking exists");
            }
            if (booking.Status == BookingStatus.CanceledByDriver 
            || booking.Status == BookingStatus.CanceledByPassenger)
            {
                return Result.Fail("Booking has already been canceled");
            }
            BookingStatus bookingStatus;
            if (_tripRepo.GetTripById(booking.TripId).DriverId == cancelBookingDto.AppUserId)
            {
                bookingStatus = BookingStatus.CanceledByDriver;
                var notification = new SendNotificationDto
                {
                    ReciverId = booking.PassengerId,
                    Title = "Cancel request",
                    Description = $"Your request for a trip {booking.Trip.From} to {booking.Trip.To} was canceled by the driver",
                };
                _notificationService.SendNotification(notification);
            }
            else if (_bookingRepo.GetBookingsByTripId(booking.TripId)
                .Select(booking => booking.PassengerId).Contains(cancelBookingDto.AppUserId))
            {
                bookingStatus = BookingStatus.CanceledByPassenger;
            }
            else
            {
                return Result.Fail("The user cannot cancel the booking");
            }
            booking.Status = bookingStatus;
            _bookingRepo.UpdateBooking(booking);
            _bookingRepo.SaveChanges();
            return Result.Success();
        }

        public Result ConfirmBooking(ConfirmBookingDto confirmBookingDto)
        {
            var booking = _bookingRepo.GetBookingById(confirmBookingDto.BookingId);
            if (booking == null)
            {
                return Result.Fail("No such booking exists");
            }
            var trip = _tripRepo.GetTripById(booking.TripId);
            if (trip.DriverId != confirmBookingDto.AppUserId)
            {
                return Result.Fail("The user cannot confirm the booking");
            }
            if (trip.NumberOfSeats == _accountRepo.GetAppUserById(trip.DriverId).Bookings
                                                        .Where(booking => booking.Status == BookingStatus.Confirmed)
                                                        .Count())
            {
                return Result.Fail("There are no more seats available on this trip");
            }
            booking.Status = BookingStatus.Confirmed;
            _bookingRepo.UpdateBooking(booking);
            _bookingRepo.SaveChanges();
            var notification = new SendNotificationDto 
            {
                ReciverId = booking.PassengerId,
                Title = "Cinfirm request",
                Description = $"Your request for a trip from {booking.Trip.From} to {booking.Trip.To} has been confirmed"
            };
            return Result.Success();
        }

        public Result<int> CreateBooking(CreateBookingDto createBookingDto)
        {
            var trip = _tripRepo.GetTripById(createBookingDto.TripId);
            if (trip == null || trip.DriverId == createBookingDto.PassengerId)
            {
                return Result<int>.Fail("Invalid trip id");
            }
            if (_accountRepo.GetAppUserById(createBookingDto.PassengerId) == null)
            {
                return Result<int>.Fail("Invalid passenger id");
            }
            if (trip.Status == TripStatus.Canceled || trip.Status == TripStatus.Completed) 
            {
                return Result<int>.Fail("The application has already been confirmed");
            }
            // TODO: Use automapper
            var booking = new Booking
            {
                TripId = createBookingDto.TripId,
                PassengerId = createBookingDto.PassengerId,
                Status = BookingStatus.Waiting,
            };
            _bookingRepo.CreateBooking(booking);
            _bookingRepo.SaveChanges();
            return Result<int>.Success(booking.BookingId);
        }

        public Result<IEnumerable<BookingDto>> GetByTripId(int tripId)
        {
            return Result<IEnumerable<BookingDto>>.Success(_bookingRepo.GetBookingsByTripId(tripId)
                .Select(booking => new BookingDto
                {
                    BookingId = booking.BookingId,
                    TripId = booking.TripId,
                    PassengerId = booking.PassengerId,
                    PassengerName = booking.Passenger.UserName,
                    Status = booking.Status,
                })
                .ToList()
            );
        }
    }
}
