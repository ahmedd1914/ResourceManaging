using ResourceManaging.Repository.Helpers;

namespace ResourceManaging.Repository.Interfaces.Resource
{
    public class ResourceUpdate : Update
    {
        private int _resourceId;
        public int ResourceId 
        { 
            get => _resourceId;
            set
            {
                _resourceId = value;
                AddUpdate("ResourceId", value);
            }
        }

        public void UpdateName(string name)
        {
            AddUpdate("Name", name);
        }

        public void UpdateCapacity(int capacity)
        {
            AddUpdate("Capacity", capacity);
        }

        public void UpdateResourceTypeId(int resourceTypeId)
        {
            AddUpdate("ResourceTypeId", resourceTypeId);
        }

        public void UpdateActiveStatus(bool isActive)
        {
            AddUpdate("IsActive", isActive);
        }
    }
} 