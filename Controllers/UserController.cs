using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moto.Controllers.ControllersInputModels;
using Moto.DB;
using Moto.Services.Interfaces;
using System.Diagnostics;

namespace Moto.Controllers
{
    [Route("ur/")]
    public class UserController : Controller
    {
        private IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }


        //[HttpPost("create/")]
        //public async Task<IActionResult> Register(IFormCollection collection, [FromBody]UserCreateInputModel data)
        //{
        //    var exists = await userService.LoginExists(data);
        //    if (exists)
        //        return Conflict(new { Reason = "Login_exists" });
        //        //return new JsonResult(new { Status="Error", Message="Login_exists"});

        //    exists = await userService.EmailExists(data);
        //    if (exists)
        //        return Conflict(new { Reason = "Email_exists" });
        //    //return new JsonResult(new { Status = "Error", Message = "Email_exists" });

        //    var user = await userService.Create(data);
        //    Console.WriteLine("Register");
        //    if(user is null)
        //        return BadRequest();

        //    return Ok(user);
        //}

        //[Authorize]
        //[HttpPost("signin/")]
        //public async Task<IActionResult> SignIn([FromBody] UserLoginModel data)
        //{
        //    Console.WriteLine("Executed");
        //    //var user = await userService.GetUser(data);

        //    //if (user is null)
        //        //return NotFound();

        //    //userService.LogIn(data);
        //    return Ok();
        //}

    }
}
