namespace ResourceManaging.Services.DTOs.Reservation
{
    public class ReservationResourceListResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<ReservationResourceData> ReservationResources { get; set; }
        public int TotalCount { get; set; }
    }
} 