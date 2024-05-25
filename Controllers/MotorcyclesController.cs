using Microsoft.AspNetCore.Mvc;
using Moto.Controllers.DTO;
using Moto.DB.Models.Products;
using Moto.Services.Interfaces;

namespace Moto.Controllers
{
    [Route("motorcycles/")]
    public class MotorcyclesController: ControllerBase
    {
        private IMotorcyclesService motorcyclesService;
        public MotorcyclesController(IMotorcyclesService motoService) 
        {
            motorcyclesService = motoService;
        }

        [HttpGet("page/")]
        public async Task<IActionResult> GetPage(PageQueryParameters parameters)
        {
            return Ok(await motorcyclesService.GetByPage(parameters));
        }

        [HttpGet("all/")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await motorcyclesService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var motorcycle = await motorcyclesService.GetById(id);
            if (motorcycle is null)
                return NotFound();
            return Ok(motorcycle);
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCount(PageQueryParameters parameters)
        {
            return Ok(await motorcyclesService.GetCount(parameters));
        }
    }
}
