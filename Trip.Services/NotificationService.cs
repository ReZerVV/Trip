using Trip.Domain;
using Trip.Repos;
using Trip.Service;
using Trip.Services.Dtos.Accounts;
using Trip.Services.Dtos.Notifications;

namespace Trip.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepo _notificationRepo;
        private readonly IAccountRepo _accountRepo;

        public NotificationService(INotificationRepo notificationRepo, IAccountRepo accountRepo)
        {
            _notificationRepo = notificationRepo;
            _accountRepo = accountRepo;
        }

        public Result DeleteNotification(int id)
        {
            var notification = _notificationRepo.GetNotificationById(id);
            if (notification == null)
            {
                return Result.Fail("Notification id not found");
            }
            notification.IsDeleted = true;
            _notificationRepo.UpdateNotification(notification);
            _notificationRepo.SaveChanges();
            return Result.Success();
        }

        public Result<NotificationDto> GetNotificationById(int id)
        {
            var notification = _notificationRepo.GetNotificationById(id);
            if (notification == null) 
            {
                return Result<NotificationDto>.Fail("Notification not found");
            }
            return Result<NotificationDto>.Success(new NotificationDto 
            {
                NotificationId = notification.NotificationId,
                Reciver = new AppUserDto
                {
                    AppUserId = notification.Reciver.AppUserId,
                    UserName = notification.Reciver.UserName,
                    Email = notification.Reciver.Email,
                },
                Date = notification.Date,
                Title = notification.Title,
                Description = notification.Description,
            });
        }

        public Result<IEnumerable<NotificationDto>> GetNotificationsByAppUserId(int appUserId)
        {
            return Result<IEnumerable<NotificationDto>>.Success(
                _notificationRepo.GetNotificationsByAppUserId(appUserId)
                    .Select(notification => new NotificationDto
                    {
                        NotificationId = notification.NotificationId,
                        Reciver = new AppUserDto
                        {
                            AppUserId = notification.Reciver.AppUserId,
                            UserName = notification.Reciver.UserName,
                            Email = notification.Reciver.Email,
                        },
                        Date = notification.Date,
                        Title = notification.Title,
                        Description = notification.Description,
                    })
                    .ToList()
            );
        }

        public Result SendNotification(SendNotificationDto sendNotificationDto)
        {
            if (_accountRepo.GetAppUserById(sendNotificationDto.ReciverId) == null)
            {
                return Result.Fail("Reciver id not found");
            }
            var notification = new Notification
            {
                ReciverId = sendNotificationDto.ReciverId,
                Date = DateTime.Now,
                IsDeleted = false,
                Title = sendNotificationDto.Title,
                Description = sendNotificationDto.Description,
            };
            _notificationRepo.CreateNotification(notification);
            _notificationRepo.SaveChanges();
            return  Result.Success();
        }
    }
}