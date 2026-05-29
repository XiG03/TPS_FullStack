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
            return StatusCode(result.statusCode, result);
        }
    }
}