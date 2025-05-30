namespace ResourceManaging.Services.DTOs.Resource
{
    public class ResourceTypeResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ResourceTypeData ResourceType { get; set; }
    }
} 