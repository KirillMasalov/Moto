using Moto.Controllers.DTO;
using Moto.DB.Models;
using Moto.DB.Models.Products;
using System;

namespace Moto.Services.Interfaces
{
    public interface ICartItemsService
    {
        public Task<IEnumerable<CartItem>> GetAllByUserId(string userId);
        public Task<CartItem> GetById(Guid id);
        public Task<Guid> Create(CartItemData data, Guid userId);
        public Task<bool> Update(Guid id, int count);
        public Task<bool> Delete(Guid id);
        public Task<Product> GetProductById(Guid id);
    }
}
