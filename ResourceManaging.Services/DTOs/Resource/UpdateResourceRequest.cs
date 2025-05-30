using System.ComponentModel.DataAnnotations;

namespace ResourceManaging.Services.DTOs.Resource
{
    public class UpdateResourceRequest
    {
        [Required]
        public int ResourceId { get; set; }

        [Required]
        public int ResourceTypeId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Capacity { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
} 