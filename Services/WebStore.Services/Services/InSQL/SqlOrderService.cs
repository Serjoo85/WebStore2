using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services.InSQL;

public class SqlOrderService : IOrderService
{
    private readonly WebStoreDb _db;
    private readonly UserManager<User> _userManger;
    private readonly ILogger<SqlOrderService> _logger;

    public SqlOrderService(WebStoreDb db, UserManager<User> userManger, ILogger<SqlOrderService> logger)
    {
        _db = db;
        _userManger = userManger;
        _logger = logger;
    }

    public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userName, CancellationToken token = default)
    {
        var orders = await _db.Orders
            .Include(o => o.User)
            .Include(o => o.Items)
            .ThenInclude(item => item.Product)
            .Where(o => o.User.UserName == userName)
            .ToArrayAsync(token)
            .ConfigureAwait(false);

        return orders;
    }

    public async Task<Order?> GetOrderByIdAsync(int id, CancellationToken token = default)
    {
        var order = await _db.Orders
            .Include(o => o.User)
            .Include(o => o.Items)
            .ThenInclude(item => item.Product)
            .FirstOrDefaultAsync(o => o.Id == id, token)
            .ConfigureAwait(false);

        return order;
    }

    public async Task<Order> CreateOrderAsync(
        string userName,
        CartViewModel cart,
        OrderViewModel orderViewModel,
        CancellationToken token = default)
    {
        var user = await _userManger.FindByNameAsync(userName).ConfigureAwait(false);
        if (user is null)
            throw new InvalidOperationException($"Пользователя с именем {userName} в системе нет.");

        await using var transaction = await _db.Database.BeginTransactionAsync(token).ConfigureAwait(false);
        
        var newOrder = new Order
        {
            User = user,
            Address = orderViewModel.Address,
            Phone = orderViewModel.Phone,
            Description = orderViewModel.Description,
        };

        var productsIds = cart.Items.Select(i => i.Product!.Id).ToArray();

        var cartProducts = await _db.Products
                .Where(i => productsIds
                .Contains(i.Id))
                .ToArrayAsync(token)
                .ConfigureAwait(false);

        newOrder.Items = cart.Items.Join(
            cartProducts,
            cartItem => cartItem.Product!.Id,
            cartProduct => cartProduct.Id,
            (cartItem, cartProduct) => new OrderItem
            {
                Order = newOrder,
                Product = cartProduct,
                Price = cartProduct.Price,
                Quantity = cartItem.Quantity,

            }).ToArray();

        await _db.Orders.AddAsync(newOrder, token).ConfigureAwait(false);
        await _db.SaveChangesAsync(token).ConfigureAwait(false);
        await transaction.CommitAsync(token).ConfigureAwait(false);

        return newOrder;
    }
}