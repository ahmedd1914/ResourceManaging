using System.ComponentModel.DataAnnotations;

namespace ResourceManaging.Services.DTOs.Reservation
{
    public class CreateReservationRequest
    {
        [Required]
        public int ReservorId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EndTime { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Purpose must be between 3 and 255 characters")]
        public string Purpose { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Participants must be at least 1")]
        public int Participants { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one resource must be selected")]
        public List<int> ResourceIds { get; set; } = new List<int>();
    }
} 