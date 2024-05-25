namespace Moto.DB.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? RecievedDate { get; set; }
        public OrderStatus Status { get; set; }
    }
    
    public enum OrderStatus
    {
        Creating,
        Sended,
        Recieved,
        Canceled,
        Retuned
    }
}
