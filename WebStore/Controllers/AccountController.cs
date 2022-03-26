using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers;

public class AccountController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}