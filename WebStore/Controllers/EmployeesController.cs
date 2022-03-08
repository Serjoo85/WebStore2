using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {


        public IActionResult Index()
        {
            return View(__Employees);
        }
        
        //[Route("~/employees/info-{Id:int}")]
        public IActionResult Details(int Id)
        {
            var employee = __Employees.FirstOrDefault(e => e.Id == Id);
            if(employee == null)
                return NotFound();
            return View(employee);
        }
    }
}
