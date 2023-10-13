using Trip.Domain;

namespace Trip.Repos
{
    public interface INotificationRepo : IRepo
    {
        void CreateNotification(Notification notification);
        void UpdateNotification(Notification notification);
        Notification GetNotificationById(int id);
        IEnumerable<Notification> GetNotificationsByAppUserId(int appUserId);
        void DeleteNotification(int id);
    }
}
