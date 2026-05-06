using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using TPS_FullStack.Server.Entities;

namespace TPS_FullStack.Server.Modules.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<AppUser> _userManager;

        public AuthRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<AppUser?> GetAccountAsync(string Username)
        {
            var user = await _userManager.FindByNameAsync(Username);
            return user;
            // if(user == null)
            // {
            //     return new ServiceDefault<AppUser>
            //     {
            //         Success = false,
            //         status = Status.NotFound
            //     };
            // }
            // if(!await _userManager.CheckPasswordAsync(user, password))
            // {
            //     return new ServiceDefault<AppUser>
            //     {
            //         Success = false,
            //         status = Status.Invalid
            //     };
            // }

            // if(await _userManager.CheckPasswordAsync(user, password))
            // {
            //     return new ServiceDefault<AppUser>
            //     {
            //         Success = true,
            //         Data = user
            //     };
            // }
            throw new NotImplementedException();
        }
    }

}

