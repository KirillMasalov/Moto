namespace Moto.DB.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Order Order { get; set; }
        public CartItem Item { get; set; }
    }
}
