using System.Net.Http.Json;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Orders;

public class OrdersClient : BaseClient, IOrderService
{
    public OrdersClient(HttpClient client) : base(client, WebApiAddresses.V1.Orders)
    {
    }

    public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userName, CancellationToken token = default)
    {
        //var response = await PostAsync($"{Address}/orders", userName).ConfigureAwait(false);
        //var orders = response
        //    .EnsureSuccessStatusCode()
        //    .Content
        //    .ReadFromJsonAsync<IEnumerable<OrderDTO>>(cancellationToken:token)
        //    .Result;
        //return orders.FromDTO();
        var orders = await GetAsync<IEnumerable<OrderDto>>($"{Address}/user/{userName}", token).ConfigureAwait(false);
        return orders!.FromDTO()!;
    }

    public async Task<Order?> GetOrderByIdAsync(int id, CancellationToken token = default)
    {
        var order = await GetAsync<OrderDto>($"{Address}/{id}", token).ConfigureAwait(false);
        return order.FromDTO();
    }

    public async Task<Order> CreateOrderAsync(string userName, CartViewModel cartViewModel, OrderViewModel orderViewModel,
        CancellationToken token = default)
    {
        var createOrder = new CreateOrderDTO
        {
            UserName = userName,
            Items = cartViewModel.ToDTO(),
            Order = orderViewModel,
        };
        var response = await PostAsync($"{Address}/create", createOrder, token).ConfigureAwait(false);
        var order = await response
            .EnsureSuccessStatusCode()
            .Content
            .ReadFromJsonAsync<OrderDto>(cancellationToken: token)
            .ConfigureAwait(false);
        return order!.FromDTO()!;
    }
}