using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TPS_FullStack.Server.Modules.JWT
{
    [Route("api/v1/jwt")]
    [ApiController]
    public class JWTController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        public JWTController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }
        
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return await Task.FromResult<IActionResult>(Unauthorized(new { message = "Refresh token is missing." }));
            }
            var result = await _jwtService.ValidateToken(refreshToken);
            return StatusCode(result.statusCode, result);
        }
    }
}
