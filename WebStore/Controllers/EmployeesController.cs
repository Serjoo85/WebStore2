using Microsoft.AspNetCore.Mvc;
using WebStore.Models;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _employeesData;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IEmployeesData employeesData, ILogger<EmployeesController> logger)
        {
            _employeesData = employeesData;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var employees = _employeesData.GetAll();
            return View(employees);
        }
        
        //[Route("~/employees/info-{Id:int}")]
        public IActionResult Details(int Id)
        {
            var employee = _employeesData.GetById(Id);
            if(employee == null)
                return NotFound();

            return View(employee);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = _employeesData.GetById(id);
            if (employee is null)
                return NotFound();

            var model = new EmployeesViewModel()
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
                Position = employee.Position,
                Salary = employee.Salary
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EmployeesViewModel model)
        {
            var employee = new Employee()
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Patronymic = model.Patronymic,
                Age = model.Age,
                Position = model.Position,
                Salary = model.Salary
            };
            _employeesData.Edit(employee);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            return View();
        }
    }
}
