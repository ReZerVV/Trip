using Microsoft.EntityFrameworkCore;
using Trip.Domain;
using Trip.Repos;

namespace Trip.Database.Repos
{
    public class NotificationRepo : INotificationRepo
    {
        private readonly AppDbContext _appDbContext;

        public NotificationRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void CreateNotification(Notification notification)
        {
            _appDbContext.Notifications.Add(notification);
        }

        public void DeleteNotification(int id)
        {
            var notification = _appDbContext.Notifications.FirstOrDefault(notification => notification.NotificationId == id);
            if (notification == null)
            {
                throw new ArgumentException();
            }
            _appDbContext.Notifications.Remove(notification);
        }

        public Notification? GetNotificationById(int id)
        {
            return _appDbContext.Notifications
                .Include(notification => notification.Reciver)
                .FirstOrDefault(notification => notification.IsDeleted == false &&
                                                notification.NotificationId == id);
        }

        public IEnumerable<Notification> GetNotificationsByAppUserId(int appUserId)
        {
            return _appDbContext.Notifications
                .Include(notification => notification.Reciver)
                .Where(notification => notification.IsDeleted == false)
                .Where(notification => notification.ReciverId == appUserId)
                .ToList();
        }

        public void SaveChanges()
        {
            _appDbContext.SaveChanges();
        }

        public void UpdateNotification(Notification notificationEntity)
        {
            var notification = _appDbContext.Notifications.FirstOrDefault(notification => notification.NotificationId == notificationEntity.NotificationId);
            if (notification == null)
            {
                throw new ArgumentException();
            }
            notification.IsDeleted = notificationEntity.IsDeleted;
            notification.Title = notificationEntity.Title;
            notification.Description = notificationEntity.Description;
        }
    }
}
