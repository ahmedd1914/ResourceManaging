using ResourceManaging.Web.Models.ViewModels.Resource;
using System.ComponentModel.DataAnnotations;

namespace ResourceManaging.Web.Models.ViewModels.Resource
{
    public class ResourceDetailsViewModel
    {
        public int ResourceId { get; set; }
        
        public int ResourceTypeId { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public bool IsActive { get; set; }
        public ResourceTypeInfo ResourceType { get; set; }
    }
} 