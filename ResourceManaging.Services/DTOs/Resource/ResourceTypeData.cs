using ResourceManaging.Models;

namespace ResourceManaging.Services.DTOs.Resource
{
    public class ResourceTypeData
    {
        public int ResourceTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public static ResourceTypeData FromResourceType(Models.ResourceType resourceType)
        {
            return new ResourceTypeData
            {
                ResourceTypeId = resourceType.ResourceTypeId,
                Name = resourceType.Name,
                Description = resourceType.Description
            };
        }
    }
} 