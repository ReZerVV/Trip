using Trip.Services.Dtos.Notifications;

namespace Trip.Services.Helpers
{
    public static class NotificationHelper
    {
        public static SendNotificationDto WelcomeNotification(int appUserId) 
        {
            return new SendNotificationDto
            {
                ReciverId = appUserId,
                Title = "Welcome to Trip!",
                Description = "Thank you for joining the Trip, your go-to platform for ride-sharing adventures. We're thrilled to have you on board, and we can't wait to help you find and share exciting trips with fellow travelers. Whether you're a driver or a passenger, we is here to make your journeys more enjoyable and cost-effective. Safe travels!",
            };
        }
    }
}
