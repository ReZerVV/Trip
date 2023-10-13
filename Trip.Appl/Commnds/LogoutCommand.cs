using Trip.App.Services;
using Trip.App.Stores;
using Trip.App.ViewModels;

namespace Trip.App.Commnds
{
    public class LogoutCommand : CommandBase
    {
        private readonly AccountStore _accountStore;
        private readonly NavigationService<LoginViewModel> _navigationLoginViewModelService;

        public LogoutCommand(
            AccountStore accountStore, 
            NavigationService<LoginViewModel> navigationLoginViewModelService
        )
        {
            _accountStore = accountStore;
            _navigationLoginViewModelService = navigationLoginViewModelService;
        }

        public override void Execute(object? parameter)
        {
            _accountStore.Logout();
            _navigationLoginViewModelService.Navigate();
        }
    }
}
