using ResourceManaging.Repository.Helpers;

namespace ResourceManaging.Repository.Interfaces.Reservation
{
    public class ReservationNotificationUpdate : Update
    {
        private int _notificationId;
        public int NotificationId
        {
            get => _notificationId;
            set
            {
                _notificationId = value;
                AddUpdate("NotificationId", value);
            }
        }

        public void UpdateReadStatus(bool isRead)
        {
            AddUpdate("IsRead", isRead);
        }

        public void UpdateMessage(string message)
        {
            AddUpdate("Message", message);
        }
    }
} 