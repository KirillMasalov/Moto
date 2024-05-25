using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moto.Controllers.DTO;
using Moto.DB.Models;
using Moto.Services;
using Moto.Services.Interfaces;

namespace Moto.Controllers
{
    [Route("cart")]
    [Authorize]
    public class CartController: ControllerBase
    {
        private ICartItemsService cartService;
        private UserManager<User> userManager;
        private IMotorcyclesService motoService;
        public CartController(ICartItemsService cs, UserManager<User> userManager, IMotorcyclesService motoService) 
        {
            cartService = cs;
            this.userManager = userManager;
            this.motoService = motoService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetCart()
        {
            var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            if (user is not null)
            {
                var cartItems = await cartService.GetAllByUserId(user.Id);
                var resultData = new List<CartItemResult>();
                foreach (var item in cartItems)
                {
                    var product = await cartService.GetProductById(item.ProductId);

                    if (product == null)
                        return NotFound();

                    resultData.Add(new CartItemResult() {Id=item.Id, Product = product, Count = item.Count }); 
                }
                return Ok(resultData);
            }
            else
                return BadRequest();
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm] CartItemData itemData)
        {
            var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            if (user is not null)
            {
                var itemId = await cartService.Create(itemData, new Guid(user.Id));
                if (itemId != null)
                    return Ok(itemId);
                else
                    return BadRequest();
            }
            else
                return BadRequest();
        }

        [HttpPut("{id}/{count}")]
        public async Task<IActionResult> UpdateItem(Guid id, int count)
        {
            if(count <= 0)
                return BadRequest();
            var success = await cartService.Update(id, count);

            return success?Ok():BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            var success = await cartService.Delete(id);
            return success ? Ok() : BadRequest();
        }
    }
}
