using System;
using System.Linq;
using Trip.App.Services;
using Trip.App.Stores;
using Trip.App.ViewModels;
using Trip.Services;
using Trip.Services.Dtos.Accounts;

namespace Trip.App.Commnds
{
    public class LoginCommand : CommandBase
    {
        private readonly LoginViewModel _loginViewModel;
        private readonly AccountStore _accountStore;
        private readonly IAccountService _accountService;
        private readonly NavigationService<HomeViewModel> _navigationHomeViewModelService;  

        public LoginCommand(
            LoginViewModel loginViewModel, 
            AccountStore accountStore,
            IAccountService accountService,
            NavigationService<HomeViewModel> navigationHomeViewModelService
        )
        {
            _loginViewModel = loginViewModel;
            _accountStore = accountStore;
            _accountService = accountService;
            _navigationHomeViewModelService = navigationHomeViewModelService;
        }

        public override void Execute(object? parameter)
        {
            try
            {
                _loginViewModel.ErrorMessage = string.Empty;
                var result = _accountService.LoginAppUser(new LoginAppUserDto
                {
                    Email = _loginViewModel.Email,
                    Password = _loginViewModel.Password,
                });
                if (!result.Succeed)
                {
                    _loginViewModel.ErrorMessage = result.Errors.First();
                    return;
                }
                _accountStore.CurrentAppUser = result.Data;
                _navigationHomeViewModelService.Navigate();
            }
            catch (Exception ex) 
            {
                _loginViewModel.ErrorMessage = "You may not have an internet connection";
            }
        }
    }
}
