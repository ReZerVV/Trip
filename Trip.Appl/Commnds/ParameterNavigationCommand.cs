using System;
using Trip.App.Stores;
using Trip.App.ViewModels;

namespace Trip.App.Commnds
{
    public class ParameterNavigationCommand<TParameter, TViewModel> : CommandBase
        where TViewModel : ViewModelBase 
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<TParameter, TViewModel> _viewModelFactory;

        public ParameterNavigationCommand(
            NavigationStore navigationStore,
            Func<TParameter, TViewModel> viewModelFactory
        )
        {
            _navigationStore = navigationStore;
            _viewModelFactory = viewModelFactory;
        }

        public override void Execute(object? parameter)
        {
            _navigationStore.CurrentViewModel = _viewModelFactory((TParameter)parameter);
        }
    }
}
