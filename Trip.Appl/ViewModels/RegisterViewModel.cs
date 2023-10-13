using System.Windows.Input;
using Trip.App.Commnds;
using Trip.App.Services;
using Trip.Services;

namespace Trip.App.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly IAccountService _accountService;

        public RegisterViewModel(
            IAccountService accountService,
            NavigationService<LoginViewModel> navigationLoginViewModelService
        )
        {
            _accountService = accountService;
            RegisterCommand = new RegisterCommand(this, _accountService, navigationLoginViewModelService);
            ToLoginCommand = new NavigationCommand<LoginViewModel>(navigationLoginViewModelService);
        }

        public ICommand RegisterCommand { get; }
        
        public ICommand ToLoginCommand { get; }

        private string userName;
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

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

        private string passwordConfirm;
        public string PasswordConfirm
        {
            get
            {
                return passwordConfirm;
            }
            set
            {
                passwordConfirm = value;
                OnPropertyChanged(nameof(PasswordConfirm));
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
