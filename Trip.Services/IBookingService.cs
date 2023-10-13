using Trip.Service;
using Trip.Services.Dtos.Bookings;

namespace Trip.Services
{
    public interface IBookingService
    {
        Result<int> CreateBooking(CreateBookingDto createBookingDto);
        Result CancelBooking(CancelBookingDto cancelBookingDto);
        Result ConfirmBooking(ConfirmBookingDto confirmBookingDto);
        Result<IEnumerable<BookingDto>> GetByTripId(int tripId);
    }
}
