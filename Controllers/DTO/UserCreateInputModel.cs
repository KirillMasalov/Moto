using System.ComponentModel.DataAnnotations;

namespace Moto.Controllers.ControllersInputModels
{
    public class UserCreateInputModel
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
