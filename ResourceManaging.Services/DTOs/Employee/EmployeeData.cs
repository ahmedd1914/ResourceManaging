using ResourceManaging.Models;

namespace ResourceManaging.Services.DTOs.Employee
{
    public class EmployeeData
    {
        public int EmployeeId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public static EmployeeData FromEmployee(Models.Employee employee)
        {
            return new EmployeeData
            {
                EmployeeId = employee.EmployeeId,
                Username = employee.Username,
                Email = employee.Email,
                FullName = employee.FullName,
                DateOfBirth = employee.DateOfBirth
            };
        }
    }
} 