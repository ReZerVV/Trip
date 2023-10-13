using Microsoft.EntityFrameworkCore;
using Trip.Database;
using Trip.Database.Repos;
using Trip.Services;
using Trip.Services.Dtos.Accounts;

namespace Trip.Tests
{
    [TestClass]
    public class AccountServiceTests
    {
        [TestMethod]
        public void LoginAppUserTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Trip.InMemory.Database")
                .Options;
            var appDbContext = new AppDbContext(options);
            {
                appDbContext.Users.Add(new Domain.AppUser
                {
                    UserName = "cyril",
                    Email = "cyril@gmail.com",
                    Password = "00000000",
                });
                appDbContext.SaveChanges();
            }
            var accountRepo = new AccountRepo(appDbContext);
            var notificationRepo = new NotificationRepo(appDbContext);
            var notificationService = new NotificationService(notificationRepo, accountRepo);
            var accountService = new AccountService(accountRepo, notificationService);

            var loginAppUserDto = new LoginAppUserDto 
            {
                Email = "cyril@gmail.com",
                Password = "00000000",
            };
            
            var result = accountService.LoginAppUser(loginAppUserDto);
            
            Assert.IsTrue(result.Succeed);
            Assert.AreEqual("cyril", result.Data.UserName);
            Assert.AreEqual("cyril@gmail.com", result.Data.Email);
            Assert.AreEqual("00000000", result.Data.Password);
        }

        [TestMethod]
        public void LoginNonExistentAppUserTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Trip.InMemory.Database")
                .Options;
            var appDbContext = new AppDbContext(options);
            var accountRepo = new AccountRepo(appDbContext);
            var notificationRepo = new NotificationRepo(appDbContext);
            var notificationService = new NotificationService(notificationRepo, accountRepo);
            var accountService = new AccountService(accountRepo, notificationService);

            var loginAppUserDto = new LoginAppUserDto
            {
                Email = "cyril@gmail.com",
                Password = "00000000",
            };

            var result = accountService.LoginAppUser(loginAppUserDto);

            Assert.IsFalse(result.Succeed);
        }

        [TestMethod]
        public void CreateAppUserTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Trip.InMemory.Database")
                .Options;
            var appDbContext = new AppDbContext(options);
            var accountRepo = new AccountRepo(appDbContext);
            var notificationRepo = new NotificationRepo(appDbContext);
            var notificationService = new NotificationService(notificationRepo, accountRepo);
            var accountService = new AccountService(accountRepo, notificationService);

            var createAppUserDto = new CreateAppUserDto 
            {
                UserName = "cyril",
                Email = "cyril@gmail.com",
                Password = "00000000",
                PasswordConfirm = "00000000",
            };

            var result = accountService.CreateAppUser(createAppUserDto);
            
            Assert.IsTrue(result.Succeed);
        }

        [TestMethod]
        public void CreateExistingAppUserTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Trip.InMemory.Database")
                .Options;
            var appDbContext = new AppDbContext(options);
            {
                appDbContext.Users.Add(new Domain.AppUser
                {
                    UserName = "cyril",
                    Email = "cyril@gmail.com",
                    Password = "00000000",
                });
                appDbContext.SaveChanges();
            }
            var accountRepo = new AccountRepo(appDbContext);
            var notificationRepo = new NotificationRepo(appDbContext);
            var notificationService = new NotificationService(notificationRepo, accountRepo);
            var accountService = new AccountService(accountRepo, notificationService);

            var createAppUserDto = new CreateAppUserDto
            {
                UserName = "cyril",
                Email = "cyril@gmail.com",
                Password = "00000000",
                PasswordConfirm = "00000000",
            };

            var result = accountService.CreateAppUser(createAppUserDto);

            Assert.IsFalse(result.Succeed);
        }

        [TestMethod]
        public void GetAppUserByIdTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Trip.InMemory.Database")
                .Options;
            var appDbContext = new AppDbContext(options);
            {
                appDbContext.Users.Add(new Domain.AppUser
                {
                    UserName = "cyril",
                    Email = "cyril@gmail.com",
                    Password = "00000000",
                });
                appDbContext.SaveChanges();
            }
            var accountRepo = new AccountRepo(appDbContext);
            var notificationRepo = new NotificationRepo(appDbContext);
            var notificationService = new NotificationService(notificationRepo, accountRepo);
            var accountService = new AccountService(accountRepo, notificationService);

            var result = accountService.GetAppUserById(1);

            Assert.IsTrue(result.Succeed);
            Assert.AreEqual("cyril", result.Data.UserName);
            Assert.AreEqual("cyril@gmail.com", result.Data.Email);
            Assert.AreEqual("00000000", result.Data.Password);
        }
    }
}
