namespace Moto.DB.Models.Products
{
    public abstract class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public float Cost { get; set; }
        public float? Rating { get; set; }
        public string? ImageFileName { get; set; }
    }
}
