using WebStore.Domain.Entities.Orders;
using WebStore.Domain.ViewModels;

namespace WebStore.Interfaces.Services;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetUserOrdersAsync(string UserName, CancellationToken token = default);
    Task<Order?> GetOrderByIdAsync(int id, CancellationToken token = default);
    Task<Order> CreateOrderAsync(string userName, CartViewModel cart, OrderViewModel orderViewModel, CancellationToken token = default);

}