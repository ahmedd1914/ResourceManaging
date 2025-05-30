using ResourceManaging.Models;

namespace ResourceManaging.Services.DTOs.Reservation
{
    public class ReservationResourceData
    {
        public int ReservationId { get; set; }
        public int ResourceId { get; set; }

        public static ReservationResourceData FromReservationResource(Models.ReservationResource resource)
        {
            return new ReservationResourceData
            {
                ReservationId = resource.ReservationId,
                ResourceId = resource.ResourceId
            };
        }
    }
} 