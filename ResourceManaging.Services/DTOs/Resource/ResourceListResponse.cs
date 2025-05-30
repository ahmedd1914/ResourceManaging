namespace ResourceManaging.Services.DTOs.Resource
{
    public class ResourceListResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<ResourceData> Resources { get; set; }
        public int TotalCount { get; set; }
    }
} 