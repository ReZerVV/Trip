using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Trip.App.ViewModels;
using Trip.Services.Dtos.Accounts;
using Trip.Services.Dtos.Bookings;

namespace Trip.App.Views
{
    /// <summary>
    /// Interaction logic for DetailsMyTripView.xaml
    /// </summary>
    public partial class DetailsMyTripView : UserControl
    {
        public DetailsMyTripView()
        {
            InitializeComponent();
        }

        private void DoneBooking_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is BookingDto booking)
            {
                var myTripsViewModel = this.DataContext as MyTripViewModel;
                myTripsViewModel.ConfirmBookingCommand.Execute(booking);
            }
        }

        private void CancelBooking_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is BookingDto booking)
            {
                var myTripsViewModel = this.DataContext as MyTripViewModel;
                myTripsViewModel.CancelBookingCommand.Execute(booking);
            }
        }

        private void UserViewProfile_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is BookingDto booking)
            {
                var myTripsViewModel = this.DataContext as MyTripViewModel;
                myTripsViewModel.ToUserProfileCommand.Execute(booking.PassengerId);
            }
        }
    }
}
