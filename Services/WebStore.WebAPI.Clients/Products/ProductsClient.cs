using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Products;

public class ProductsClient : BaseClient, IProductData
{
    private readonly ILogger<ProductsClient> _logger;

    public ProductsClient(HttpClient client, ILogger<ProductsClient>? logger) : base(client, WebApiAddresses.Products)
    {
        _logger = logger;
    }

    public IEnumerable<Section> GetSections()
    {
        var sections = Get<IEnumerable<SectionDTO>>($"{Address}/sections");
        return sections.FromDTO() ?? Enumerable.Empty<Section>();
    }

    public async Task<IEnumerable<Section>> GetSectionsAsync()
    {
        var sections = await GetAsync<IEnumerable<SectionDTO>>($"{Address}/sections");
        //TODO проверить какая строка отреагирует верно.
        //return sections ?? Task.FromResult(Enumerable.Empty<Section>());
        return sections is null
            ? Enumerable.Empty<Section>() 
            : sections.FromDTO();
    }

    public Section? GetSectionById(int id)
    {
        var section = Get<SectionDTO>($"{Address}/section/{id}");
        return section.FromDTO();
    }

    public async Task<Section?> GetSectionByIdAsync(int id)
    {
        var section = await GetAsync<SectionDTO>($"{Address}/section/{id}");
        return section.FromDTO();
    }

    public IEnumerable<Brand> GetBrands()
    {
        var brands = Get<IEnumerable<BrandDTO>>($"{Address}/brands");
        return brands.FromDTO() ?? Enumerable.Empty<Brand>();
    }

    public async Task<IEnumerable<Brand>> GetBrandsAsync()
    {
        var brands = await GetAsync<IEnumerable<BrandDTO>>($"{Address}/brands");
        return brands.FromDTO() ?? Enumerable.Empty<Brand>();
    }

    public Brand? GetBrandById(int id)
    {
        var brand = Get<BrandDTO>($"{Address}/brand/{id}");
        return brand.FromDTO();
    }

    public async Task<Brand?> GetBrandByIdAsync(int id)
    {
        var brand = await GetAsync<BrandDTO?>($"{Address}/brand/{id}");
        return brand.FromDTO();
    }

    public IEnumerable<Product> GetProducts(ProductFilter? filter = null)
    {
        var response = Post<ProductFilter>($"{Address}/products", filter ?? new ProductFilter());
        var products = response.Content.ReadFromJsonAsync<IEnumerable<ProductDTO>>().Result;
        return products.FromDTO() ?? Enumerable.Empty<Product>();
    }

    public async Task<IEnumerable<Product>> GetProductsAsync(ProductFilter? filter = null)
    {
        var response = await PostAsync<ProductFilter>($"{Address}/products", filter ?? new());
        var products = response.Content.ReadFromJsonAsync<IEnumerable<ProductDTO>>().Result;
        return products.FromDTO();
    }

    public Product? GetProductById(int id)
    {
        var product = Get<ProductDTO>($"{Address}/product/{id}");
        return product.FromDTO();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        var product = await GetAsync<ProductDTO?>($"{Address}/product/{id}");
        return product.FromDTO();
    }
}