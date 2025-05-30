using System.ComponentModel.DataAnnotations;
using ResourceManaging.Web.Models.ViewModels.Resource;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ResourceManaging.Web.Models
{
    public class EditResourceViewModel
    {
        public int ResourceId { get; set; }

        [Required(ErrorMessage = "Resource type is required")]
        [Display(Name = "Resource Type")]
        public int ResourceTypeId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
        public string Name { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be at least 1")]
        public int Capacity { get; set; }

        public bool IsActive { get; set; }

        [ValidateNever]
        public List<ResourceTypeInfo> ResourceTypes { get; set; }
    }
} 