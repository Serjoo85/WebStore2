using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;

namespace WebStore.Areas.Admin.Controllers;

[Authorize(Roles = Role.Administrators)]
public class ProductsController : Controller
{
    private readonly IProductData _productData;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductData productData, ILogger<ProductsController> logger)
    {
        _productData = productData;
        _logger = logger;
    }

    public IActionResult Index()
    {
        var products = _productData.GetProducts();
        return View(products);
    }
}