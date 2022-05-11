using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.DTO;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers;

[ApiController]
[Route(WebApiAddresses.V1.Orders)]
public class OrderApiController: ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrderApiController> _logger;

    public OrderApiController(IOrderService orderService, ILogger<OrderApiController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    [HttpPost("GetOrdersByUserName")]
    public async Task<IActionResult> GetUserOrdersAsync([FromBody] string userName)
    {
        var orders = await _orderService.GetUserOrdersAsync(userName);
        return Ok(orders.ToDTO());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrderByIdAsync(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order is null)
            return NotFound();

        return Ok(order.ToDTO());
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderDTO order)
    {
        var cartVM = order.Items.ToCartVM();
        var OrderVM = order.Order;
        var newOrder = await _orderService.CreateOrderAsync(order.UserName,cartVM, OrderVM);
        return Ok(newOrder.ToDTO());
    }
}