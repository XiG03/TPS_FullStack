using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using TPS_FullStack.Server.Entities;
using TPS_FullStack.Server.Modules.JWT;

namespace TPS_FullStack.Server.Modules.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepo;
        private readonly IJwtService _jwtService;
        private readonly UserManager<AppUser> _userManager;

        public AuthService(IAuthRepository authRepo,
                            IJwtService jwtService,
                            UserManager<AppUser> userManager)
        {
            _authRepo = authRepo;
            _jwtService = jwtService;
            _userManager = userManager;
        }
        public async Task<ServiceDefault<LoginResponse>> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.Username);
                if (user == null)
                {
                    return new ServiceDefault<LoginResponse>
                    {
                        statusCode = StatusCodes.Status401Unauthorized,
                        Message = "Account not found",
                        Data = null
                    };
                }

                var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);
                if (!isValidPassword)
                {
                    return new ServiceDefault<LoginResponse>
                    {
                        statusCode = StatusCodes.Status401Unauthorized,
                        Message = "Incorrect Password",
                        Data = null
                    };
                }
                if (isValidPassword && user.Kichhoat == false)
                {
                    return new ServiceDefault<LoginResponse>
                    {
                        statusCode = StatusCodes.Status200OK,
                        Message = "First login",
                        Data = new LoginResponse
                        {
                            TempToken = "First_Login"
                        }
                    };
                }

                return new ServiceDefault<LoginResponse>
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Complete generate refresh and access token",
                    Data = new LoginResponse
                    {
                        RefreshToken = await _jwtService.GenerateRefreshTokenAsync(user.Id),
                        AccessToken = await _jwtService.GenerateAccessTokenAsync(user.Id, null)
                    }
                };
            } catch(Exception ex)
            {
                return new ServiceDefault<LoginResponse>
                {
                    statusCode = StatusCodes.Status500InternalServerError,
                    Message = "Server error" + ex.Message,
                    Data = null
                };
            }
            throw new NotImplementedException();
        }
    }
}


