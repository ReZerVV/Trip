using Trip.App.Services;
using Trip.App.Stores;
using Trip.Services;
using Trip.Services.Dtos.Accounts;

namespace Trip.App.ViewModels
{
    public class PublicProfileViewModel : ProfileViewModel
    {
        public PublicProfileViewModel(
            NavigationStore navigationStore, 
            int appUserId, 
            ITripService tripService, 
            IReviewService reviewService,
            NavigationService<LoginViewModel> navigationLoginViewModelService) 
            : base(
                navigationStore, 
                new AccountStore { CurrentAppUser = new AppUserDto { AppUserId = appUserId } }, 
                tripService, 
                reviewService,
                navigationLoginViewModelService, 
                false)
        {
        }
    }
}
