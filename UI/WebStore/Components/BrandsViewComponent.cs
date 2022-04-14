using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

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
                Order = b.Order,
            })
            .ToList();

            brandViewModels.Sort((a,b) => Comparer<int>.Default.Compare(a.Order,b.Order));

        return View(brandViewModels);
    }
}