using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;

namespace WebStore.Areas.Admin.Controllers;

//[Area("Admin")]
//[Authorize]
public class HomeController : Controller
{
    public IActionResult Index() => View();
}