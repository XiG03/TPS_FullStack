using System.Data;
using Microsoft.Data.SqlClient;
using TPS_FullStack.Server.Entities;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class TopicsRepository : ITopicsRepository
    {
        private readonly IConfiguration _configuration;
        public TopicsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<List<Chuyende>> GetChuyendesAsync()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var list = new List<Chuyende>();

            using var conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            var query = @"SELECT * FROM Chuyende";

            using var cmd = new SqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while(await reader.ReadAsync())
            {
                list.Add(new Chuyende
                {
                    MaID = reader.GetString(0),
                    Ten = reader.GetString(1),
                    Mota = reader.GetString(2)
                });
            }

            return list;
            throw new NotImplementedException();
        }

        public Task<Chuyende_ChitietDto> GetTopicByID(string MaID)
        {
            throw new NotImplementedException();
        }
    }

}

