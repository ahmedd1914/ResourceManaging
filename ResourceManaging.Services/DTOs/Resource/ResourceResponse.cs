namespace ResourceManaging.Services.DTOs.Resource
{
    public class ResourceResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ResourceData Resource { get; set; }
    }
} 