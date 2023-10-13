using Trip.Service;
using Trip.Services.Dtos.Notifications;

namespace Trip.Services
{
    public interface INotificationService
    {
        Result SendNotification(SendNotificationDto sendNotificationDto);
        Result DeleteNotification(int id);
        Result<NotificationDto> GetNotificationById(int id);
        Result<IEnumerable<NotificationDto>> GetNotificationsByAppUserId(int appUserId);
    }
}