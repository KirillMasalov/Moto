namespace Moto.DB.Models.Products
{
    public class Motorcycle: Product
    {
        public string Brand { get; set; }
        public int EnginePower { get; set; }
        public int EngineCapacity { get; set; }
    }
}
