using System.ComponentModel.DataAnnotations;

namespace ResourceManaging.Web.Models
{
    public class CreateResourceTypeViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 255 characters")]
        public string Description { get; set; }
    }
} 