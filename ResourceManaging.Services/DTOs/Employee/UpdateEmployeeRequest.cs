using System.ComponentModel.DataAnnotations;

namespace ResourceManaging.Services.DTOs.Employee
{
    public class UpdateEmployeeRequest
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
    }
} 