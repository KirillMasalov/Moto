using Microsoft.AspNetCore.Mvc;

namespace Moto.Controllers.DTO
{
    public abstract class ProductPostData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public float? Rating { get; set; }

        [BindProperty(Name = "ImageFile")]
        public IFormFile? Image {get;set;}
    }
}
