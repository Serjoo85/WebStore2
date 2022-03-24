using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InSQL
{
    public class SqlProductData : IProductData
    {
        private readonly WebStoreDb _db;
        private readonly ILogger<SqlProductData> _logger;

        public SqlProductData(WebStoreDb db, ILogger<SqlProductData> logger)
        {
            _db = db;
            _logger = logger;
        }

        public IEnumerable<Section> GetSections() => _db.Sections;

        public IEnumerable<Brand> GetBrands() => _db.Brands;

        public IEnumerable<Product> GetProducts(ProductFilter? filter = null)
        {
            IQueryable<Product> query = _db.Products;

            if (filter is { SectionId: { } })
                query = query.Where(x => x.SectionId == filter.SectionId);

            if (filter?.BrandId is { } brandId)
                query = query.Where(x => x.BrandId == brandId);

            return query;
        }
    }
}
