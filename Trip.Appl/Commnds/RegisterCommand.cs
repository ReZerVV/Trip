using System.Linq;
using System;
using Trip.App.ViewModels;
using Trip.Services;
using Trip.Services.Dtos.Accounts;
using Trip.App.Services;

namespace Trip.App.Commnds
{
    public class RegisterCommand : CommandBase
    {
        private readonly RegisterViewModel _registerViewModel;
        private readonly IAccountService _accountService;
        private readonly NavigationService<LoginViewModel> _navigationLoginViewModelService;

        public RegisterCommand(
            RegisterViewModel registerViewModel,
            IAccountService accountService,
            NavigationService<LoginViewModel> navigationLoginViewModelService
        )
        {
            _registerViewModel = registerViewModel;
            _accountService = accountService;
            _navigationLoginViewModelService = navigationLoginViewModelService;
        }

        public override void Execute(object? parameter)
        {
            try
            {
                _registerViewModel.ErrorMessage = string.Empty;
                var result = _accountService.CreateAppUser(new CreateAppUserDto
                {
                    UserName = _registerViewModel.UserName,
                    Email = _registerViewModel.Email,
                    Password = _registerViewModel.Password,
                    PasswordConfirm = _registerViewModel.PasswordConfirm,
                });
                if (!result.Succeed)
                {
                    _registerViewModel.ErrorMessage = result.Errors.First();
                    return;
                }
                _navigationLoginViewModelService.Navigate();
            }
            catch (Exception ex)
            {
                _registerViewModel.ErrorMessage = "You may not have an internet connection";
            }
        }
    }
}
