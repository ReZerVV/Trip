using Trip.Services.Dtos.Accounts;

namespace Trip.App.Stores
{
    public class AccountStore
    {
        public AppUserDto CurrentAppUser { get; set; }

        public void Logout()
        {
            CurrentAppUser = null;
        }
    }
}
