using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moto.Controllers.ControllersInputModels;
using Moto.DB;
using Moto.DB.Models;
using Moto.Services.Interfaces;

namespace Moto.Services
{
    public class UserService: IUserService
    {
        private AppDbContext dbContext;

        public UserService(AppDbContext inDbContext) 
        {
            dbContext = inDbContext;
        }

        public async Task<User> GetUserByName(string name)
        {
            var normalizedName = name.ToUpper();
            return await dbContext.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedName);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var normalizedEmail = email.ToUpper();
            return await dbContext.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);
        }
    }
}
