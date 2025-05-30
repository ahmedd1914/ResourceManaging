using ResourceManaging.Services.Interfaces.Resource;
using ResourceManaging.Services.DTOs.Resource;
using ResourceManaging.Repository.Interfaces.Resource;
using ResourceManaging.Models;

namespace ResourceManaging.Services.Implementations.Resource
{
    public class ResourceTypeService : IResourceTypeService
    {
        private readonly IResourceTypeRepository _resourceTypeRepository;

        public ResourceTypeService(IResourceTypeRepository resourceTypeRepository)
        {
            _resourceTypeRepository = resourceTypeRepository;
        }

        public async Task<ResourceTypeResponse> GetResourceTypeByIdAsync(int id)
        {
            var resourceType = await _resourceTypeRepository.RetrieveByIdAsync(id);
            if (resourceType == null)
            {
                return new ResourceTypeResponse
                {
                    Success = false,
                    Message = "Resource type not found"
                };
            }

            return new ResourceTypeResponse
            {
                Success = true,
                Message = "Resource type retrieved successfully",
                ResourceType = ResourceTypeData.FromResourceType(resourceType)
            };
        }

        public async Task<ResourceTypeListResponse> GetResourceTypesByFilterAsync(ResourceTypeFilter filter)
        {
            var resourceTypes = await _resourceTypeRepository.RetrieveByFilterAsync(filter);
            return new ResourceTypeListResponse
            {
                Success = true,
                Message = "Resource types retrieved successfully",
                ResourceTypes = resourceTypes.Select(ResourceTypeData.FromResourceType).ToList(),
                TotalCount = resourceTypes.Count()
            };
        }

        public async Task<ResourceTypeResponse> CreateResourceTypeAsync(CreateResourceTypeRequest request)
        {
            var resourceType = new Models.ResourceType
            {
                Name = request.Name,
                Description = request.Description
            };

            var id = await _resourceTypeRepository.CreateAsync(resourceType);
            if (id <= 0)
            {
                return new ResourceTypeResponse
                {
                    Success = false,
                    Message = "Failed to create resource type"
                };
            }

            resourceType.ResourceTypeId = id;
            return new ResourceTypeResponse
            {
                Success = true,
                Message = "Resource type created successfully",
                ResourceType = ResourceTypeData.FromResourceType(resourceType)
            };
        }

        public async Task<bool> UpdateResourceTypeAsync(UpdateResourceTypeRequest request)
        {
            var resourceType = await _resourceTypeRepository.RetrieveByIdAsync(request.ResourceTypeId);
            if (resourceType == null)
            {
                return false;
            }

            var update = new ResourceTypeUpdate();
            update.UpdateName(request.Name);
            update.UpdateDescription(request.Description);
            update.AddUpdate("ResourceTypeId", request.ResourceTypeId);

            return await _resourceTypeRepository.UpdateAsync(update);
        }

        public async Task<bool> DeleteResourceTypeAsync(int id)
        {
            return await _resourceTypeRepository.DeleteAsync(id);
        }
    }
}
