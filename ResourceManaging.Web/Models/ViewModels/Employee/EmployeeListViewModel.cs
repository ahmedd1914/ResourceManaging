// Models/ViewModels/Employee/EmployeeListViewModel.cs
using System;
using System.Collections.Generic;
using ResourceManaging.Models;

namespace ResourceManaging.Web.Models.ViewModels.Employee
{
    public class EmployeeListViewModel
    {
        public List<EmployeeViewModel> Employees { get; set; }
        public int TotalCount { get; set; }
  }

    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }

    }
}
