using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SingleBlog()
        {
            var x = View();
            return x;
        }
    }
}
