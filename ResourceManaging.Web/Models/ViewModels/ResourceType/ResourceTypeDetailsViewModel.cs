namespace ResourceManaging.Web.Models
{
    public class ResourceTypeDetailsViewModel
    {
        public int ResourceTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ResourceInfo> Resources { get; set; } = new List<ResourceInfo>();
    }
} 