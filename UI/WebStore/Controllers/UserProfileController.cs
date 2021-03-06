using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers;

[Authorize]
public class UserProfileController : Controller
{
    public IActionResult Index() => View();

    public async Task<ActionResult> Orders([FromServices] IOrderService orderService)
    {
        var orders = await orderService.GetUserOrdersAsync(User.Identity!.Name!);

        return View(orders.Select(order => new UserOrderViewModel
        {
            Id = order.Id,
            Address = order.Address,
            Phone = order.Phone,
            Description = order.Description,
            TotalPrice = order.TotalPrice,
            Date = order.Date,

        }));
    }
}
