using Trip.App.ViewModels;
using Trip.Services.Dtos.Trips;
using Trip.Services;
using System.Linq;
using System;

namespace Trip.App.Commnds
{
    public class CancelTripCommand : CommandBase
    {
        private readonly MyTripViewModel _myTripViewModel;
        private readonly TripDto _tripDto;
        private readonly ITripService _tripService;

        public CancelTripCommand(
            MyTripViewModel myTripViewModel,
            TripDto tripDto,
            ITripService tripService
        )
        {
            _myTripViewModel = myTripViewModel;
            _tripDto = tripDto;
            _tripService = tripService;
        }

        public override void Execute(object? parameter)
        {
            try
            {
                var result = _tripService.CancelTrip(_tripDto.TripId);
                if (!result.Succeed)
                {
                    _myTripViewModel.ErrorMessage = result.Errors.First();
                    return;
                }
                _myTripViewModel.BackCommand.Execute(null);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
