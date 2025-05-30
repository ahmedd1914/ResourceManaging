using ResourceManaging.Services.DTOs.Resource;
using ResourceManaging.Repository.Interfaces.Resource;

namespace ResourceManaging.Services.Interfaces.Resource
{
    public interface IResourceTypeService
    {
        /// <summary>
        /// Gets a resource type by its ID
        /// </summary>
        /// <param name="id">Resource type ID</param>
        /// <returns>Resource type data</returns>
        Task<ResourceTypeResponse> GetResourceTypeByIdAsync(int id);

        /// <summary>
        /// Gets resource types based on filter criteria
        /// </summary>
        /// <param name="filter">Filter criteria for resource types</param>
        /// <returns>List of resource types matching the filter</returns>
        Task<ResourceTypeListResponse> GetResourceTypesByFilterAsync(ResourceTypeFilter filter);

        /// <summary>
        /// Creates a new resource type
        /// </summary>
        /// <param name="request">Resource type creation request</param>
        /// <returns>Created resource type data</returns>
        Task<ResourceTypeResponse> CreateResourceTypeAsync(CreateResourceTypeRequest request);

        /// <summary>
        /// Updates an existing resource type
        /// </summary>
        /// <param name="request">Update data containing resource type ID and fields to update</param>
        /// <returns>True if update was successful</returns>
        Task<bool> UpdateResourceTypeAsync(UpdateResourceTypeRequest request);

        /// <summary>
        /// Deletes a resource type
        /// </summary>
        /// <param name="id">Resource type ID</param>
        /// <returns>True if deletion was successful</returns>
        Task<bool> DeleteResourceTypeAsync(int id);
    }
}
