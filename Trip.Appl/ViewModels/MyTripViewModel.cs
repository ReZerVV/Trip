using System.Collections.ObjectModel;
using System.Windows.Input;
using Trip.App.Commnds;
using Trip.App.Services;
using Trip.App.Stores;
using Trip.Services;
using Trip.Services.Dtos.Bookings;
using Trip.Services.Dtos.Trips;

namespace Trip.App.ViewModels
{
    public class MyTripViewModel : ViewModelBase
    {
        public MyTripViewModel(
            TripDto tripDto,
            NavigationStore navigationStore,
            AccountStore accountStore,
            IAccountService accountService,
            ITripService tripService,
            IReviewService reviewService,
            IBookingService bookingService,
            NavigationService<LoginViewModel> navigationLoginViewModelService
        )
        {
            Bookings = new ObservableCollection<BookingDto>(
                bookingService.GetByTripId(tripDto.TripId).Data
            );
            Passengers = new ObservableCollection<BookingDto>();
            BackCommand = new BackCommand(navigationStore);
            CompleteTripCommand = new CompleteTripCommand(
                this,
                tripDto,
                tripService
            );
            CancelTripCommand = new CancelTripCommand(
                this,
                tripDto,
                tripService
            );
            ConfirmBookingCommand = new ConfirmBookingCommand(
                this,
                accountStore,
                bookingService
            );
            CancelBookingCommand = new CancelBookingCommand(
                this,
                accountStore,
                bookingService
            );
            ToUserProfileCommand = new ParameterNavigationCommand<int, ProfileViewModel>(
                navigationStore,
                (int appUserId) => new ProfileViewModel(
                    appUserId,
                    accountStore,
                    navigationStore,
                    accountService,
                    tripService,
                    reviewService,
                    navigationLoginViewModelService));
        }

        public ICommand BackCommand { get; }

        public ICommand ToUserProfileCommand { get; }

        public ICommand CompleteTripCommand { get; }

        public ICommand CancelTripCommand { get; }

        public ObservableCollection<BookingDto> Bookings { get; set; }

        public ObservableCollection<BookingDto> Passengers { get; set; }

        public ICommand ConfirmBookingCommand { get; }
        
        public ICommand CancelBookingCommand { get; }

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
