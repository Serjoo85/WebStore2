using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services.InSQL
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

        public Section? GetSectionById(int id) => _db.Sections
            .Include(s => s.Products)
            .FirstOrDefault(s => s.Id == id);

        public IEnumerable<Brand> GetBrands() => _db.Brands;

        public Brand? GetBrandById(int id) => _db.Brands
            .Include(b => b.Products)
            .FirstOrDefault(b => b.Id == id);
        

        public IEnumerable<Product> GetProducts(ProductFilter? filter = null)
        {
            IQueryable<Product> query = _db.Products
                .Include(p => p.Section)
                .Include(p => p.Brand);

            if (filter?.Ids?.Length > 0)
            {
                query = query.Where(p => filter.Ids.Contains(p.Id));
            }
            else
            {
                if (filter is { SectionId: { } })
                    query = query.Where(x => x.SectionId == filter.SectionId);

                if (filter?.BrandId is { } brandId)
                    query = query.Where(x => x.BrandId == brandId);
            }
            return query;
        }

        public Product? GetProductById(int id) => _db.Products
            .Include(p => p.Section)
            .Include(p => p.Brand)
            .FirstOrDefault(p => p.Id == id);


    }
}
