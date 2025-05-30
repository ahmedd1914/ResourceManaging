using ResourceManaging.Services.Interfaces.Employee;
using ResourceManaging.Services.DTOs.Employee;
using ResourceManaging.Repository.Interfaces.Employee;
using ResourceManaging.Models;
using ResourceManaging.Services.Helpers;

namespace ResourceManaging.Services.Implementations.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<EmployeeResponse> GetEmployeeByIdAsync(int id)
        {
            var employee = await _employeeRepository.RetrieveByIdAsync(id);
            if (employee == null)
            {
                return new EmployeeResponse
                {
                    Success = false,
                    Message = "Employee not found"
                };
            }

            return new EmployeeResponse
            {
                Success = true,
                Message = "Employee retrieved successfully",
                Employee = EmployeeData.FromEmployee(employee)
            };
        }

        public async Task<EmployeeListResponse> GetEmployeesByFilterAsync(EmployeeFilter filter)
        {
            var employees = await _employeeRepository.RetrieveByFilterAsync(filter);
            return new EmployeeListResponse
            {
                Success = true,
                Message = "Employees retrieved successfully",
                Employees = employees.Select(EmployeeData.FromEmployee).ToList(),
                TotalCount = employees.Count()
            };
        }

        public async Task<EmployeeResponse> CreateEmployeeAsync(CreateEmployeeRequest request)
        {
            var employee = new Models.Employee
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = SecurityHelper.HashPassword(request.Password),
                FullName = request.FullName,
                DateOfBirth = request.DateOfBirth
            };

            var id = await _employeeRepository.CreateAsync(employee);
            if (id <= 0)
            {
                return new EmployeeResponse
                {
                    Success = false,
                    Message = "Failed to create employee"
                };
            }

            employee.EmployeeId = id;
            return new EmployeeResponse
            {
                Success = true,
                Message = "Employee created successfully",
                Employee = EmployeeData.FromEmployee(employee)
            };
        }

        public async Task<UpdateEmployeeResponse> UpdateEmployeeAsync(UpdateEmployeeRequest request)
        {
            var employee = await _employeeRepository.RetrieveByIdAsync(request.EmployeeId);
            if (employee == null)
            {
                return new UpdateEmployeeResponse
                {
                    Success = false,
                    Message = "Employee not found"
                };
            }

            var update = new EmployeeUpdate
            {
                EmployeeId = request.EmployeeId
            };

            update.UpdateEmail(request.Email);
            update.UpdateFullName(request.FullName);
            update.UpdateDateOfBirth(request.DateOfBirth);

            var updateResult = await _employeeRepository.UpdateAsync(update);

            if (!updateResult)
            {
                return new UpdateEmployeeResponse
                {
                    Success = false,
                    Message = "Failed to update employee"
                };
            }

            var updatedEmployee = await _employeeRepository.RetrieveByIdAsync(request.EmployeeId);

            return new UpdateEmployeeResponse
            {
                Success = true,
                Message = "Employee updated successfully",
                Employee = EmployeeData.FromEmployee(updatedEmployee)
            };
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            return await _employeeRepository.DeleteAsync(id);
        }
    }
}
