using WebStore.Infrastructure.Middleware;
using WebStore.Services;
using WebStore.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllersWithViews();

services.AddScoped<IEmployeesData, InMemoryEmployeesData>();
services.AddScoped<IProductData, InMemoryProductData>();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapDefaultControllerRoute();

//app.UseMiddleware<TestMiddleware>();

app.MapControllerRoute(
    name: "ActionRoute",
    pattern: "{controller} {action}({a}, {b})");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
