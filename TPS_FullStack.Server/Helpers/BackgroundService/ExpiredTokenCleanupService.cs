using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Helpers
{
    public class ExpiredTokenCleanupService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        public ExpiredTokenCleanupService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await DeleteExpiredTokensAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting expired tokens: {ex.Message}");
                }
                await Task.Delay(TimeSpan.FromDays(7), stoppingToken);
            }
            throw new NotImplementedException();
        }
        private async Task DeleteExpiredTokensAsync()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("DELETE FROM RefreshTokens WHERE ExpiryTime < @CurrentDate", connection);
                command.Parameters.AddWithValue("@CurrentDate", DateTime.UtcNow);
                var affectedRows = await command.ExecuteNonQueryAsync();
                Console.WriteLine($"Deleted {affectedRows} expired tokens.");
            }
        }
    }

}

