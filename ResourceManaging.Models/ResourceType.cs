using System.ComponentModel.DataAnnotations;

namespace ResourceManaging.Models
{
    public class ResourceType
    {
        
        public int ResourceTypeId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
        public string Name { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 255 characters")]
        public string Description { get; set; }

       
    }
}