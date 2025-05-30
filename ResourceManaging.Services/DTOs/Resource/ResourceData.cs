using ResourceManaging.Models;

namespace ResourceManaging.Services.DTOs.Resource
{
    public class ResourceData
    {
        public int ResourceId { get; set; }
        public int ResourceTypeId { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public bool IsActive { get; set; }

        public static ResourceData FromResource(Models.Resource resource)
        {
            return new ResourceData
            {
                ResourceId = resource.ResourceId,
                ResourceTypeId = resource.ResourceTypeId,
                Name = resource.Name,
                Capacity = resource.Capacity,
                IsActive = resource.IsActive
            };
        }
    }
} 