using Moto.DB.Models.Products;

namespace Moto.DB.Models
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public int Count { get; set; }
        public bool IsOrdered { get; set; } = false;
    }
}
