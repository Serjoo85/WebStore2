using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.ViewModels;

namespace WebStore.Domain.DTO
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Description { get; set; }
        public DateTimeOffset Date { get; set; }
        public IEnumerable<OrderItemDTO> Items { get; set; }

    }

    public class OrderItemDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateOrderDTO
    {
        public string UserName { get; set; } = null!;
        public OrderViewModel Order { get; set; } = null!;
        public IEnumerable<OrderItemDTO> Items { get; set; } = null!;
    }

    public static class OrderDTOMapper
    {
        public static OrderDto? ToDTO(this Order? order) => order is null 
        ? null
        : new OrderDto
        {
            Address = order.Address,
            Description = order.Description,
            Date = order.Date,
            Id = order.Id,
            Items = order.Items.ToDTO()!,
            Phone = order.Phone,
        };

        public static IEnumerable<OrderDto?> ToDTO(this IEnumerable<Order?> orders) => orders.Select(ToDTO);


        public static Order? FromDTO(this OrderDto? orderDTO) => orderDTO is null
        ? null
        : new Order
        {
            Address = orderDTO.Address,
            Date = orderDTO.Date,
            Description = orderDTO.Description,
            Id = orderDTO.Id,
            Items = orderDTO.Items.FromDTO()!.ToList()!,
            Phone = orderDTO.Phone,
        };

        public static IEnumerable<Order?> FromDTO(this IEnumerable<OrderDto?> orders) => orders.Select(FromDTO);

        public static IEnumerable<OrderItemDTO> ToDTO(this CartViewModel cartViewModel) => cartViewModel.Items
            .Select(i => new OrderItemDTO()
            {
                ProductId = i.Product.Id,
                Price = i.Product.Price,
                Quantity = i.Quantity,

            });
            
        public static CartViewModel ToCartVM(this IEnumerable<OrderItemDTO> items) => new CartViewModel()
        {
            Items = items.Select(i => (new ProductViewModel(){Id = i.ProductId,Price = i.Price}, i.Quantity))
        };
    }

    public static class OrderItemDTOMapper
    {
        public static OrderItemDTO? ToDTO(this OrderItem? orderItem) =>  orderItem is null
        ? null
        : new OrderItemDTO
        {
            Id = orderItem.Id,
            Price = orderItem.Price,
            ProductId = orderItem.Product.Id,
            Quantity = orderItem.Quantity,
        };

        public static IEnumerable<OrderItemDTO?> ToDTO(this IEnumerable<OrderItem?> orderItems) => orderItems.Select(ToDTO);

        public static OrderItem? FromDTO(this OrderItemDTO? orderItemDTO) => orderItemDTO is null
        ? null
        : new OrderItem
        {
            Id = orderItemDTO.Id,
            Price = orderItemDTO.Price,
            Product = new Product{Id = orderItemDTO.Id},
            Quantity = orderItemDTO.Quantity,
        };

        public static IEnumerable<OrderItem?> FromDTO(this IEnumerable<OrderItemDTO?> orderItemDTO) => orderItemDTO.Select(FromDTO);
    }

    //public class CartDTO
    //{
    //    public IEnumerable<(ProductDTO? Product, int Quantity)> Items { get; set; }
    //    public int Quantity { get; set; }
    //    public int TotalPrice { get; set; }
    //}

    //public class OrderDTO
    //{
    //    public int Id { get; set; }
    //    public string Phone { get; set; }
    //    public string Address { get; set; }
    //    public string Description { get; set; }
    //}


    //public class OrderDTO
    //{
    //    public string Address { get; set; } = null!;
    //    public string Phone { get; set; } = null!;
    //    public string? Description { get; set; }
    //}

    //public class CartOrderDTO
    //{
    //    public CartDTO Cart { get; set; }
    //    public OrderDTO Order { get; set; }
    //}

    //public static class CartDTOMapping
    //{
    //    public static CartDTO? ToDTO(this CartViewModel cart) => cart is null
    //        ? null
    //        : new CartDTO
    //        {
    //            Items = cart.Items.Select(i => i.Product.)
    //        };
    //}

    //public class OrderDTOMapping
    //{

    //}

    //public class CartOrderDTOMapping
    //{
}
