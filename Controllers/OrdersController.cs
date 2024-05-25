using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moto.Controllers.DTO;
using Moto.DB.Models;
using Moto.Services.Interfaces;

namespace Moto.Controllers
{
    [Route("order")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private IOrdersService ordersService;
        private UserManager<User> userManager;
        private ICartItemsService cartItemsService;
        public OrdersController(IOrdersService ordersService, UserManager<User> userManager, ICartItemsService cartItemsService) 
        {
            this.ordersService = ordersService;
            this.userManager = userManager;
            this.cartItemsService = cartItemsService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            if (user is not null)
            {
                return Ok(await ordersService.GetUsersOrders(user.Id));
            }

            return BadRequest();
        }

        [HttpGet("items/{id}")]
        public async Task<IActionResult> GetOrderItems(Guid id)
        {
            var items = await ordersService.GetOrdersItems(id);
            return Ok(items.Select(i=>i.Item));
        }

        [HttpPost("")]
        public async Task<IActionResult> Create()
        {
            var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            if (user is not null)
            {
                var success = await ordersService.Create(user);
                return success?Ok():BadRequest();
            }

            return Unauthorized();
        }

        [HttpGet("itemsimages/{id}")]
        public async Task<IActionResult> GetOrderItemsImages(Guid id)
        {
            var items = await ordersService.GetOrdersItems(id);
            if (items != null)
            {
                var resultData = new List<string?>();
                foreach (var item in items)
                {
                    var product = await cartItemsService.GetProductById(item.Item.ProductId);

                    if (product == null)
                        return NotFound();

                    resultData.Add(product.ImageFileName);
                }
                return Ok(resultData);
            }

            return NotFound();
        }
    }
}
