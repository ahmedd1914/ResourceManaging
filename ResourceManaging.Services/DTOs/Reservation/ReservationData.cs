using ResourceManaging.Models;

namespace ResourceManaging.Services.DTOs.Reservation
{
    public class ReservationData
    {
        public int ReservationId { get; set; }
        public int ReservorId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Purpose { get; set; }
        public int Participants { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<int> ResourceIds { get; set; } = new List<int>();

        public static ReservationData FromReservation(Models.Reservation reservation, List<int> resourceIds)
        {
            return new ReservationData
            {
                ReservationId = reservation.ReservationId,
                ReservorId = reservation.ReservorId,
                StartTime = reservation.StartTime,
                EndTime = reservation.EndTime,
                Purpose = reservation.Purpose,
                Participants = reservation.Participants,
                IsCancelled = reservation.IsCancelled,
                CreatedAt = reservation.CreatedAt,
                ResourceIds = resourceIds ?? new List<int>()
            };
        }
    }
} 