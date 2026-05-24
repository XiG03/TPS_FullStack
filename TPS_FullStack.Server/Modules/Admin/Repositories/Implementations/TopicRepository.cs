using System.Diagnostics;
using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class TopicRepository : ITopicRepository
    {
        public async Task CreateAsync(SqlConnection conn, SqlTransaction trans, string MaID,string Ten, string Mota, bool Khongsudung, DateTime? CreatedAt, string? CreatedBy, DateTime? UpdatedAt, string? UpdatedBy, DateTime? DeletedAt, string? DeletedBy)
        {
            var queryCreate = @"INSERT INTO dbo.Chuyende(MaID, Ten, Mota, Khongsudung, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, DeletedAt, DeletedBy)
                                VALUES (@MaID, @Ten, @Mota, @Khongsudung, @CreatedAt, @CreatedBy, @UpdatedAt, @UpdatedBy,  @DeletedAt, @DeletedBy)";
            using (var cmd = new SqlCommand(queryCreate, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Mota", (object?)Mota ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Ten", (object?)Ten ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Khongsudung", (object?)Khongsudung ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedAt", (object?)CreatedAt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedBy", (object?)CreatedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UpdatedAt", (object?)UpdatedAt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UpdatedBy", (object?)UpdatedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DeletedAt", (object?)DeletedAt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DeletedBy", (object?)DeletedBy ?? DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }
            // throw new NotImplementedException();
        }

        public async Task DeleteAsync(SqlConnection conn, SqlTransaction trans, string? MaID, DateTime DeletedAt, string DeletedBy)
        {
            var queryDelete = @"UPDATE dbo.Chuyende
                                SET
                                    DeletedAt = @DeletedAt,
                                    DeletedBy = @DeletedBy
                                WHERE MaID = @MaID";
            using(var cmd = new SqlCommand(queryDelete, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DeletedAt", (object?)DeletedAt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DeletedBy", (object?)DeletedBy ?? DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }
            // throw new NotImplementedException();
        }

        public async Task<List<TopicModel>> GetAllAsync(SqlConnection conn)
        {
            var queryGetAll = @"SELECT MaID, Ten, Mota, Khongsudung
                                FROM dbo.Chuyende
                                WHERE Khongsudung = 0";
            using(var cmd = new SqlCommand(queryGetAll, conn))
            {
                using var reader = await cmd.ExecuteReaderAsync();

                var result = new List<TopicModel>();
                while (await reader.ReadAsync())
                {
                    result.Add(new TopicModel
                    {
                        MaID = reader["MaID"].ToString(),
                        Ten = reader["Ten"].ToString(),
                        Mota = reader["Mota"].ToString()
                    });
                }
                return result;
            }
            // throw new NotImplementedException();
        }

        public async Task<TopicModel> GetByIdAsync(SqlConnection conn, string? MaID)
        {
            var queryGetById = @"SELECT MaID, Ten, Mota, Khongsudung
                                FROM dbo.Chuyende
                                WHERE MaID = @MaID AND Khongsudung = 0";
            using(var cmd = new SqlCommand(queryGetById, conn))
            {
                cmd.Parameters.AddWithValue("@MaID", MaID);
                using var reader = await cmd.ExecuteReaderAsync();
                var result = new TopicModel();
                if(await reader.ReadAsync())
                {
                    result.MaID = reader["MaID"].ToString();
                    result.Ten = reader["Ten"].ToString();
                    result.Mota = reader["Mota"].ToString();
                }
                else
                {
                    return null;
                }
                return result;
            }
            // throw new NotImplementedException();
        }

        public async Task UpdateAsync(SqlConnection conn, SqlTransaction trans, string MaID, string Ten, string Mota,  DateTime UpdatedAt, string UpdatedBy)
        {
            var queryUpdate = @"UPDATE dbo.Chuyende
                                SET
                                    Ten = @Ten,
                                    Mota = @Mota,
                                    UpdatedAt = @UpdatedAt,
                                    UpdatedBy = @UpdatedBy
                                WHERE MaID = @MaID";
            using(var cmd = new SqlCommand(queryUpdate, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Ten", (object?)Ten ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Mota", (object?)Mota ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UpdatedAt", (object?)UpdatedAt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UpdatedBy", (object?)UpdatedBy ?? DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }
            // throw new NotImplementedException();
        }
    }

}

