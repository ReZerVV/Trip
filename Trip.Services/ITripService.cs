using Trip.Service;
using Trip.Services.Dtos.Trips;

namespace Trip.Services
{
    public interface ITripService
    {
        Result<int> CreateTrip(CreateTripDto createTripDto);
        Result<TripDto> GetTirpById(int tripId);
        Result EditTrip(EditTripDto editTripDto);
        Result CancelTrip(int tripId);
        Result CompleteTrip(int tripId);
        Result<IEnumerable<TripDto>> GetHistoryTripByAppUserId(int appUserId);
        Result<IEnumerable<TripDto>> GetTripsByAppUserId(int appUserId);
        Result<IEnumerable<TripDto>> SearchTrip(SearchQueryTripDto searchQueryTripDto);
    }
}
