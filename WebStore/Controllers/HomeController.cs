using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private static readonly List<Employee> __Employees = new List<Employee>
        {
            new Employee { Id =1, Age = 21, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Position = "Менеджер читсоты", Salary = 10000},
            new Employee { Id =2, Age = 22, LastName = "Пётров", FirstName = "Пётр", Patronymic = "Петрович", Position = "Менеджер читсоты", Salary = 10000},
            new Employee { Id =3, Age = 23, LastName = "Сидоров", FirstName = "Сидр", Patronymic = "Сидорович", Position = "Менеджер читсоты", Salary = 10000},
        };

        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Employees()
        {
            return View(__Employees);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PersonalCard(int id)
        {
            return View(__Employees[id - 1]);
        }
    }
}
