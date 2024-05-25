using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moto.Controllers.DTO;
using Moto.Services.Interfaces;

namespace Moto.Controllers
{
    [Route("admin/")]
    [Authorize(Policy = "AdminRole")]
    public class AdminController: Controller
    {
        private IMotorcyclesService motorcyclesService;
        public AdminController(IMotorcyclesService motoService) 
        {
            motorcyclesService = motoService;
        }

        //[HttpGet("/")]
        //public async Task<IActionResult> CheckAdminPanel()
        //{
        //    return Ok();
        //}

        //[HttpGet("/motorcycles")]
        //public async Task<IActionResult> GetMotorcyclesList()
        //{
        //    return Ok(await motorcyclesService.GetAll());
        //}

        [HttpDelete("/motorcycles/{id}")]
        public async Task<IActionResult> DeleteMotorcycle(Guid id)
        {
            var success = await motorcyclesService.DeleteById(id);

            return success?Ok():NotFound();
        }

        [HttpPut("/motorcycles/{id}")]
        public async Task<IActionResult> ChangeMotorcycle(Guid id, [FromForm] MotorcyclePostData changeData)
        {
            var success = await motorcyclesService.UpdateById(id, changeData);

            return success ? Ok() : NotFound();
        }


        [HttpPost("/motorcycles")]
        public async Task<IActionResult> CreateMotorcycle([FromBody] MotorcyclePostData createData)
        {
            await motorcyclesService.Create(createData);
            return Ok();
        }
    }
}
