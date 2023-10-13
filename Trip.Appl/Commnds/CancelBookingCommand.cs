using System;
using Trip.App.Stores;
using Trip.App.ViewModels;
using Trip.Services.Dtos.Bookings;
using Trip.Services;
using System.Linq;

namespace Trip.App.Commnds
{
    public class CancelBookingCommand : CommandBase
    {
        private readonly MyTripViewModel _myTripViewModel;
        private readonly AccountStore _accountStore;
        private readonly IBookingService _bookingService;

        public CancelBookingCommand(
            MyTripViewModel myTripViewModel,
            AccountStore accountStore,
            IBookingService bookingService
        )
        {
            _myTripViewModel = myTripViewModel;
            _accountStore = accountStore;
            _bookingService = bookingService;
        }

        public override void Execute(object? parameter)
        {
            try
            {
                var result = _bookingService.CancelBooking(new CancelBookingDto
                {
                    AppUserId = _accountStore.CurrentAppUser.AppUserId,
                    BookingId = (parameter as BookingDto).BookingId,
                });
                if (!result.Succeed)
                {
                    _myTripViewModel.ErrorMessage = result.Errors.First();
                }
                var booking = _myTripViewModel.Passengers.First(booking => booking.BookingId == (parameter as BookingDto).BookingId);
                _myTripViewModel.Bookings.Add(booking);
                _myTripViewModel.Passengers.Remove(booking);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
