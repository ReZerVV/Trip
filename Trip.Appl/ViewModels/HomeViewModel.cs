using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Trip.App.Commnds;
using Trip.App.Services;
using Trip.App.Stores;
using Trip.Services;
using Trip.Services.Dtos.Notifications;

namespace Trip.App.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private class NotificationDateComparer : IComparer<NotificationDto>
        {
            public int Compare(NotificationDto? x, NotificationDto? y)
            {
                return y.Date.CompareTo(x.Date);
            }
        }

        public HomeViewModel(
            AccountStore accountStore,
            INotificationService notificationService,
            NavigationService<SearchTripViewModel> navigationSearchTripViewModelService,
            NavigationService<CreateTripViewModel> navigationCreateTripViewModelService,
            NavigationService<MyTripsViewModel> navigationMyTripsViewModelService,
            NavigationService<ProfileViewModel> navigationProfileViewModelService
        )
        {
            {
                var notifications = notificationService.GetNotificationsByAppUserId(accountStore.CurrentAppUser.AppUserId).Data
                    .ToList();
                notifications.Sort(new NotificationDateComparer());
                Notifications = new ObservableCollection<NotificationDto>(notifications);
            } 
            ToSearchCommand = new NavigationCommand<SearchTripViewModel>(navigationSearchTripViewModelService);
            ToCreateCommand = new NavigationCommand<CreateTripViewModel>(navigationCreateTripViewModelService);
            ToMyTripsCommand = new NavigationCommand<MyTripsViewModel>(navigationMyTripsViewModelService);
            ToProfileCommand = new NavigationCommand<ProfileViewModel>(navigationProfileViewModelService);
        }

        public ObservableCollection<NotificationDto> Notifications { get; set; }

        public int NotificationsCount => Notifications.Count;

        public ICommand ToSearchCommand { get; }

        public ICommand ToCreateCommand { get; }

        public ICommand ToProfileCommand { get; }

        public ICommand ToMyTripsCommand { get; }
    }
}
