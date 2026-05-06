using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Identity.Client;

namespace TPS_FullStack.Server.Modules.JWT
{
    public interface IJwtService
    {
        public Task<ServiceDefault<string>> GenerateRefreshTokenAsync(string UserId);
        public string GenerateRefreshToken();

        public Task<string> GenerateAccessTokenAsync(string UserId, string role);

        //Ham check RefreshToken va Gen AccessToken
    }
}


