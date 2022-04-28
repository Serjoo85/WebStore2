using System.Diagnostics.CodeAnalysis;
using WebStore.Domain.Entities;

namespace WebStore.Domain.DTO;

public class ProductDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    public int SectionId { get; set; }
    public SectionDTO Section { get; set; } = null!;
    public BrandDTO? Brand { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public decimal Price { get; set; }
}

public class SectionDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    public int? ParentId { get; set; }
}

public class BrandDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }

}

public static class BrandDTOMapper
{
    [return:NotNullIfNotNull("brand")]
    public static BrandDTO? ToDTO(this Brand? brand) => brand is null
    ? null
    : new BrandDTO
        {
            Id = brand.Id,
            Name = brand.Name,
            Order = brand.Order,
        };
    [return: NotNullIfNotNull("brand")]

    public static Brand? FromDTO(this BrandDTO? dto) => dto is null
        ? null
        : new Brand
        {
            Id = dto.Id,
            Order = dto.Order,
            Name = dto.Name,
        };

    public static IEnumerable<BrandDTO>? ToDTO(this IEnumerable<Brand>? brands) => 
        brands?.Select(b => b.ToDTO());

    public static IEnumerable<Brand>? FromDTO(this IEnumerable<BrandDTO>? dtos) => 
        dtos?.Select(d => d.FromDTO());
}

public static class SectionDTOMapper
{
    [return: NotNullIfNotNull("section")]

    public static SectionDTO? ToDTO(this Section? section) => section is null
        ? null
        : new SectionDTO
        {
            Id = section.Id,
            Name = section.Name,
            Order = section.Order,
        };

    [return: NotNullIfNotNull("section")]
    public static Section? FromDTO(this SectionDTO? dto) => dto is null
        ? null
        : new Section
        {
            Id = dto.Id,
            Order = dto.Order,
            Name = dto.Name,
        };

    public static IEnumerable<SectionDTO> ToDTO(this IEnumerable<Section>? sections) =>
        sections?.Select(b => b.ToDTO());

    public static IEnumerable<Section> FromDTO(this IEnumerable<SectionDTO>? dtos) =>
        dtos?.Select(d => d.FromDTO());
}

public static class ProductDTOMapper
{
    [return: NotNullIfNotNull("product")]

    public static ProductDTO? ToDTO(this Product? product) => product is null
        ? null
        : new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            ImageUrl = product.ImageUrl,
            Brand = product.Brand.ToDTO(),
            Order = product.Order,
            Section = product.Section.ToDTO(),
            Price = product.Price,
        };

    [return: NotNullIfNotNull("product")]
    public static Product? FromDTO(this ProductDTO? dto) => dto is null
        ? null
        : new Product
        {
            Id = dto.Id,
            Name = dto.Name,
            ImageUrl = dto.ImageUrl,
            Brand = dto.Brand.FromDTO(),
            Order = dto.Order,
            Section = dto.Section.FromDTO(),
            Price = dto.Price,
        };

    public static IEnumerable<ProductDTO> ToDTO(this IEnumerable<Product>? products) =>
        products?.Select(p => p.ToDTO());

    public static IEnumerable<Product> FromDTO(this IEnumerable<ProductDTO>? dtos) =>
        dtos?.Select(p => p.FromDTO());
}