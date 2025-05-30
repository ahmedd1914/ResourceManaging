using System.ComponentModel.DataAnnotations;

namespace ResourceManaging.Models
{
    public class Resource
    {
        public int ResourceId { get; set; }

        [Required]
        public int ResourceTypeId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
        public string Name { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be at least 1")]
        public int Capacity { get; set; }

        [Required]
        public bool IsActive { get; set; }
      
    }
}