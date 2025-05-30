using ResourceManaging.Repository.Helpers;

namespace ResourceManaging.Repository.Interfaces.Employee
{
    public class EmployeeUpdate : Update
    {
        private int _employeeId;
        public int EmployeeId 
        { 
            get => _employeeId;
            set
            {
                _employeeId = value;
                AddUpdate("EmployeeId", value);
            }
        }

        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public void UpdateEmail(string email)
        {
            AddUpdate("Email", email);
        }

        public void UpdateFullName(string fullName)
        {
            AddUpdate("FullName", fullName);
        }

        public void UpdateDateOfBirth(DateTime dateOfBirth)
        {
            AddUpdate("DateOfBirth", dateOfBirth);
        }
    }
} 