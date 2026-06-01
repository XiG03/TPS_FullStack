
namespace TPS_FullStack.Server.Modules.Auth
{
    public interface IAuthService
    {
        public Task<ServiceDefault<LoginResponse>> LoginAsync(LoginRequest request);
        public Task<ServiceDefault<RegisterResponse>> RegisterAsync(RegisterRequest request);
    }   

}

