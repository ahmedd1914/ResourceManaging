using ResourceManaging.Services.DTOs.Employee;
using ResourceManaging.Repository.Interfaces.Employee;

namespace ResourceManaging.Services.Interfaces.Employee
{
    public interface IEmployeeService
    {
        /// <summary>
        /// Gets an employee by their ID
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Employee response with employee data</returns>
        Task<EmployeeResponse> GetEmployeeByIdAsync(int id);

        /// <summary>
        /// Gets employees based on filter criteria
        /// </summary>
        /// <param name="filter">Filter criteria for employees</param>
        /// <returns>List of employees matching the filter</returns>
        Task<EmployeeListResponse> GetEmployeesByFilterAsync(EmployeeFilter filter);

        /// <summary>
        /// Creates a new employee
        /// </summary>
        /// <param name="request">Employee creation request</param>
        /// <returns>Created employee data</returns>
        Task<EmployeeResponse> CreateEmployeeAsync(CreateEmployeeRequest request);

        /// <summary>
        /// Updates an existing employee
        /// </summary>
        /// <param name="request">Update request containing employee ID and fields to update</param>
        /// <returns>True if update was successful</returns>
        Task<UpdateEmployeeResponse> UpdateEmployeeAsync(UpdateEmployeeRequest request);

        /// <summary>
        /// Deletes an employee
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>True if deletion was successful</returns>
        Task<bool> DeleteEmployeeAsync(int id);
    }
}
