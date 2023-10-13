using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Trip.App.Commnds;
using Trip.App.Services;
using Trip.App.Stores;
using Trip.Services;
using Trip.Services.Dtos.Accounts;
using Trip.Services.Dtos.Trips;

namespace Trip.App.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private readonly AppUserDto _account;

        private class HistoryDateComparer : IComparer<TripDto>
        {
            public int Compare(TripDto? x, TripDto? y)
            {
                return y.Date.CompareTo(x.Date);
            }
        }

        public ProfileViewModel(
            int accountId,
            AccountStore accountStore,
            NavigationStore navigationStore,
            IAccountService accountService,
            ITripService tripService,
            IReviewService reviewService,
            NavigationService<LoginViewModel> navigationLoginViewModelService)
        {
            var result = accountService.GetAppUserById(accountId);
            if (!result.Succeed)
            {
                navigationStore.Back();
            }
            _account = result.Data;
            var histories = tripService.GetHistoryTripByAppUserId(accountStore.CurrentAppUser.AppUserId).Data.ToList();
            histories.Sort(new HistoryDateComparer());
            Histories = new ObservableCollection<TripDto>(histories);
            IsMyAccount = accountStore.CurrentAppUser.AppUserId == accountId;

            ToHistoryCommand = new ParameterNavigationCommand<TripDto, HistoryViewModel>(
                navigationStore,
                (TripDto trip) => new HistoryViewModel(navigationStore, accountStore, trip, reviewService)
            );
            LogoutCommand = new LogoutCommand(accountStore, navigationLoginViewModelService);
            BackCommand = new BackCommand(navigationStore);
        }

        public ProfileViewModel(
            AccountStore accountStore,
            NavigationStore navigationStore,
            IAccountService accountService,
            ITripService tripService,
            IReviewService reviewService,
            NavigationService<LoginViewModel> navigationLoginViewModelService)
            : this(
                  accountStore.CurrentAppUser.AppUserId,
                  accountStore,
                  navigationStore,
                  accountService,
                  tripService,
                  reviewService,
                  navigationLoginViewModelService)
        {

        }

        public ICommand ToHistoryCommand { get; }

        public ICommand LogoutCommand { get; }

        public ICommand BackCommand { get; }

        public ObservableCollection<TripDto> Histories { get; set; }
        
        public string UserName => _account.UserName;

        public string Email => _account.Email;

        public bool IsMyAccount { get; }
    }
}
