using Microsoft.AspNetCore.Mvc;
using WebStore.Infrastructure.Mapping;
using WebStore.Services.Interfaces;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        //private readonly IProductData _productData;

        public HomeController(IConfiguration configuration, IProductData productData)
        {
            _configuration = configuration;
            //_productData = productData;
        }

        public IActionResult Index([FromServices] IProductData productData)
        {
            var products = productData.GetProducts();
            var productViewModels = products
                .OrderBy(p => p.Order)
                .Take(6)
                .ToView();

            ViewBag.Products = productViewModels;
            return View();
            //return View(productViewModels);
        }
    }
}
