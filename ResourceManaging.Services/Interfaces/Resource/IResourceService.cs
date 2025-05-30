using ResourceManaging.Services.DTOs.Resource;
using ResourceManaging.Repository.Interfaces.Resource;

namespace ResourceManaging.Services.Interfaces.Resource
{
    public interface IResourceService
    {
        /// <summary>
        /// Gets a resource by its ID
        /// </summary>
        /// <param name="id">Resource ID</param>
        /// <returns>Resource data</returns>
        Task<ResourceResponse> GetResourceByIdAsync(int id);

        /// <summary>
        /// Gets resources based on filter criteria
        /// </summary>
        /// <param name="filter">Filter criteria for resources</param>
        /// <returns>List of resources matching the filter</returns>
        Task<ResourceListResponse> GetResourcesByFilterAsync(ResourceFilter filter);

        /// <summary>
        /// Gets resources by type
        /// </summary>
        /// <param name="resourceTypeId">Resource type ID</param>
        /// <returns>List of resources of the specified type</returns>
        Task<ResourceListResponse> GetResourcesByTypeAsync(int resourceTypeId);

        /// <summary>
        /// Creates a new resource
        /// </summary>
        /// <param name="request">Resource creation request</param>
        /// <returns>Created resource data</returns>
        Task<ResourceResponse> CreateResourceAsync(CreateResourceRequest request);

        /// <summary>
        /// Updates an existing resource
        /// </summary>
        /// <param name="request">Update data containing resource ID and fields to update</param>
        /// <returns>True if update was successful</returns>
        Task<bool> UpdateResourceAsync(UpdateResourceRequest request);

        /// <summary>
        /// Activates or deactivates a resource
        /// </summary>
        /// <param name="request">Request containing resource ID and new active status</param>
        /// <returns>True if update was successful</returns>
        Task<bool> SetResourceStatusAsync(SetResourceStatusRequest request);

        /// <summary>
        /// Deletes a resource
        /// </summary>
        /// <param name="id">Resource ID</param>
        /// <returns>True if deletion was successful</returns>
        Task<bool> DeleteResourceAsync(int id);
    }
}
