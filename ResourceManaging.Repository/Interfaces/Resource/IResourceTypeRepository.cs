using ResourceManaging.Repository.Base;
using ResourceManaging.Repository.Implementation.Resources;

namespace ResourceManaging.Repository.Interfaces.Resource
{
    public interface IResourceTypeRepository : IBaseRepository<Models.ResourceType, ResourceTypeFilter, ResourceTypeUpdate>
    {
         
    }
}