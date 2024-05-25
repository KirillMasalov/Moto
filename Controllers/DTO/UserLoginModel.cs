using System.ComponentModel.DataAnnotations;

namespace Moto.Controllers.ControllersInputModels
{
    public class UserLoginModel
    {
        [Required]
        public string Login {  get; set; }
        [Required]
        public string Password { get; set; }

        public UserLoginModel(string login ,string password) 
        {
            Password = password;
            Login = login;
        }

    }
}
