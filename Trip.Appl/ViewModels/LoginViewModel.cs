using System.Windows.Input;
using Trip.App.Commnds;
using Trip.App.Services;
using Trip.App.Stores;
using Trip.Services;

namespace Trip.App.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel(
            NavigationStore navigationStore,
            AccountStore accountStore,
            IAccountService accountService,
            NavigationService<HomeViewModel> navigationHomeViewModelService,
            NavigationService<RegisterViewModel> navigationRegisterViewModelService
        )
        {
            navigationStore.HistoryViewModel.Clear();
            LoginCommand = new LoginCommand(this, accountStore, accountService, navigationHomeViewModelService);
            ToRegisterCommand = new NavigationCommand<RegisterViewModel>(navigationRegisterViewModelService);
        }

        public ICommand LoginCommand { get; }

        public ICommand ToRegisterCommand { get; }

        private string email;
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private string password;
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
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
