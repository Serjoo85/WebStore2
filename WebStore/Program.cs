using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Infrastructure.Middleware;
using WebStore.Services;
using WebStore.Services.InSQL;
using WebStore.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllersWithViews();

var configuration = builder.Configuration;

services.AddDbContext<WebStoreDb>(
    opt => opt.UseSqlServer(
        connectionString: configuration.GetConnectionString(
            "SqlServer")));
services.AddTransient<IEmployeesData, InMemoryEmployeesData>();

//services.AddScoped<IProductData, InMemoryProductData>();
services.AddScoped<IProductData, SqlProductData>();
services.AddScoped<IDbInitializer, DbInitializer>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    await dbInitializer.InitializeAsync(true);
}

app.UseStaticFiles();

app.UseRouting();

app.MapDefaultControllerRoute();

app.UseMiddleware<TestMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
