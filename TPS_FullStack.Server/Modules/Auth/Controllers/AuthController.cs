using Microsoft.AspNetCore.Mvc;
using TPS_FullStack.Server;
using TPS_FullStack.Server.Modules.Auth;

namespace TPS_FullStack.Server.Modules.Auth.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);
            if (result.Data?.RefreshToken != null)
            {
                Response.Cookies.Append("refreshToken", result.Data.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(7)
                });
            }
            return StatusCode(result.statusCode, result);
        }
        [HttpPost("register")]

        public async Task<ActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (request.Password != request.ConfirmPassword)
            {
                return BadRequest(new ServiceDefault<RegisterResponse>
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    Message = "Password and Confirm Password do not match",
                    Data = null
                });
            }
            var result = await _authService.RegisterAsync(request);
            return StatusCode(result.statusCode, result);
        }
    }
}