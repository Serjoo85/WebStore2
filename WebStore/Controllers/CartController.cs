using Microsoft.AspNetCore.Mvc;
using WebStore.Services.Interfaces;

namespace WebStore.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService) => _cartService = cartService;


    public IActionResult Index() => View(_cartService.GetViewModel());

}