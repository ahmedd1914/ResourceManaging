using System;
using System.ComponentModel.DataAnnotations;

namespace ResourceManaging.Web.Models.ViewModels.Employee
{
    public class EditEmployeeViewModel
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
