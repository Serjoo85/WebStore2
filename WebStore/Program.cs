using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Middleware;
using WebStore.Services;
using WebStore.Services.InSQL;
using WebStore.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllersWithViews(opt =>
{
    opt.Conventions.Add(new AddAreasControllerRoute());
});

services.AddControllersWithViews();

var configuration = builder.Configuration;
var dbConnectionStringName = configuration["Database"];
var dbConnectionString = configuration.GetConnectionString(dbConnectionStringName);
services.AddDbContext<WebStoreDb>(opt => opt.UseSqlServer(dbConnectionString));

services.AddTransient<IDbInitializer, DbInitializer>();

services.AddIdentity<User, Role>(/*opt => opt*/)
    .AddEntityFrameworkStores<WebStoreDb>()
    .AddDefaultTokenProviders();

// Настройки Identity
services.Configure<IdentityOptions>(opt =>
{
#if DEBUG
    opt.Password.RequireDigit = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequiredUniqueChars = 3;
    opt.Password.RequiredLength = 3;
#endif

    opt.User.RequireUniqueEmail = false;
    opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

    opt.Lockout.AllowedForNewUsers = false;
    opt.Lockout.MaxFailedAccessAttempts = 10;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
});

// Настройки Cookies
services.ConfigureApplicationCookie(opt =>
{
    opt.Cookie.Name = "WebStoreGb";
    opt.Cookie.HttpOnly = true;

    opt.ExpireTimeSpan = TimeSpan.FromDays(10);

    opt.LoginPath = "/Account/Login";
    opt.LogoutPath = "/Account/Logout";
    opt.AccessDeniedPath = "/Account/AccessDenied";

    opt.SlidingExpiration = true;
});

services.AddScoped<IEmployeesData, SqlEmployeeData>();
services.AddScoped<IProductData, SqlProductData>();
services.AddScoped<ICartService, InCookiesCartService>();
services.AddScoped<IOrderService, SqlOrderService>();
//services.AddAutoMapper(Assembly.GetEntryAssembly());
services.AddAutoMapper(typeof(Program));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    await dbInitializer.InitializeAsync(true);
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<TestMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=home}/{action=Index}/{id?}"
        );
    endpoints.MapControllerRoute(
            name: "EmployeeDetails",
            pattern: "{controller=Employee}/{action=Index}/{id?}"
        );
    endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}"
        );
});

app.Run();