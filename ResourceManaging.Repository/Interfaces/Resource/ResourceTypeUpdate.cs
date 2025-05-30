using ResourceManaging.Repository.Helpers;

namespace ResourceManaging.Repository.Interfaces.Resource
{
    public class ResourceTypeUpdate : Update
    {
        public int ResourceTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public void UpdateName(string name)
        {
            AddUpdate("Name", name);
        }

        public void UpdateDescription(string description)
        {
            AddUpdate("Description", description);
        }
    }
} 