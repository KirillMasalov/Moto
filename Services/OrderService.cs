using Microsoft.EntityFrameworkCore;
using Moto.DB;
using Moto.DB.Models;
using Moto.DB.Models.Products;
using Moto.Services.Interfaces;

namespace Moto.Services
{
    public class OrderService:IOrdersService
    {
        private AppDbContext dbContext;
        private ICartItemsService cartItemsService;

        public OrderService(AppDbContext dbContext, ICartItemsService cartItemsService)
        {
            this.dbContext = dbContext;
            this.cartItemsService = cartItemsService;
        }

        public async Task<IEnumerable<Order>> GetUsersOrders(string userId)
        {
            var orders = dbContext.Orders.Where(o => o.User.Id == userId);
            return orders;
        }

        public async Task<IEnumerable<OrderItem>> GetOrdersItems(Guid orderId)
        {
            var orderItems = await dbContext.OrderItems.Where(o => o.Order.Id == orderId).ToListAsync();
            return orderItems;
        }

        public async Task<bool> Create(User user)
        {
            var userId = user.Id;
            var cartItemsId = await cartItemsService.GetAllByUserId(userId.ToString());
            if (cartItemsId == null || cartItemsId.Count() == 0)
            {
                return false;
            }
            var order = new Order();
            order.CreateDate = DateTime.Now;
            order.User = user;
            order.Status = OrderStatus.Creating;
            dbContext.Orders.Add(order);

            var cartItems = new List<CartItem>();

            foreach (var cartItemId in cartItemsId)
            {
                var cartItem = await cartItemsService.GetById(cartItemId.Id);
                if (cartItem != null)
                {
                    cartItems.Add(cartItem);

                    var orderItem = new OrderItem();
                    orderItem.Order = order;
                    orderItem.Item = cartItem;
                    await dbContext.OrderItems.AddAsync(orderItem);
                }
            }


            foreach (var item in cartItems)
            {
                item.IsOrdered = true;
            }

            await dbContext.SaveChangesAsync();

            return true;
        }

        
    }
}
