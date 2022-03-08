using Microsoft.AspNetCore.Mvc;
using WebStore.Models;
using WebStore.Services.Interfaces;

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
    }
}
