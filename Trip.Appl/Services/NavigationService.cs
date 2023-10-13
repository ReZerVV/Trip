using System;
using Trip.App.Stores;
using Trip.App.ViewModels;

namespace Trip.App.Services
{
    public class NavigationService<TViewModel>
        where TViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<TViewModel> _viewModelFactory;

        public NavigationService(NavigationStore navigationStore, Func<TViewModel> viewModelFactory)
        {
            _navigationStore = navigationStore;
            _viewModelFactory = viewModelFactory;
        }
        
        public NavigationService(NavigationStore navigationStore, TViewModel viewModel)
        {
            _navigationStore = navigationStore;
            _viewModelFactory = () => viewModel;
        }
        
        public void Navigate()
        {
            _navigationStore.CurrentViewModel = _viewModelFactory.Invoke();
        }
    }
}
