using Trip.App.Stores;

namespace Trip.App.Commnds
{
    public class BackCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;

        public BackCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object? parameter)
        {
            _navigationStore.Back();
        }
    }
}
