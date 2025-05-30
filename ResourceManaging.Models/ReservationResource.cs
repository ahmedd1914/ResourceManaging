using System.ComponentModel.DataAnnotations;

namespace ResourceManaging.Models
{
    public class ReservationResource
    {
        [Required]
        public int ReservationId { get; set; }

        [Required]
        public int ResourceId { get; set; }
    }
}