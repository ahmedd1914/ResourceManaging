namespace ResourceManaging.Services.DTOs.Reservation
{
    public class ReservationNotificationResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ReservationNotificationData Notification { get; set; }
    }
} 