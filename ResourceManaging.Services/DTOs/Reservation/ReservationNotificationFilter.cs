namespace ResourceManaging.Services.DTOs.Reservation
{
    public class ReservationNotificationFilter
    {
        public int? ReservationId { get; set; }
        public bool? IsRead { get; set; }
        public char? NotificationType { get; set; }
    }
} 