using System;
using Trip.App.Stores;
using Trip.App.ViewModels;
using Trip.Services;
using Trip.Services.Dtos.Bookings;

namespace Trip.App.Commnds
{
    public class BookingTripCommand : CommandBase
    {
        private readonly DetailsMyTripViewModel _detailsTripViewModel;
        private readonly AccountStore _accountStore;
        private readonly IBookingService _bookingService;
        private readonly NavigationStore _navigationStore;

        public BookingTripCommand(
            DetailsMyTripViewModel detailsTripViewModel, 
            AccountStore accountStore, 
            IBookingService bookingService,
            NavigationStore navigationStore
        )
        {
            _detailsTripViewModel = detailsTripViewModel;
            _accountStore = accountStore;
            _bookingService = bookingService;
            _navigationStore = navigationStore;
        }

        public override void Execute(object? parameter)
        {
            try
            {
                var result = _bookingService.CreateBooking(new CreateBookingDto
                {
                    TripId = _detailsTripViewModel.TripId,
                    PassengerId = _accountStore.CurrentAppUser.AppUserId,
                });
                if (result.Succeed)
                {
                    _navigationStore.Back();
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
