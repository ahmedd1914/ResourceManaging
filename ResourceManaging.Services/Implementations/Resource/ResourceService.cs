using ResourceManaging.Services.Interfaces.Resource;
using ResourceManaging.Services.DTOs.Resource;
using ResourceManaging.Repository.Interfaces.Resource;
using ResourceManaging.Models;

namespace ResourceManaging.Services.Implementations.Resource
{
    public class ResourceService : IResourceService
    {
        private readonly IResourceRepository _resourceRepository;

        public ResourceService(IResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        public async Task<ResourceResponse> GetResourceByIdAsync(int id)
        {
            var resource = await _resourceRepository.RetrieveByIdAsync(id);
            if (resource == null)
            {
                return new ResourceResponse
                {
                    Success = false,
                    Message = "Resource not found"
                };
            }

            return new ResourceResponse
            {
                Success = true,
                Message = "Resource retrieved successfully",
                Resource = ResourceData.FromResource(resource)
            };
        }

        public async Task<ResourceListResponse> GetResourcesByFilterAsync(ResourceFilter filter)
        {
            var resources = await _resourceRepository.RetrieveByFilterAsync(filter);
            return new ResourceListResponse
            {
                Success = true,
                Message = "Resources retrieved successfully",
                Resources = resources.Select(ResourceData.FromResource).ToList(),
                TotalCount = resources.Count()
            };
        }

        public async Task<ResourceResponse> CreateResourceAsync(CreateResourceRequest request)
        {
            var resource = new Models.Resource
            {
                ResourceTypeId = request.ResourceTypeId,
                Name = request.Name,
                Capacity = request.Capacity,
                IsActive = true
            };

            var id = await _resourceRepository.CreateAsync(resource);
            if (id <= 0)
            {
                return new ResourceResponse
                {
                    Success = false,
                    Message = "Failed to create resource"
                };
            }

            resource.ResourceId = id;
            return new ResourceResponse
            {
                Success = true,
                Message = "Resource created successfully",
                Resource = ResourceData.FromResource(resource)
            };
        }

        public async Task<bool> UpdateResourceAsync(UpdateResourceRequest request)
        {
            var resource = await _resourceRepository.RetrieveByIdAsync(request.ResourceId);
            if (resource == null)
            {
                return false;
            }

            var update = new ResourceUpdate
            {
                ResourceId = request.ResourceId
            };
            update.UpdateName(request.Name);
            update.UpdateCapacity(request.Capacity);
            update.UpdateResourceTypeId(request.ResourceTypeId);
            update.UpdateActiveStatus(request.IsActive);

            return await _resourceRepository.UpdateAsync(update);
        }

        public async Task<bool> SetResourceStatusAsync(SetResourceStatusRequest request)
        {
            var resource = await _resourceRepository.RetrieveByIdAsync(request.ResourceId);
            if (resource == null)
            {
                return false;
            }

            var update = new ResourceUpdate();
            update.UpdateActiveStatus(request.IsActive);

            return await _resourceRepository.UpdateAsync(update);
        }

        public async Task<bool> DeleteResourceAsync(int id)
        {
            return await _resourceRepository.DeleteAsync(id);
        }

        public async Task<ResourceListResponse> GetResourcesByTypeAsync(int resourceTypeId)
        {
            var filter = new ResourceFilter();
            filter.AddResourceTypeFilter(resourceTypeId);
            return await GetResourcesByFilterAsync(filter);
        }
    }
}
