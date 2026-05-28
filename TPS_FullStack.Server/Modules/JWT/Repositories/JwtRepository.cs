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

                var query = @"INSERT INTO dbo.RefreshToken(Id, UserId, refreshToken, ExpiryTime, IsRevoked, CreatedAt)
                            VALUES (@MaId, @UserId, @refreshToken, ExpiryTime, IsRevoked, CreatedAt)";   

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    //Input parameters


                    // Check query
                    int rows = cmd.ExecuteNonQuery();
                    if(rows == 1)
                    {
                        return true;
                    }
                    if(rows == 0)
                    {
                        return false;
                    }
                }
            }
            throw new NotImplementedException();
        }


    }
}


