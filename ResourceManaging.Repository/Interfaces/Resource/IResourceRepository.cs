using ResourceManaging.Repository.Base;
using ResourceManaging.Repository.Implementation.Resources;

namespace ResourceManaging.Repository.Interfaces.Resource
{
    public interface IResourceRepository : IBaseRepository<Models.Resource, ResourceFilter, ResourceUpdate>
    {
         
    }
}