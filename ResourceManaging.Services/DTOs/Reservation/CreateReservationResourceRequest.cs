using System.ComponentModel.DataAnnotations;

namespace ResourceManaging.Services.DTOs.Reservation
{
    public class CreateReservationResourceRequest
    {
        [Required]
        public int ReservationId { get; set; }

        [Required]
        public int ResourceId { get; set; }
    }
} 