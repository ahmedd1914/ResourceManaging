using System.ComponentModel.DataAnnotations;

namespace ResourceManaging.Services.DTOs.Resource
{
    public class CreateResourceTypeRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }
    }
} 