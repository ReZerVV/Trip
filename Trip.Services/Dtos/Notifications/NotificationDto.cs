using Trip.Services.Dtos.Accounts;

namespace Trip.Services.Dtos.Notifications
{
    public class NotificationDto
    {
        public int NotificationId { get; set; }
        public AppUserDto Reciver { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
