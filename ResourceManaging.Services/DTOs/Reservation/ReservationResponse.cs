namespace ResourceManaging.Services.DTOs.Reservation
{
    public class ReservationResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ReservationData Reservation { get; set; }
    }
} 