using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using WebStore.Domain;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

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

    public IActionResult Details(int id)
    {
        var product = _productData.GetProductById(id);
        if(product is null)
            return NotFound();
        return View(product.ToView());
    }
}