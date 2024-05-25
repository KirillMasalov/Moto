namespace Moto.Controllers.DTO
{
    public class MotorcyclePostData : ProductPostData
    {
        public string Brand { get; set; }
        public int EnginePower { get; set; }
        public int EngineCapacity { get; set; }
    }
}
