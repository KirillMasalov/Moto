using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moto.DB.Models;

namespace Moto.DB
{
    public class ApplicationUserManager: UserManager<User>
    {
        public ApplicationUserManager(
            IUserStore<User> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<User>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public override Task<User> FindByNameAsync(string userName)
        {
            return base.FindByNameAsync(userName);
        }
        public override Task<User> FindByEmailAsync(string email)
        {
            return base.FindByEmailAsync(email);
        }
    }
}
