using Moto.DB.Models.Products;

namespace Moto.Controllers.DTO
{
    public class CartItemResult
    {
        public Guid Id { get; set; }
        public Product Product { get; set; }
        public int Count {  get; set; }
    }
}
