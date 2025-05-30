namespace ResourceManaging.Services.DTOs.Resource
{
    public class ResourceTypeListResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<ResourceTypeData> ResourceTypes { get; set; }
        public int TotalCount { get; set; }
    }
} 