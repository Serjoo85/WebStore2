using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers;

[ApiController]
[Route(WebApiAddresses.Products)]
public class ProductsApiController : ControllerBase
{
    private readonly IProductData _productData;
    private readonly ILogger<ProductsApiController> _logger;

    public ProductsApiController(IProductData productData, ILogger<ProductsApiController> logger)
    {
        _productData = productData;
        _logger = logger;
    }

    [HttpGet("sections")]  //GET -> http://localhost:5001/api/products/sections
    public IActionResult GetSections()
    {
        _logger.LogInformation("Запрос секций...");
        var sections = _productData.GetSections().ToDTO();

        if (!sections.Any())
        {
            _logger.LogInformation("Секции не найдены.");
            return NoContent();
        }
        _logger.LogInformation("Секции отправлены.");

        return Ok(sections);
    }

    [HttpGet("section/{id:int}")]
    public IActionResult GetSectionById(int id)
    {
        _logger.LogInformation("Запрос секции c Id:{0}.", id);

        var section = _productData.GetSectionById(id).ToDTO();
        if (section is null)
        {
            _logger.LogInformation("Cекция Id:{0} не найдена.", id);
            return NoContent();
        }
        _logger.LogInformation("Cекция Id:{0} отправлена.", id);

        return Ok(section);
    }

    [HttpGet("brands")]
    public IActionResult GetBrands()
    {
        _logger.LogInformation("Запрос брендов ...");
        var brands = _productData.GetBrands().ToDTO();
        if (!brands.Any())
        {
            _logger.LogInformation("Бренды не найдены.");
            return NoContent();
        }
        _logger.LogInformation("Бренды отправлены.");
        return Ok(brands);
    }

    [HttpGet("brand/{id:int}")]
    public IActionResult GetBrandById(int id)
    {
        _logger.LogInformation("Запрос бренда c Id:{0}.", id);
        var brand = _productData.GetBrandById(id).ToDTO();
        if (brand is null)
        {
            _logger.LogWarning("Бренд Id:{0} не найден.", id);
            return NoContent();
        }
        _logger.LogInformation("Бренд Id:{0} отправлен.", id);
        return Ok(brand);
    }

    [HttpPost("products")]
    public IActionResult GetProducts(ProductFilter? filter)
    {
        _logger.LogInformation("Запрос продуктов ...");
        var products = _productData.GetProducts(filter).ToDTO();
        if (!products.Any())
        {
            _logger.LogInformation("Продукты не найдены.");
            return NoContent();
        }
        _logger.LogInformation("Продукты отправлены.");
        return Ok(products);
    }

    [HttpPost("products/{id:int}")]
    public IActionResult GetProductsById(int id)
    {
        _logger.LogInformation("Запрос продукта c Id:{0}.", id);
        var products = _productData.GetProductById(id).ToDTO();
        if (products is null)
        {
            _logger.LogInformation("Продукт Id:{0} не найден.", id);
            return NoContent();
        }
        _logger.LogInformation("Продукт Id:{0} отправлен.", id);
        return Ok(products);
    }
}