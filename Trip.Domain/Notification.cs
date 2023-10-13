namespace Trip.Domain
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public int ReciverId { get; set; }
        public AppUser Reciver { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
