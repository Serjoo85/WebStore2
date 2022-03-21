using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Infrastructure.Middleware;
using WebStore.Services;
using WebStore.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllersWithViews();

var configuration = builder.Configuration;
services.AddDbContext<WebStoreDb>(
    opt => opt.UseSqlServer(
        connectionString: configuration.GetConnectionString(
            "SqlServer")));

services.AddScoped<IEmployeesData, InMemoryEmployeesData>();
services.AddScoped<IProductData, InMemoryProductData>();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapDefaultControllerRoute();

app.UseMiddleware<TestMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
