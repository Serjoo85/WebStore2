using Microsoft.Extensions.Configuration;
using WebStore.WebAPI.Clients.Products;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var http = new HttpClient
{
    BaseAddress = new(configuration["WebAPI"])
};

var productsClient = new ProductsClient(http, null);

Console.WriteLine("Ожидание запуска WebAPI");
Console.ReadLine();

foreach (var product in productsClient.GetProducts())
{
    Console.WriteLine("[{0}] {1}", product.Id, product.Name);
}

Console.ReadKey();

