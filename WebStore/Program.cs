using WebStore.Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapDefaultControllerRoute();

app.UseMiddleware<TestMiddleware>();

app.MapControllerRoute(
    name: "ActionRoute",
    pattern: "{controller} {action}({a}, {b})");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
