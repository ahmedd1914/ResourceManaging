using ResourceManaging.Repository.Base;
using ResourceManaging.Repository.Implementation.Employees;

namespace ResourceManaging.Repository.Interfaces.Employee
{
    public interface IEmployeeRepository : IBaseRepository<Models.Employee, EmployeeFilter, EmployeeUpdate>
    {
         
    }
}
