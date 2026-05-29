using System.Threading.RateLimiting;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;

namespace TPS_FullStack.Server.Modules.JWT
{
    public class JwtRepository : IJwtRepository
    {
        private readonly IConfiguration _configuration;

        public JwtRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool SaveRefreshToken(string UserId, string refreshToken, DateTime ExpiryTime)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var query = @"INSERT INTO dbo.RefreshTokens (Id, UserId, refreshToken, ExpiryTime, IsRevoked, CreatedAt)
                            VALUES (@MaId, @UserId, @refreshToken, @ExpiryTime, @IsRevoked, @CreatedAt)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaId", Guid.NewGuid().ToString());
                    cmd.Parameters.AddWithValue("@UserId", string.IsNullOrWhiteSpace(UserId) ? DBNull.Value : UserId);
                    cmd.Parameters.AddWithValue("@refreshToken", (object?)refreshToken ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ExpiryTime", (object?)ExpiryTime ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsRevoked", false);
                    cmd.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);
                    // Check query
                    int rows = cmd.ExecuteNonQuery();
                    if (rows == 1)
                    {
                        return true;
                    }
                    if (rows == 0)
                    {
                        return false;
                    }
                }
            }
            throw new NotImplementedException();
        }


    }
}


