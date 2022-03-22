using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebStore.Domain.Entities;

namespace WebStore.DAL.Context;

public class WebStoreDb : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Employee> Employees { get; set; }

    public WebStoreDb(DbContextOptions<WebStoreDb> options) : base(options)
    {
        
    }

    // Пример fluentApi для настройки базы даных.
    //protected override void OnModelCreating(ModelBuilder model)
    //{
    //    base.OnModelCreating(model);
    //    model.Entity<Brand>()
    //        .HasMany(brand => brand.Products)
    //        .WithOne(product => product.Brand)
    //        .OnDelete(DeleteBehavior.Cascade);
    //}
}
