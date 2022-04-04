using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Data;

namespace WebStore.Services.InSQL;

public class DbInitializer : IDbInitializer
{
    private readonly WebStoreDb _db;
    private readonly ILogger<DbInitializer> _logger;

    public DbInitializer(WebStoreDb db, ILogger<DbInitializer> logger)
    {
        _db = db;
        _logger = logger;
    }
    public async Task<bool> RemoveAsync(CancellationToken cancel = default)
    {
        _logger.LogInformation("Удаление БД...");

        var removed = await _db.Database.EnsureDeletedAsync(cancel).ConfigureAwait(false);

        _logger.LogInformation(removed 
            ? "БД удалена успешно." 
            : "Удаление БД не требуется (отсутствует на сервере).");

        return removed;
    }

    public async Task InitializeAsync(bool removeBefore = false, CancellationToken cancel = default)
    {
        _logger.LogInformation("Инициализация БД ...");

        if (removeBefore)
            await RemoveAsync(cancel).ConfigureAwait(false);

        // Команда для создания БД.
        //await _db.Database.EnsureCreatedAsync(cancel).ConfigureAwait(false);
        var pending_migrations = await _db.Database.GetPendingMigrationsAsync(cancel).ConfigureAwait(false);
        if (pending_migrations.Any())
        {
            _logger.LogInformation("Выполнено миграции БД ...");
            await _db.Database.MigrateAsync(cancel).ConfigureAwait(false);
            _logger.LogInformation("Выполнено миграции БД завершено.");
        }
        else
        {
            _logger.LogInformation("Миграция БД не требуется.");
        }

        await InitializeProductAsync(cancel).ConfigureAwait(false);
        await InitializeEmployeeAsync(cancel).ConfigureAwait(false);

        _logger.LogInformation("Выполнено успешно");
    }

    private async Task InitializeEmployeeAsync(CancellationToken cancel)
    {
        if (await _db.Employees.AnyAsync(cancel).ConfigureAwait(false))
        {
            _logger.LogInformation("Инициализация тестовых данных по сотрудникам в БД не требуется.");
            return;
        }

        _logger.LogInformation("Инициализация тестовых данных по сотрудникам в БД ...");

        await using (var transaction = await _db.Database.BeginTransactionAsync(cancel))
        {
            _logger.LogInformation("Добавление сотрудников...");
            await _db.AddRangeAsync(TestData.__Employees).ConfigureAwait(false);
            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Employees] ON", cancel).ConfigureAwait(false);
            await _db.SaveChangesAsync(cancel).ConfigureAwait(false);
            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Employees] OFF", cancel).ConfigureAwait(false);
            await transaction.CommitAsync(cancel).ConfigureAwait(false);
            _logger.LogInformation("Сотрудники успешно добавлены.");
        }
    }

    private async Task InitializeProductAsync(CancellationToken cancel)
    {
        if (await _db.Products.AnyAsync(cancel).ConfigureAwait(false))
        {
            _logger.LogInformation("Инициализация тестовых данных по продуктам в БД не требуется.");
            return;
        }

        _logger.LogInformation("Инициализация тестовых данных по продуктам в БД...");

        var sectionsPool = TestData.Sections.ToDictionary(s => s.Id);
        var brandPool = TestData.Brands.ToDictionary(b => b.Id);

        foreach ( var childSection in TestData.Sections.Where(s => s.ParentId is not null))
            childSection.Parent = sectionsPool[(int)childSection.ParentId!];
            
        foreach (var product in TestData.Products)
        {
            product.Section = sectionsPool[product.SectionId];
            if(product.BrandId is { } brandId)
                product.Brand = brandPool[brandId];
            product.Id = 0;
            product.SectionId = 0;
            product.BrandId = null;
        }

        foreach(var brand in TestData.Brands)
            brand.Id = 0;
        foreach(var section in TestData.Sections)
        {
            section.Id = 0;
            section.ParentId = null;
        }

        _logger.LogInformation("Добавление данных в БД...");
        await using (var transaction = await _db.Database.BeginTransactionAsync(cancel))
        {
            await _db.Sections.AddRangeAsync(TestData.Sections, cancel);
            await _db.Brands.AddRangeAsync(TestData.Brands, cancel);
            await _db.Products.AddRangeAsync(TestData.Products, cancel);
            
            await _db.SaveChangesAsync(cancel);

            await transaction.CommitAsync(cancel).ConfigureAwait(false);
        }
        _logger.LogInformation("Инициализация тестовых данных в БД успешно выполнена.");
    }
}