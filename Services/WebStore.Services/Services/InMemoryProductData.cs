using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Data;

namespace WebStore.Services.Services;

[Obsolete("Будет удаён в ближайшее время", true)]
public class InMemoryProductData : IProductData
{
    public IEnumerable<Section> GetSections() => TestData.Sections;
    public Task<IEnumerable<Section>> GetSectionsAsync()
    {
        throw new NotImplementedException();
    }

    public Section? GetSectionById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Section?> GetSectionByIdAsync(int id)
    {
        throw new NotImplementedException();
    }


    public IEnumerable<Brand> GetBrands() => TestData.Brands;
    public Task<IEnumerable<Brand>> GetBrandsAsync()
    {
        throw new NotImplementedException();
    }

    public Brand? GetBrandById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Brand?> GetBrandByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Product> GetProducts(ProductFilter? filter = null)
    {
        IEnumerable<Product> query = TestData.Products;
        if (filter is { SectionId: { } }) 
            query = query.Where(x => x.SectionId == filter.SectionId);
        
        //if (filter != null && filter.SectionId != null)
        //{
        //    query = query.Where(x => x.SectionId == filter.SectionId);
        //}

        if(filter?.BrandId is { } brandId)
            query = query.Where(x => x.BrandId == brandId);

        return query;
    }

    public Task<IEnumerable<Product>> GetProductsAsync(ProductFilter? filter = null)
    {
        throw new NotImplementedException();
    }

    public Product? GetProductById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Product?> GetProductByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}