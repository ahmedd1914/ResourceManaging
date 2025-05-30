using ResourceManaging.Repository.Helpers;

namespace ResourceManaging.Repository.Interfaces.Resource
{
    public class ResourceTypeFilter : Filter
    {
        public void AddNameFilter(string name)
        {
            AddParameter("Name", name);
        }

        public void AddDescriptionFilter(string description)
        {
            AddParameter("Description", description);
        }
    }
} 