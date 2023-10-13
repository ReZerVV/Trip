using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Trip.App.ViewModels;
using Trip.Services.Dtos.Trips;

namespace Trip.App.Views
{
    /// <summary>
    /// Interaction logic for SearchTripView.xaml
    /// </summary>
    public partial class SearchTripView : UserControl
    {
        public SearchTripView()
        {
            InitializeComponent();
        }

        private void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is TripDto trip)
            {
                var searchTripViewModel = this.DataContext as SearchTripViewModel;
                searchTripViewModel.ToDetailsTripCommand.Execute(trip);
            }
        }
    }
}
