using Microsoft.EntityFrameworkCore;
using Moto.Controllers.DTO;
using Moto.DB;
using Moto.DB.Models;
using Moto.DB.Models.Products;
using Moto.Services.Interfaces;

namespace Moto.Services
{
    public class CartService: ICartItemsService
    {
        private AppDbContext dbContext;
        private IMotorcyclesService motorcyclesService;
        public CartService(AppDbContext dbContext, IMotorcyclesService motorcyclesService)
        {
            this.dbContext = dbContext;
            this.motorcyclesService = motorcyclesService;
        }

        public async Task<Guid> Create(CartItemData data, Guid userId)
        {
            var newItem = new CartItem();
            newItem.UserId = userId;
            newItem.ProductId = data.ProductId;
            newItem.Count = data.Count;

            await dbContext.CartItems.AddAsync(newItem);
            await dbContext.SaveChangesAsync();
            return newItem.Id;
        }

        public async Task<IEnumerable<CartItem>> GetAllByUserId(string userId)
        {
           return await dbContext.CartItems.Where(i => i.UserId.ToString() == userId && !i.IsOrdered).ToListAsync<CartItem>();
        }

        public async Task<bool> Update(Guid id, int count)
        {
            var item = await dbContext.FindAsync<CartItem>(id);
            if (item != null)
            { 
                item.Count = count;
                await dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var item = await dbContext.FindAsync<CartItem>(id);
            if (item is not null)
            {
                dbContext.CartItems.Remove(item);
                await dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<CartItem> GetById(Guid id)
        {
            var item = await dbContext.CartItems.FindAsync(id);
            return item;
        }

        public async Task<Product> GetProductById(Guid productId)
        {
            Product product = null;
            product ??= await motorcyclesService.GetById(productId);

            return product;
        }
    }
}
