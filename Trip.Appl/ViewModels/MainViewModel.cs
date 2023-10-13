using Trip.App.Stores;

namespace Trip.App.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        public ViewModelBase CurrentViewModel
            => _navigationStore.CurrentViewModel;

        public MainViewModel(
            NavigationStore navigationStore, 
            LoginViewModel viewModel
        )
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            navigationStore.CurrentViewModel = viewModel;
        }

        public void OnCurrentViewModelChanged() 
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
