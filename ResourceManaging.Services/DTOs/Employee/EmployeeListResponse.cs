using ResourceManaging.Models;

namespace ResourceManaging.Services.DTOs.Employee
{
    public class EmployeeListResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<EmployeeData> Employees { get; set; }
        public int TotalCount { get; set; }
    }
} 