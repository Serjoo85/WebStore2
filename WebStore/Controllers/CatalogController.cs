using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using WebStore.Domain;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers;

public class CatalogController : Controller
{
    private readonly IProductData _productData;

    public CatalogController(IProductData productData)
    {
        _productData = productData;
    }

    public IActionResult Index(int? sectionId, int? brandId)
    {
        var filter = new ProductFilter()
        {
            BrandId = brandId,
            SectionId = sectionId,
        };

        var products = _productData.GetProducts(filter);
        return View(new CatalogViewModel
        {
            SectionId = sectionId,
            BrandId = brandId,
            Products = products
                .OrderBy(p => p.Order)
                .Select(p => new ProductViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        ImageUrl = p.ImageUrl,
                        Order = p.Order,
                        SectionId = p.SectionId,
                        BrandId = p.BrandId
                    }),
        });
    }
}