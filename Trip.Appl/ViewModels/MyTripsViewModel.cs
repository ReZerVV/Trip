using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Trip.App.Commnds;
using Trip.App.Services;
using Trip.App.Stores;
using Trip.Services;
using Trip.Services.Dtos.Trips;

namespace Trip.App.ViewModels
{
    public class MyTripsViewModel : ViewModelBase
    {
        private class TripDataComparer : IComparer<TripDto>
        {
            public int Compare(TripDto? x, TripDto? y)
            {
                return y.Date.CompareTo(x.Date);
            }
        }

        public MyTripsViewModel(
            AccountStore accountStore,
            NavigationStore navigationStore,
            ITripService tripService,
            IReviewService reviewService,
            IBookingService bookingService,
            NavigationService<LoginViewModel> navigationLoginViewModelService)
        {
            BackCommand = new BackCommand(navigationStore);
            {
                var trips = tripService.GetTripsByAppUserId(accountStore.CurrentAppUser.AppUserId).Data.ToList();
                trips.Sort(new TripDataComparer());
                Trips = new ObservableCollection<TripDto>(trips);
            }
            ToDetailsMyTripCommand = new ParameterNavigationCommand<TripDto, MyTripViewModel>(
                navigationStore,
                (TripDto param) => new MyTripViewModel(
                    param,
                    navigationStore,
                    accountStore,
                    tripService,
                    reviewService,
                    bookingService,
                    navigationLoginViewModelService)
            );
        }

        public ICommand ToDetailsMyTripCommand { get; }

        public ICommand BackCommand { get; }

        public ObservableCollection<TripDto> Trips { get; set; }
    }
}
