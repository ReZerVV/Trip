using System.Windows;
using System.Windows.Controls;
using Trip.App.ViewModels;
using Trip.Services.Dtos.Trips;

namespace Trip.App.Views
{
    /// <summary>
    /// Interaction logic for ProfileView.xaml
    /// </summary>
    public partial class ProfileView : UserControl
    {
        public ProfileView()
        {
            InitializeComponent();
        }

        private void MyButton_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is TripDto trip)
            {
                var profileViewModel = this.DataContext as ProfileViewModel;
                profileViewModel.ToHistoryCommand.Execute(trip);
            }
        }
    }
}
