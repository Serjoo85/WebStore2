using WebStore.Data;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services;

[Obsolete("Будет удаён в ближайшее время", true)]
public class InMemoryProductData : IProductData
{
    public IEnumerable<Section> GetSections() => TestData.Sections;
    public Section? GetSectionById(int id)
    {
        throw new NotImplementedException();
    }


    public IEnumerable<Brand> GetBrands() => TestData.Brands;
    public Brand? GetBrandById(int id)
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

    public Product? GetProductById(int id)
    {
        throw new NotImplementedException();
    }
}