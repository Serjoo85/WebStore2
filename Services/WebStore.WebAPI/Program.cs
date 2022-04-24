using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;
using WebStore.Services.Services;
using WebStore.Services.Services.InSQL;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

var dbConnectionStringName = configuration["Database"];
var dbConnectionString = configuration.GetConnectionString(dbConnectionStringName);

switch (dbConnectionStringName)
{
    case "SqlServer":
    case "DockerDb":
        services.AddDbContext<WebStoreDb>(opt => opt.UseSqlServer(dbConnectionString));
        break;
    case "Sqlite":
        services.AddDbContext<WebStoreDb>(opt => opt.UseSqlite(dbConnectionString, o => o.MigrationsAssembly("WebStore.DAL.Sqlite")));
        break;
}

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

// AddAsync services to the container.
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
