using System.ComponentModel.DataAnnotations;

namespace ResourceManaging.Services.DTOs.Resource
{
    public class SetResourceStatusRequest
    {
        [Required]
        public int ResourceId { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
} 