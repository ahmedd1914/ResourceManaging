using ResourceManaging.Models;

namespace ResourceManaging.Services.DTOs.Reservation
{
    public class ReservationNotificationData
    {
        public int NotificationId { get; set; }
        public int ReservationId { get; set; }
        public char NotificationType { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
        public string Message { get; set; }

        public static ReservationNotificationData FromNotification(Models.ReservationNotification notification)
        {
            return new ReservationNotificationData
            {
                NotificationId = notification.NotificationId,
                ReservationId = notification.ReservationId,
                NotificationType = notification.NotificationType,
                SentAt = notification.SentAt,
                IsRead = notification.IsRead,
                Message = notification.Message
            };
        }
    }
} 