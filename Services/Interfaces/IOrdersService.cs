using Moto.DB.Models;
using Moto.DB.Models.Products;

namespace Moto.Services.Interfaces
{
    public interface IOrdersService
    {
        public Task<IEnumerable<Order>> GetUsersOrders(string userId);
        public Task<IEnumerable<OrderItem>> GetOrdersItems(Guid orderId);

        public Task<bool> Create(User user);
    }
}
