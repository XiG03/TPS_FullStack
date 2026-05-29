using TPS_FullStack.Server.Entities;

namespace TPS_FullStack.Server.Modules.Auth
{
    public interface IAuthRepository
    {
        public Task<AppUser?> GetAccountAsync(string Username);
        public Task<AppUser?> GetByUserNameAsync(string? Username, string? Password);
        public Task<string?> GetRoleByIdAsync(string? UserId);
    }

}

