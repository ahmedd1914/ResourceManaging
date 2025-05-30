using System.ComponentModel.DataAnnotations;

namespace ResourceManaging.Services.DTOs.Resource
{
    public class CreateResourceRequest
    {
        [Required]
        public int ResourceTypeId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
        public string Name { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than 0")]
        public int Capacity { get; set; }

        public bool IsActive { get; set; } = true;
    }
} 