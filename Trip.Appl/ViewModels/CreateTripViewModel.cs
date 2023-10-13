using System;
using System.Globalization;
using System.Windows.Input;
using Trip.App.Commnds;
using Trip.App.Services;
using Trip.App.Stores;
using Trip.Services;

namespace Trip.App.ViewModels
{
    public class CreateTripViewModel : ViewModelBase
    {
        public CreateTripViewModel(
            AccountStore accountStore,
            ITripService tripService,
            NavigationService<HomeViewModel> navigationHomeViewModelService
        )
        {
            CreateTripCommand = new CreateTripCommand(
                this, 
                accountStore,
                tripService,
                navigationHomeViewModelService
            );
            BackCommand = new NavigationCommand<HomeViewModel>(navigationHomeViewModelService);
            Date = DateOnly.FromDateTime(DateTime.Now);
            Time = TimeOnly.FromDateTime(DateTime.Now);
        }

        public ICommand CreateTripCommand { get; }
        
        public ICommand BackCommand { get; }

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

        private TimeOnly time;
        public TimeOnly Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
                OnPropertyChanged(nameof(Time));
            }
        }

        private int numberOfSeats;
        public int NumberOfSeats
        {
            get
            {
                return numberOfSeats;
            }
            set
            {
                numberOfSeats = value;
                OnPropertyChanged(nameof(numberOfSeats));
            }
        }

        private decimal price;
        public decimal Price
        {
            get
            {
                return price;
            }
            set
            {
                price = value;
                OnPropertyChanged(nameof(Price));
            }
        }
    }
}
