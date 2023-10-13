using System;
using System.Collections.Generic;
using Trip.App.ViewModels;

namespace Trip.App.Stores
{
    public class NavigationStore
    {
        public event Action CurrentViewModelChanged;

        private ViewModelBase currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return currentViewModel;
            }
            set
            {
                if (currentViewModel != null)
                { 
                    HistoryViewModel.Push(currentViewModel);
                }
                currentViewModel = value;
                CurrentViewModelChanged.Invoke();
            }
        }

        public Stack<ViewModelBase> HistoryViewModel = new Stack<ViewModelBase>();

        public void Back() 
        {
            if (HistoryViewModel.TryPeek(out ViewModelBase viewModel))
            {
                currentViewModel = viewModel;
                CurrentViewModelChanged.Invoke();
                HistoryViewModel.Pop();
            }
        }
    }
}
