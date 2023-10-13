using System;
using System.Linq;
using Trip.App.Services;
using Trip.App.Stores;
using Trip.App.ViewModels;
using Trip.Service;
using Trip.Services;
using Trip.Services.Dtos.Trips;

namespace Trip.App.Commnds
{
    public class CreateTripCommand : CommandBase
    {
        private readonly CreateTripViewModel _createTripViewModel;
        private readonly AccountStore _accountStore;
        private readonly ITripService _tripService;
        private readonly NavigationService<HomeViewModel> _navigationHomeViewModelService;

        public CreateTripCommand(
            CreateTripViewModel createTripViewModel, 
            AccountStore accountStore,
            ITripService tripService, 
            NavigationService<HomeViewModel> navigationHomeViewModelService
        )
        {
            _createTripViewModel = createTripViewModel;
            _accountStore = accountStore;
            _tripService = tripService;
            _navigationHomeViewModelService = navigationHomeViewModelService;
        }

        public override void Execute(object? parameter)
        {
            try
            {
                _createTripViewModel.ErrorMessage = string.Empty;
                var result = _tripService.CreateTrip(new CreateTripDto 
                {
                    DriverId = _accountStore.CurrentAppUser.AppUserId,
                    Date = _createTripViewModel.Date.ToDateTime(_createTripViewModel.Time),
                    From = _createTripViewModel.From,
                    To = _createTripViewModel.To,
                    NumberOfSeats = _createTripViewModel.NumberOfSeats,
                    Price = _createTripViewModel.Price,
                });
                if (!result.Succeed)
                {
                    _createTripViewModel.ErrorMessage = result.Errors.First();
                    return;
                }
                _navigationHomeViewModelService.Navigate();
            }
            catch (Exception ex)
            {
                _createTripViewModel.ErrorMessage = "Error: no internet connection";
            }
        }
    }
}
