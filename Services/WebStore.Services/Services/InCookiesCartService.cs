using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Services.Services;

public class InCookiesCartService : ICartService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProductData _productData;
    private readonly string _cartName;

    private Cart Cart
    {
        get
        {
            var context = _httpContextAccessor.HttpContext;
            var cookies = context!.Response.Cookies;

            var cartCookies = context!.Request.Cookies[_cartName];

            if (cartCookies is null)
            {
                var cart = new Cart();
                cookies.Append(_cartName, JsonConvert.SerializeObject(cart));
                return cart;
            }

            ReplaceCart(cookies, cartCookies);
            return JsonConvert.DeserializeObject<Cart>(cartCookies)!;
        }
        set => ReplaceCart(_httpContextAccessor.HttpContext!.Response.Cookies, JsonConvert.SerializeObject(value));
    }


    private void ReplaceCart(IResponseCookies cookies, string cart)
    {
        cookies.Delete(_cartName);
        cookies.Append(_cartName, cart);
    }

    public InCookiesCartService(IHttpContextAccessor httpContextAccessor, IProductData productData)
    {
        _httpContextAccessor = httpContextAccessor;
        _productData = productData;

        var user = _httpContextAccessor.HttpContext!.User;
        var userName = user.Identity!.IsAuthenticated ? $"-{user.Identity.Name}" : null;
        _cartName = $"WebStore.Gb.Cart{userName}";
    }

    public void Add(int id)
    {
        var cart = Cart;

        var item = cart.Items.FirstOrDefault(item => item.ProductId == id);
        if (item is null)
            cart.Items.Add(new() { ProductId = id });
        else
            item.Quantity++;
        Cart = cart;
    }

    public void Decrement(int id)
    {
        var cart = Cart;

        var item = cart.Items.FirstOrDefault(item => item.ProductId == id);
        if (item is null)
            return;

        if (item.Quantity > 0)
            item.Quantity--;

        if(item.Quantity <= 0)
            cart.Items.Remove(item);

        Cart = cart;
    }

    public void Remove(int id)
    {
        var cart = Cart;

        var item = cart.Items.FirstOrDefault(item => item.ProductId == id);
        if(item is null)
            return;

        cart.Items.Remove(item);

        Cart = cart;
    }

    public void Clear()
    {
        // Cart = new() - иначе больше переписывать.
        var cart = Cart;

        cart.Items.Clear();

        Cart = cart;
    }

    public CartViewModel GetViewModel()
    {
        var cart = Cart;
        var products = _productData.GetProducts(new()
        {
            Ids = cart.Items.Select(item => item.ProductId).ToArray()
        });

        var productsView = products.ToView().ToDictionary(p => p!.Id);
        return new()
        {
            Items = cart.Items
                .Where(item => productsView.ContainsKey(item.ProductId))
                .Select(item => (productsView[item.ProductId], item.Quantity))
        };
    }
}