using Trip.Domain;

namespace Trip.Repos
{
    public interface IAccountRepo : IRepo
    {
        void CreatAppUser(AppUser appUser);
        AppUser GetAppUserById(int appUserId);
        AppUser GetAppUserByEmail(string appUserEmail);
        AppUser GetAppUserByUserName(string appUserUserName);
        void UpdateAppUser(AppUser appUser);
        void DeleteAppUser(int appUserId);
    }
}
