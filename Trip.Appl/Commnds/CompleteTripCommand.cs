using System;
using System.Linq;
using Trip.App.ViewModels;
using Trip.Services;
using Trip.Services.Dtos.Trips;

namespace Trip.App.Commnds
{
    public class CompleteTripCommand : CommandBase
    {
        private readonly MyTripViewModel _myTripViewModel;
        private readonly TripDto _tripDto;
        private readonly ITripService _tripService;

        public CompleteTripCommand(
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
                var result = _tripService.CompleteTrip(_tripDto.TripId);
                if (!result.Succeed)
                {
                    _myTripViewModel.ErrorMessage = result.Errors.First();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
