namespace ResourceManaging.Web.Models
{
    public class ResourceInfo
    {
        public int ResourceId { get; set; }
        public string Name { get; set; }
        public int ResourceTypeId { get; set; }
        public int Capacity { get; set; }
        public bool IsActive { get; set; }
        public string ResourceTypeName { get; set; }
    }
} 