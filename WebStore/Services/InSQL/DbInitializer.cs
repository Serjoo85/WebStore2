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

        _logger.LogInformation("Выполнено успешно");
    }

    private async Task InitializeProductAsync(CancellationToken cancel)
    {
        if (await _db.Products.AnyAsync(cancel).ConfigureAwait(false))
        {
            _logger.LogInformation("Инициализация тестовых данных в БД не требуется.");
            return;
        }

        _logger.LogInformation("Инициализация тестовых данных в БД...");

        await using (var transaction = await _db.Database.BeginTransactionAsync(cancel))
        {
            _logger.LogInformation("Добавление секций...");
            await _db.Sections.AddRangeAsync(TestData.Sections, cancel).ConfigureAwait(false);
            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] ON", cancel).ConfigureAwait(false);
            await _db.SaveChangesAsync(cancel).ConfigureAwait(false);
            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] OFF", cancel).ConfigureAwait(false);
            await transaction.CommitAsync(cancel).ConfigureAwait(false);
            _logger.LogInformation("Секции успешно добавлены.");

        }

        await using (var transaction = await _db.Database.BeginTransactionAsync(cancel))
        {
            _logger.LogInformation("Добавление брендов...");
            await _db.Brands.AddRangeAsync(TestData.Brands, cancel).ConfigureAwait(false);
            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] ON", cancel).ConfigureAwait(false);
            await _db.SaveChangesAsync(cancel).ConfigureAwait(false);
            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] OFF", cancel).ConfigureAwait(false);
            await transaction.CommitAsync(cancel).ConfigureAwait(false);
            _logger.LogInformation("Бренды успешно добавлены.");
        }

        await using (var transaction = await _db.Database.BeginTransactionAsync(cancel))
        {
            _logger.LogInformation("Добавление продуктов...");
            await _db.Products.AddRangeAsync(TestData.Products, cancel).ConfigureAwait(false);
            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] ON", cancel).ConfigureAwait(false);
            await _db.SaveChangesAsync(cancel).ConfigureAwait(false);
            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] OFF", cancel).ConfigureAwait(false);
            await transaction.CommitAsync(cancel).ConfigureAwait(false);
            _logger.LogInformation("Продукты успешно добавлены.");
        }

        _logger.LogInformation("Инициализация тестовых данных в БД успешно выполнена.");
    }
}