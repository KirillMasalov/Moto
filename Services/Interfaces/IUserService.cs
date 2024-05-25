using Moto.Controllers.ControllersInputModels;
using Moto.DB.Models;

namespace Moto.Services.Interfaces
{
    public interface IUserService
    {
        public Task<User> GetUserByName(string name);
        public Task<User> GetUserByEmail(string email);
    }
}
