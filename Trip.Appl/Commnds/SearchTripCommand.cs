using System;
using System.Linq;
using Trip.App.ViewModels;
using Trip.Services;
using Trip.Services.Dtos.Trips;

namespace Trip.App.Commnds
{
    public class SearchTripCommand : CommandBase
    {
        private readonly SearchTripViewModel _searchTripViewModel;
        private readonly ITripService _tripService;

        public SearchTripCommand(SearchTripViewModel searchTripViewModel, ITripService tripService)
        {
            _searchTripViewModel = searchTripViewModel;
            _tripService = tripService;
        }

        public override void Execute(object? parameter)
        {
            try
            {
                _searchTripViewModel.ErrorMessage = string.Empty;
                _searchTripViewModel.ResultMessage = string.Empty;
                var searchQueryTripDto = new SearchQueryTripDto
                {
                    Date = _searchTripViewModel.Date.ToDateTime(TimeOnly.FromDateTime(DateTime.Now).AddMinutes(1)),
                    From = _searchTripViewModel.From,
                    To = _searchTripViewModel.To,
                    MinPrice = _searchTripViewModel.MinPrice,
                    MaxPrice = _searchTripViewModel.MaxPrice,
                };
                var result = _tripService.SearchTrip(searchQueryTripDto);
                if (!result.Succeed)
                {
                    _searchTripViewModel.ErrorMessage = result.Errors.First();
                    return;
                }
                _searchTripViewModel.Trips = result.Data.ToList();
                _searchTripViewModel.ResultMessage = result.Data.Count() == 0 ? "There are no trips available at the moment" : string.Empty;
            }
            catch (Exception ex)
            {
                _searchTripViewModel.ErrorMessage = "An unexpected error occurred";
            }
        }
    }
}
