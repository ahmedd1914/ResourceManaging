using Microsoft.AspNetCore.Mvc;
using ResourceManaging.Services.Interfaces.Employee;
using ResourceManaging.Services.DTOs.Employee;
using ResourceManaging.Repository.Interfaces.Employee;
using ResourceManaging.Web.Models.ViewModels.Employee;
using System.Linq;
using ResourceManaging.Web.Attributes;

namespace ResourceManaging.Web.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _employeeService.GetEmployeesByFilterAsync(new EmployeeFilter());
            var model = new EmployeeListViewModel
            {
                Employees = response.Employees?.Select(e => new EmployeeViewModel
                {
                    EmployeeId = e.EmployeeId,
                    FullName = e.FullName,
                    Username = e.Username,
                    Email = e.Email,
                    BirthDate = e.DateOfBirth
                }).ToList() ?? new List<EmployeeViewModel>(),
                TotalCount = response.TotalCount
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var response = await _employeeService.GetEmployeeByIdAsync(id);
            if (response.Success)
            {
                var model = new EmployeeViewModel
                {
                    EmployeeId = response.Employee.EmployeeId,
                    FullName = response.Employee.FullName,
                    Username = response.Employee.Username,
                    Email = response.Employee.Email,
                    BirthDate = response.Employee.DateOfBirth
                };
                return View(model);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateEmployeeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var request = new CreateEmployeeRequest
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
                FullName = model.FullName,
                DateOfBirth = model.DateOfBirth
            };
            var response = await _employeeService.CreateEmployeeAsync(request);
            if (response.Success)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", response.Message);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _employeeService.GetEmployeeByIdAsync(id);
            if (!response.Success) return NotFound();

            var model = new EditEmployeeViewModel
            {
                EmployeeId = response.Employee.EmployeeId,
                FullName = response.Employee.FullName,
                Email = response.Employee.Email,
                DateOfBirth = response.Employee.DateOfBirth
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var request = new UpdateEmployeeRequest
            {
                EmployeeId = model.EmployeeId,
                FullName = model.FullName,
                Email = model.Email,
                DateOfBirth = model.DateOfBirth
            };
            var response = await _employeeService.UpdateEmployeeAsync(request);
            if (response.Success)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", response.Message ?? "Update failed.");
            return View(model);
        }
    }
}
