using System.ComponentModel.DataAnnotations;

namespace ResourceManaging.Services.DTOs.Reservation
{
    public class CreateReservationNotificationRequest
    {
        [Required]
        public int ReservationId { get; set; }

        [Required]
        public char NotificationType { get; set; }

        [Required]
        [StringLength(500)]
        public string Message { get; set; }
    }
} 