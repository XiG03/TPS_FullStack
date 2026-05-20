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
        public async Task<ServiceDefault<TokenResponseDto>> LoginAsync(LoginDto request)
        {
            var user = await _authRepo.GetAccountAsync(request.Username);
            if (user == null)
            {
                return new ServiceDefault<TokenResponseDto>
                {
                    statusCode = StatusCodes.Status401Unauthorized,
                    Message = "Account not found"
                };
            }

            var isValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isValid)
            {
                return new ServiceDefault<TokenResponseDto>
                {
                    statusCode = StatusCodes.Status401Unauthorized,
                    Message = "Invalid password"
                };
            }
            var refreshtoken = await _jwtService.GenerateRefreshTokenAsync(user.Id);
            var accessToken = await _jwtService.GenerateAccessTokenAsync(user.Id, null); // Sua role

            if (refreshtoken.statusCode == StatusCodes.Status500InternalServerError)
            {
                return new ServiceDefault<TokenResponseDto>
                {
                    statusCode = StatusCodes.Status500InternalServerError,
                    Message = "Can not gen refresh and access token"
                };
            }

            return new ServiceDefault<TokenResponseDto>
            {
                statusCode = StatusCodes.Status200OK,
                Message = "Complete generate refresh and access token",
                Data = new TokenResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshtoken.Data
                }
            };


            // if (user.status == Status.NotFound)
            // {
            //     return new ServiceDefault<TokenResponseDto>
            //     {
            //         Success = false,
            //         Message = "Account Not Found"
            //     };
            // }

            // if (user.status == Status.Invalid)
            // {
            //     return new ServiceDefault<TokenResponseDto>
            //     {
            //         Success = false,
            //         Message = "Invalid Username or Password"
            //     };
            // }

            // if (user.Success == true && user.Data != null)
            // {
            //     var refreshtoken = await _jwtService.GenerateRefreshTokenAsync(user.Data.Id);
            //     var accessToken = await _jwtService.GenerateAccessTokenAsync(user.Data.Id, null); // Sua role
            //     if (!refreshtoken.Success)
            //     {
            //         return new ServiceDefault<TokenResponseDto>
            //         {
            //             Success = false,
            //             Message = "Can not gen refresh and access token"
            //         };
            //     }

            //     if (refreshtoken.Success)
            //     {
            //         return new ServiceDefault<TokenResponseDto>
            //         {
            //             Success = true,
            //             Message = "Complete generate refresh and access token",
            //             Data = new TokenResponseDto
            //             {
            //                 AccessToken = accessToken,
            //                 RefreshToken = refreshtoken.Data
            //             }
            //         };
            //     }
            throw new NotImplementedException();
        }



    }
}


