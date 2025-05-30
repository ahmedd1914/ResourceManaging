using ResourceManaging.Repository.Helpers;

namespace ResourceManaging.Repository.Interfaces.Reservation
{
    public class ReservationNotificationFilter : Filter
    {
        public void AddReservationFilter(int reservationId)
        {
            AddParameter("ReservationId", reservationId);
        }

        public void AddNotificationTypeFilter(char notificationType)
        {
            AddParameter("NotificationType", notificationType);
        }

        public void AddReadStatusFilter(bool isRead)
        {
            AddParameter("IsRead", isRead);
        }
    }
} 