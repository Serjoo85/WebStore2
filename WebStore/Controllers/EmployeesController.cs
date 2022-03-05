using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        private static readonly List<Employee> __Employees = new List<Employee>
        {
            new Employee { Id =1, Age = 21, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Position = "Менеджер читсоты", Salary = 10000},
            new Employee { Id =2, Age = 22, LastName = "Пётров", FirstName = "Пётр", Patronymic = "Петрович", Position = "Менеджер читсоты", Salary = 10000},
            new Employee { Id =3, Age = 23, LastName = "Сидоров", FirstName = "Сидр", Patronymic = "Сидорович", Position = "Менеджер читсоты", Salary = 10000},
        };

        public IActionResult Index()
        {
            return View(__Employees);
        }

        public IActionResult Detailes(int Id)
        {
            var employee = __Employees.FirstOrDefault(e => e.Id == Id);
            if(employee == null)
                return NotFound();
            return View(employee);
        }
    }
}
