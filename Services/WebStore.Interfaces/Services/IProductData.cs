using WebStore.Domain;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services;

public interface IProductData
{
    IEnumerable<Section> GetSections();
    Task<IEnumerable<Section>> GetSectionsAsync();
    Section? GetSectionById(int id);
    Task<Section?> GetSectionByIdAsync(int id);
    IEnumerable<Brand> GetBrands();
    Task<IEnumerable<Brand>> GetBrandsAsync();
    Brand? GetBrandById(int id);
    Task<Brand?> GetBrandByIdAsync(int id);
    IEnumerable<Product> GetProducts(ProductFilter? filter = null);
    Task<IEnumerable<Product>> GetProductsAsync(ProductFilter? filter = null);
    Product? GetProductById(int id);
    Task<Product?> GetProductByIdAsync(int id);
}