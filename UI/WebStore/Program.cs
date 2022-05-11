using Microsoft.AspNetCore.Identity;
using WebStore.Domain.Entities.Identity;
using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Middleware;
using WebStore.Interfaces.Services;
using WebStore.Services.Services;
using WebStore.WebAPI.Clients.Employees;
using WebStore.WebAPI.Clients.Identity;
using WebStore.WebAPI.Clients.Orders;
using WebStore.WebAPI.Clients.Products;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllersWithViews(opt =>
{
    opt.Conventions.Add(new AddAreasControllerRoute());
});

services.AddControllersWithViews();

var configuration = builder.Configuration;
services.AddIdentity<User, Role>(/*opt => opt*/)
    .AddDefaultTokenProviders();

services.AddHttpClient("WebStoreApiIdentity", client => client.BaseAddress = new(configuration["WebAPI"]))
    .AddTypedClient<IUserStore<User>, UsersClient>()
    .AddTypedClient<IUserRoleStore<User>, UsersClient>()
    .AddTypedClient<IUserPasswordStore<User>, UsersClient>()
    .AddTypedClient<IUserEmailStore<User>, UsersClient>()
    .AddTypedClient<IUserPhoneNumberStore<User>, UsersClient>()
    .AddTypedClient<IUserTwoFactorStore<User>, UsersClient>()
    .AddTypedClient<IUserClaimStore<User>, UsersClient>()
    .AddTypedClient<IUserLoginStore<User>, UsersClient>()
    .AddTypedClient<IRoleStore<Role>, RolesClient>()
    ;

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

services.AddScoped<ICartService, InCookiesCartService>();

services.AddHttpClient("WebStoreApi", client => client.BaseAddress = new(configuration["WebAPI"]))
    .AddTypedClient<IOrderService, OrdersClient>()
    .AddTypedClient<IEmployeesData, EmployeesClient>()
    .AddTypedClient<IProductData, ProductsClient>();
    

services.AddAutoMapper(typeof(Program));

var app = builder.Build();

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