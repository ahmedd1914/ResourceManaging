using ResourceManaging.Repository.Helpers;

namespace ResourceManaging.Repository.Interfaces.Resource
{
    public class ResourceFilter : Filter
    {
        public bool? IsActive { get; set; }
        public int? ResourceTypeId { get; set; }

        public void AddResourceTypeFilter(int resourceTypeId)
        {
            AddParameter("ResourceTypeId", resourceTypeId);
        }

        public void AddNameFilter(string name)
        {
            AddParameter("Name", name);
        }

        public void AddCapacityFilter(int capacity)
        {
            AddParameter("Capacity", capacity);
        }

        public void AddActiveStatusFilter(bool isActive)
        {
            AddParameter("IsActive", isActive);
        }
    }
} 