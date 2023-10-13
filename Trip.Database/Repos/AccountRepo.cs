using Microsoft.EntityFrameworkCore;
using Trip.Domain;
using Trip.Repos;

namespace Trip.Database.Repos
{
    public class AccountRepo : IAccountRepo
    {
        private readonly AppDbContext appDbContext;

        public AccountRepo(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public void CreatAppUser(AppUser appUser)
        {
            appDbContext.Users.Add(appUser);
        }

        public void DeleteAppUser(int appUserId)
        {
            var appUser = appDbContext.Users.FirstOrDefault(appUser => appUser.AppUserId == appUserId);
            if (appUser == null)
            {
                throw new ArgumentException();
            }
            appDbContext.Users.Remove(appUser);
        }

        public AppUser? GetAppUserByEmail(string appUserEmail)
        {
            return appDbContext.Users
                .Include(appUser => appUser.Trips)
                .Include(appUser => appUser.Bookings)
                .Include(appUser => appUser.Reviews)
                .FirstOrDefault(appUser => appUser.Email == appUserEmail);
        }

        public AppUser? GetAppUserById(int appUserId)
        {
            return appDbContext.Users
                .Include(appUser => appUser.Trips)
                .Include(appUser => appUser.Bookings)
                .Include(appUser => appUser.Reviews)
                .FirstOrDefault(appUser => appUser.AppUserId == appUserId);
        }

        public AppUser? GetAppUserByUserName(string appUserUserName)
        {
            return appDbContext.Users
                .Include(appUser => appUser.Trips)
                .Include(appUser => appUser.Bookings)
                .Include(appUser => appUser.Reviews)
                .FirstOrDefault(appUser => appUser.UserName == appUserUserName);
        }

        public void SaveChanges()
        {
            appDbContext.SaveChanges();
        }

        public void UpdateAppUser(AppUser appUserEntity)
        {
            var appUser = appDbContext.Users.FirstOrDefault(appUser => appUser.AppUserId == appUserEntity.AppUserId);
            if (appUser == null)
            {
                throw new ArgumentException();
            }
            appUser.UserName = appUserEntity.UserName;
            appUser.Email = appUserEntity.Email;
            appUser.Password = appUserEntity.Password;
            appDbContext.Users.Update(appUser);
        }
    }
}
