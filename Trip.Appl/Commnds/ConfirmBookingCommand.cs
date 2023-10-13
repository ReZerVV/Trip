using System;
using System.Linq;
using Trip.App.Stores;
using Trip.App.ViewModels;
using Trip.Services;
using Trip.Services.Dtos.Bookings;

namespace Trip.App.Commnds
{
    public class ConfirmBookingCommand : CommandBase
    {
        private readonly MyTripViewModel _myTripViewModel;
        private readonly AccountStore _accountStore;
        private readonly IBookingService _bookingService;

        public ConfirmBookingCommand(
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
                var result = _bookingService.ConfirmBooking(new ConfirmBookingDto 
                {
                    AppUserId = _accountStore.CurrentAppUser.AppUserId,
                    BookingId = (parameter as BookingDto).BookingId,
                });
                if (!result.Succeed)
                {
                    _myTripViewModel.ErrorMessage = result.Errors.First();
                    return;
                }
                var booking = _myTripViewModel.Bookings.First(booking => booking.BookingId == (parameter as BookingDto).BookingId);
                _myTripViewModel.Passengers.Add(booking);
                _myTripViewModel.Bookings.Remove(booking);
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
