namespace ResourceManaging.Services.DTOs.Reservation
{
    public class ReservationResourceResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ReservationResourceData ReservationResource { get; set; }
    }
} 