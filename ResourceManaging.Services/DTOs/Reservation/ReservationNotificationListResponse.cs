namespace ResourceManaging.Services.DTOs.Reservation
{
    public class ReservationNotificationListResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<ReservationNotificationData> Notifications { get; set; }
        public int TotalCount { get; set; }
    }
} 