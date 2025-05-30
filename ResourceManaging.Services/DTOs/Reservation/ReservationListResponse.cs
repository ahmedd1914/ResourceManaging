namespace ResourceManaging.Services.DTOs.Reservation
{
    public class ReservationListResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<ReservationData> Reservations { get; set; }
        public int TotalCount { get; set; }
    }
} 