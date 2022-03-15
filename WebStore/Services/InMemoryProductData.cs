using WebStore.Data;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services;

public class InMemoryProductData : IProductData
{
    public IEnumerable<Section> GetSections() => TestData.Sections;

    public IEnumerable<Brand> GetBrands() => TestData.Brands;
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
}