using Trip.App.Services;
using Trip.App.ViewModels;

namespace Trip.App.Commnds
{
    public class NavigationCommand<TViewModel> : CommandBase
        where TViewModel : ViewModelBase
    {
        private readonly NavigationService<TViewModel> _navigationService;

        public NavigationCommand(NavigationService<TViewModel> navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object parametr)
        {
            _navigationService.Navigate();
        }
    }
}
