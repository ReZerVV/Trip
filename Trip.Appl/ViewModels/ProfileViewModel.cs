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
    public class ProfileViewModel : ViewModelBase
    {
        private readonly AccountStore _accountStore;

        private class HistoryDateComparer : IComparer<TripDto>
        {
            public int Compare(TripDto? x, TripDto? y)
            {
                return y.Date.CompareTo(x.Date);
            }
        }

        public ProfileViewModel(
            NavigationStore navigationStore,
            AccountStore accountStore,
            ITripService tripService,
            IReviewService reviewService,
            NavigationService<LoginViewModel> navigationLoginViewModelService,
            bool logoutButtonDraw = true
        )
        {
            _accountStore = accountStore;
            var histories = tripService.GetHistoryTripByAppUserId(accountStore.CurrentAppUser.AppUserId).Data.ToList();
            histories.Sort(new HistoryDateComparer());
            Histories = new ObservableCollection<TripDto>(histories);
            ToHistoryCommand = new ParameterNavigationCommand<TripDto, HistoryViewModel>(
                navigationStore,
                (TripDto trip) => new HistoryViewModel(navigationStore, accountStore, trip, reviewService)
            );
            LogoutCommand = new LogoutCommand(accountStore, navigationLoginViewModelService);
            BackCommand = new BackCommand(navigationStore);
            IsLogoutButtonDraw = logoutButtonDraw;
        }

        public bool IsLogoutButtonDraw { get; }

        public ICommand ToHistoryCommand { get; }

        public ICommand LogoutCommand { get; }

        public ICommand BackCommand { get; }

        public ObservableCollection<TripDto> Histories { get; set; }
        
        public string UserName => _accountStore.CurrentAppUser.UserName;

        public string Email => _accountStore.CurrentAppUser.Email;
    }
}
