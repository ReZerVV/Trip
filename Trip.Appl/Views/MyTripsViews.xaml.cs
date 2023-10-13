using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Trip.App.ViewModels;
using Trip.Services.Dtos.Trips;

namespace Trip.App.Views
{
    /// <summary>
    /// Interaction logic for MyTripsViews.xaml
    /// </summary>
    public partial class MyTripsViews : UserControl
    {
        public MyTripsViews()
        {
            InitializeComponent();
        }

        private void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is TripDto trip)
            {
                var myTripsViewModel = this.DataContext as MyTripsViewModel;
                myTripsViewModel.ToDetailsMyTripCommand.Execute(trip);
            }
        }
    }
}
