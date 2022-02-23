using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return Content("Hellow from controller!");
        }

        public IActionResult ContentString(string Id = "-id-")
        {
            return Content($"content: {Id}");
        }

        public IActionResult ConfigString()
        {
            return Content($"config: {_configuration["ServerGreetings"]}");
        }
    }
}
