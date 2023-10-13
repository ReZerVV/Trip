using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Trip.App.Commnds;
using Trip.App.Services;
using Trip.App.Stores;
using Trip.Services;
using Trip.Services.Dtos.Trips;

namespace Trip.App.ViewModels
{
    public class SearchTripViewModel : ViewModelBase
    {
        public SearchTripViewModel(
            NavigationStore navigationStore,
            AccountStore accountStore,
            ITripService tripService,
            IBookingService bookingService,
            IReviewService reviewService,
            NavigationService<LoginViewModel> navigationLoginViewModelService)
        {
            SearchCommand = new SearchTripCommand(this, tripService);
            BackCommand = new BackCommand(navigationStore);
            Date = DateOnly.FromDateTime(DateTime.Now);
            Trips = new List<TripDto>();
            ToDetailsTripCommand = new ParameterNavigationCommand<TripDto, DetailsMyTripViewModel>(
                navigationStore,
                (TripDto param) => new DetailsMyTripViewModel(
                    navigationStore,
                    accountStore,
                    param,
                    tripService,
                    bookingService,
                    reviewService,
                    navigationLoginViewModelService
                )
            );
        }

        public ICommand ToDetailsTripCommand { get; }
        
        public ICommand SearchCommand { get; }

        public ICommand BackCommand { get; }

        public List<TripDto> trips;
        public List<TripDto> Trips 
        {
            get
            {
                return trips;
            }
            set
            {
                trips = value;
                OnPropertyChanged(nameof(Trips));
                OnPropertyChanged(nameof(MinPrice));
                OnPropertyChanged(nameof(MaxPrice));
            }
        }

        private string from;
        public string From
        {
            get
            {
                return from;
            }
            set
            {
                from = value;
                OnPropertyChanged(nameof(From));
            }
        }

        private string to;
        public string To
        {
            get
            {
                return to;
            }
            set
            {
                to = value;
                OnPropertyChanged(nameof(To));
            }
        }

        private DateOnly date;
        public DateOnly Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        private int currentPrice;
        public int CurrentPrice
        {
            get
            {
                return currentPrice;
            }
            set
            {
                currentPrice = value;
                OnPropertyChanged(nameof(CurrentPrice));
            }
        }

        public decimal MinPrice => Trips.Select(trip => trip.Price).Min();
        public decimal MaxPrice => Trips.Select(trip => trip.Price).Max();

        private string resultMessage;
        public string ResultMessage
        {
            get
            { 
                return resultMessage;
            }
            set
            { 
                resultMessage = value;
                OnPropertyChanged(nameof(ResultMessage));
            }
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            set
            {
                errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
    }
}
