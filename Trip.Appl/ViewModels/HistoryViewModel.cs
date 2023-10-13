using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Trip.App.Commnds;
using Trip.App.Services;
using Trip.App.Stores;
using Trip.Domain;
using Trip.Services;
using Trip.Services.Dtos.Reviews;
using Trip.Services.Dtos.Trips;

namespace Trip.App.ViewModels
{
    public class HistoryViewModel : ViewModelBase
    {
        private readonly TripDto _tripDto;
        private readonly AccountStore _accountStore;

        public HistoryViewModel(
            NavigationStore navigationStore,
            AccountStore accountStore,
            TripDto tripDto,
            IAccountService accountService,
            ITripService tripService,
            IReviewService reviewService,
            NavigationService<LoginViewModel> navigationLoginViewModelService)
        {
            _accountStore = accountStore;
            _tripDto = tripDto;
            Reviews = new ObservableCollection<ReviewDto>(
                reviewService.GetReviewsByTripId(tripDto.TripId).Data.ToList()
            );
            BackCommand = new BackCommand(navigationStore);
            AddReviewCommand = new AddReviewCommand(
                this,
                reviewService);
            ToReviewerProfile = new ParameterNavigationCommand<ReviewDto, ProfileViewModel>(
                navigationStore,
                (ReviewDto reviewDto) => 
                    new ProfileViewModel(
                        reviewDto.ReviewerId,
                        accountStore,
                        navigationStore,
                        accountService,
                        tripService,
                        reviewService,
                        navigationLoginViewModelService)
            );
        }

        public ICommand AddReviewCommand { get; }

        public ICommand BackCommand { get; }

        public ICommand ToReviewerProfile { get; set; }

        private ObservableCollection<ReviewDto> reviews;
        public ObservableCollection<ReviewDto> Reviews 
        {
            get
            {
                return reviews;
            }
            set
            {
                reviews = value;
                OnPropertyChanged(nameof(Reviews));
            }
        }

        public ObservableCollection<bool> RatingCells
        {
            get
            {
                return new ObservableCollection<bool>
                {
                    Grade >= 1 ? true : false,
                    Grade >= 2 ? true : false,
                    Grade >= 3 ? true : false,
                    Grade >= 4 ? true : false,
                    Grade >= 5 ? true : false,
                };
            }
        }

        public int AppUserId => _accountStore.CurrentAppUser.AppUserId;

        public int TripId => _tripDto.TripId;

        public string DriverName => _tripDto.DriverName;

        public string From => _tripDto.From;

        public string To => _tripDto.To;

        public int NumberOfSeats => _tripDto.NumberOfSeats;

        public int NumberOfOccupiedSeats => _tripDto.NumberOfOccupiedSeats;

        public decimal Price => _tripDto.Price;

        private string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public int grade;
        public int Grade
        {
            get
            {
                return grade;
            }
            set
            {
                grade = value;
                OnPropertyChanged(nameof(Grade));
            }
        }
    }
}
