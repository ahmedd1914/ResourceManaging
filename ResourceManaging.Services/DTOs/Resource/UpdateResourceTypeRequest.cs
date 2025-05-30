using System.ComponentModel.DataAnnotations;

namespace ResourceManaging.Services.DTOs.Resource
{
    public class UpdateResourceTypeRequest
    {
        [Required]
        public int ResourceTypeId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }
} 