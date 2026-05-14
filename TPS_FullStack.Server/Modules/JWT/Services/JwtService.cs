using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

namespace TPS_FullStack.Server.Modules.JWT
{

    public class JwtService : IJwtService
    {
        private readonly IJwtRepository _jwtRepo;
        private readonly IConfiguration _configuration;

        public JwtService(IJwtRepository jwtRepo,
                            IConfiguration configuration)
        {
            _jwtRepo = jwtRepo;
            _configuration = configuration;
        }
        public async Task<ServiceDefault<string>> GenerateRefreshTokenAsync(string UserId)
        {
            var refreshToken = GenerateRefreshToken();
            var ExpiryTime = DateTime.UtcNow.AddDays(7);
            var result = _jwtRepo.SaveRefreshToken(UserId, refreshToken, ExpiryTime);
            if (!result && refreshToken != null)
            {
                return new ServiceDefault<string>
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Complete generate refreshtoken",
                    Data = refreshToken
                };
            }
            else
            {
                return new ServiceDefault<string>
                {
                    statusCode = StatusCodes.Status500InternalServerError,
                    Message = "Generating refreshtoken fail"
                };
            }
            throw new NotImplementedException();
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new Byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
            throw new NotImplementedException();
        }

        public async Task<string> GenerateAccessTokenAsync(string UserId, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, UserId),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Token")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var tokenDescriptor = new JwtSecurityToken(

            issuer: _configuration.GetValue<string>("Jwt:Issuer"),
            audience: _configuration.GetValue<string>("Jwt:Audience"),
            claims: claims,
            expires: DateTime.Now.AddDays(1), // can be change on 
            signingCredentials: creds
        );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            
            throw new NotImplementedException();
        }


    }
}

