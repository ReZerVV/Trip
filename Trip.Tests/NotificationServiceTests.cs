using Microsoft.EntityFrameworkCore;
using Trip.Database;
using Trip.Database.Repos;
using Trip.Domain;
using Trip.Services;
using Trip.Services.Dtos.Notifications;

namespace Trip.Tests
{
    [TestClass]
    public class NotificationServiceTests
    {
        [TestMethod]
        public void SendNotificationTest_1()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Trip.InMemory.Database")
                .Options;
            var appDbContext = new AppDbContext(options);
            {
                appDbContext.Users.AddRange(
                    new AppUser 
                    {
                        UserName = "cyril",
                        Email = "cyril@gmail.com",
                        Password = "13112003",
                    },
                    new AppUser
                    {
                        UserName = "cyril1",
                        Email = "cyril1@gmail.com",
                        Password = "13112003",
                    }
                );
            }
            appDbContext.SaveChanges();
            var notificationRepo = new NotificationRepo(appDbContext);
            var accountRepo = new AccountRepo(appDbContext);
            var notificationService = new NotificationService(notificationRepo, accountRepo);

            var sendNotificationDto = new SendNotificationDto 
            {
                ReciverId = 2,
                Title = "Hello!",
                Description = "...",
            };

            var result = notificationService.SendNotification(sendNotificationDto);

            Assert.IsTrue(result.Succeed);
            Assert.IsNotNull(appDbContext.Notifications.FirstOrDefault(notification => notification.NotificationId == 1));
        }

        [TestMethod]
        public void DeleteNotificationTest_1()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Trip.InMemory.Database")
                .Options;
            var appDbContext = new AppDbContext(options);
            {
                appDbContext.Users.AddRange(
                    new AppUser
                    {
                        UserName = "cyril",
                        Email = "cyril@gmail.com",
                        Password = "13112003",
                    },
                    new AppUser
                    {
                        UserName = "cyril1",
                        Email = "cyril1@gmail.com",
                        Password = "13112003",
                    }
                );
                appDbContext.Notifications.AddRange(
                    new Notification 
                    {
                        ReciverId = 2,
                        Title = "Hello!",
                        Description = "...",
                    }
                );
            }
            appDbContext.SaveChanges();
            var notificationRepo = new NotificationRepo(appDbContext);
            var accountRepo = new AccountRepo(appDbContext);
            var notificationService = new NotificationService(notificationRepo, accountRepo);

            var result = notificationService.DeleteNotification(1);

            Assert.IsTrue(result.Succeed);
            Assert.IsNull(appDbContext.Notifications.FirstOrDefault(notification => notification.IsDeleted == false &&
                                                                                    notification.NotificationId == 1));
        }

        [TestMethod]
        public void GetNotificationByIdTest_1()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Trip.InMemory.Database")
                .Options;
            var appDbContext = new AppDbContext(options);
            {
                appDbContext.Users.AddRange(
                    new AppUser
                    {
                        UserName = "cyril",
                        Email = "cyril@gmail.com",
                        Password = "13112003",
                    },
                    new AppUser
                    {
                        UserName = "cyril1",
                        Email = "cyril1@gmail.com",
                        Password = "13112003",
                    }
                );
                appDbContext.Notifications.AddRange(
                    new Notification
                    {
                        ReciverId = 2,
                        Title = "Hello!",
                        Description = "...",
                    }
                );
            }
            appDbContext.SaveChanges();
            var notificationRepo = new NotificationRepo(appDbContext);
            var accountRepo = new AccountRepo(appDbContext);
            var notificationService = new NotificationService(notificationRepo, accountRepo);

            var result = notificationService.GetNotificationById(1);

            Assert.IsTrue(result.Succeed);
            Assert.AreEqual("Hello!", result.Data.Title);
        }

        [TestMethod]
        public void GetNotificationaByAppUserIdTest_1()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Trip.InMemory.Database")
                .Options;
            var appDbContext = new AppDbContext(options);
            {
                appDbContext.Users.AddRange(
                    new AppUser
                    {
                        UserName = "cyril",
                        Email = "cyril@gmail.com",
                        Password = "13112003",
                    },
                    new AppUser
                    {
                        UserName = "cyril1",
                        Email = "cyril1@gmail.com",
                        Password = "13112003",
                    }
                );
                appDbContext.Notifications.AddRange(
                    new Notification
                    {
                        ReciverId = 2,
                        Title = "Hello!",
                        Description = "...",
                    }
                );
            }
            appDbContext.SaveChanges();
            var notificationRepo = new NotificationRepo(appDbContext);
            var accountRepo = new AccountRepo(appDbContext);
            var notificationService = new NotificationService(notificationRepo, accountRepo);

            var result = notificationService.GetNotificationsByAppUserId(2);

            Assert.IsTrue(result.Succeed);
            CollectionAssert.AreEqual(
                new int[] { 1 }, 
                result.Data.Select(notification => notification.NotificationId)
                .ToList()
            );
        }
    }
}
