using System.Windows.Input;
using Trip.App.Commnds;
using Trip.App.Services;
using Trip.App.Stores;
using Trip.Services;
using Trip.Services.Dtos.Trips;

namespace Trip.App.ViewModels
{
    public class DetailsMyTripViewModel : ViewModelBase
    {
        public DetailsMyTripViewModel(
            NavigationStore navigationStore,
            AccountStore accountStore,
            TripDto tripDto,
            ITripService tripService,
            IBookingService bookingService,
            IReviewService reviewService,
            NavigationService<LoginViewModel> navigationLoginViewModelService
        )
        {
            BookingTripCommand = new BookingTripCommand(
                this,
                accountStore,
                bookingService,
                navigationStore
            );
            BackCommand = new BackCommand(navigationStore);
            ToDriverProfile = new NavigationCommand<PublicProfileViewModel>(
                new NavigationService<PublicProfileViewModel>(
                    navigationStore,
                    () => new PublicProfileViewModel(
                        navigationStore,
                        tripDto.DriverId,
                        tripService,
                        reviewService,
                        navigationLoginViewModelService
                    )
                )
            );
            TripId = tripDto.TripId;
            DriverName = tripDto.DriverName;
            From = tripDto.From;
            To = tripDto.To;
            NumberOfSeats = tripDto.NumberOfSeats;
            NumberOfOccupiedSeats = tripDto.NumberOfOccupiedSeats;
            Price = tripDto.Price;
        }

        public ICommand BackCommand { get; }

        public ICommand ToDriverProfile { get; }

        public ICommand BookingTripCommand { get; }

        public int TripId { get; }

        public string DriverName { get; }

        public string From { get; }
        
        public string To { get; }

        public int NumberOfSeats { get; }

        public int NumberOfOccupiedSeats { get; }

        public decimal Price { get; }
    }
}
