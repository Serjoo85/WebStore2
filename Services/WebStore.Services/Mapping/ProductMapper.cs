using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;

namespace WebStore.Services.Mapping;

public static class ProductMapper
{
    public static ProductViewModel? ToView(this Product? product) => product is null
        ? null
        : new ProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            Section = product.Section.Name,
            Brand = product.Brand?.Name,
        };

    public static Product? FromView(this ProductViewModel? productVM) => productVM is null
        ? null
        : new Product
        {
            Id = productVM.Id,
            Name = productVM.Name,
            Price = productVM.Price,
            ImageUrl = productVM.ImageUrl,
        };

    public static IEnumerable<ProductViewModel?> ToView(this IEnumerable<Product?> products) => products.Select(ToView);

    public static IEnumerable<Product?> FromViewModel(this IEnumerable<ProductViewModel?> productViewModels) => productViewModels.Select(FromView);
}