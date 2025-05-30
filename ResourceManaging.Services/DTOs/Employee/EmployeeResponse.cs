namespace ResourceManaging.Services.DTOs.Employee
{
    public class EmployeeResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public EmployeeData Employee { get; set; }
    }
} 