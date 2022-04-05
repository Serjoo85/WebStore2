using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using WebStore.Domain;
using WebStore.Infrastructure.Mapping;
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
        var catalog = new CatalogViewModel
        {
            SectionId = sectionId,
            BrandId = brandId,
            Products = products
                .OrderBy(p => p.Order)
                .ToView()!
        };

        return View(catalog);
    }

    public IActionResult Details(int Id)
    {
        var product = _productData.GetProductById(Id);
        if(product is null)
            return NotFound();
        return View(product.ToView());
    }
}