using Microsoft.AspNetCore.Mvc;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

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
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    Order = p.Order,
                    SectionId = p.SectionId,
                    BrandId = p.BrandId
                });

            ViewBag.Products = productViewModels;
            return View();
            //return View(productViewModels);
        }
    }
}
