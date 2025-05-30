using ResourceManaging.Services.Interfaces.Authentication;
using ResourceManaging.Services.DTOs.Authentication;
using ResourceManaging.Repository.Interfaces.Employee;
using ResourceManaging.Models;
using ResourceManaging.Services.Helpers;

namespace ResourceManaging.Services.Implementations.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public AuthenticationService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Username and password are required"
                };
            }

            var filter = new EmployeeFilter
            {
                Username = request.Username
            };

            var employees = await _employeeRepository.RetrieveByFilterAsync(filter);
            var employee = employees.FirstOrDefault();

            if (employee == null || SecurityHelper.HashPassword(request.Password) != employee.PasswordHash)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Invalid username or password"
                };
            }

            return new LoginResponse
            {
                Success = true,
                Message = "Login successful",
                Username = employee.Username,
                Email = employee.Email,
                FullName = employee.FullName,
                EmployeeId = employee.EmployeeId
            };
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var filter = new EmployeeFilter
            {
                Username = request.Username,
                Email = request.Email
            };

            var existingUsers = await _employeeRepository.RetrieveByFilterAsync(filter);
            if (existingUsers.Any())
            {
                return new RegisterResponse
                {
                    Success = false,
                    Message = "Username or email already exists"
                };
            }

            var employee = new ResourceManaging.Models.Employee
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = SecurityHelper.HashPassword(request.Password),
                FullName = request.FullName,
                DateOfBirth = request.DateOfBirth
            };

            var result = await _employeeRepository.CreateAsync(employee);
            return new RegisterResponse
            {
                Success = result > 0,
                Message = result > 0 ? "Registration successful" : "Registration failed",
                EmployeeId = result,
                Username = employee.Username
            };
        }
    }
}
