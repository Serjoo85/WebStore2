using Microsoft.AspNetCore.Mvc;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Components;

public class BrandsViewComponent : ViewComponent
{
    private readonly IProductData _productData;

    public BrandsViewComponent(IProductData productData)
    {
        _productData = productData;
    }
    public IViewComponentResult Invoke()
    {
        var brands = _productData.GetBrands();

        var brandViewModels = brands.Select(b => new BrandViewModel()
            {
                Id = b.Id,
                Name = b.Name,
            })
            .ToList();


        return View(brandViewModels);
    }
}