namespace Trip.Services.Dtos.Notifications
{
    public class SendNotificationDto
    {
        public int ReciverId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
