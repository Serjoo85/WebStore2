using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
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
        public IActionResult Details(int id)
        {
            var employee = _employeesData.GetById(id);
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
            if (!ModelState.IsValid)
                return View(model);

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

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if(id <= 0)
                return BadRequest();

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
        public IActionResult DeleteConfirmed(int id)
        {
            if (!_employeesData.Delete(id))
                return NotFound();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeesViewModel model)
        {
            if (model.LastName == "Иванов" && model.Age < 20)
            {
                ModelState.AddModelError("", "Employee with last name Ivanov must be elder than 20!");
            }

            if (!ModelState.IsValid)
                return View(model);

            if(model is null)
                throw new ArgumentNullException(nameof(model));

            var employee = new Employee()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Patronymic = model.Patronymic,
                Age = model.Age,
                Position = model.Position,
                Salary = model.Salary
            };
            _employeesData.Add(employee);

            return RedirectToAction(nameof(Index));
        }
    }
}
