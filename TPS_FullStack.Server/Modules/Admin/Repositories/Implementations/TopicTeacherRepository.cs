using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class TopicTeacherRepository : ITopicTeacherRepository
    {
        public async Task CreateAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string? GiangvienID, string? ChuyendeID)
        {
            var query = @"INSERT INTO dbo.Chuyende_Giangvien (MaID, ChuyendeID, GiangvienID)
                        SELECT @MaID, @ChuyendeID, @GiangvienID
                        WHERE EXISTS (
                            SELECT 1
                            FROM dbo.Chuyende
                            WHERE MaID = @ChuyendeID
                        );";

            using var cmd = new SqlCommand(query, conn, trans);
            cmd.Parameters.AddWithValue("@MaID", MaID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ChuyendeID", ChuyendeID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@GiangvienID", GiangvienID ?? (object)DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(SqlConnection conn, SqlTransaction trans, string? MaID)
        {
            var query = @"DELETE FROM dbo.Chuyende_Giangvien WHERE MaID = @MaID";

            using var cmd = new SqlCommand(query, conn, trans);
            cmd.Parameters.AddWithValue("@MaID", MaID ?? (object)DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<TopicTeacherModel>> GetAllAsync(SqlConnection conn)
        {
            var query = @"SELECT MaID, ChuyendeID, GiangvienID
                  FROM dbo.Chuyende_Giangvien";

            using var cmd = new SqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            var list = new List<TopicTeacherModel>();

            while (await reader.ReadAsync())
            {
                list.Add(new TopicTeacherModel
                {
                    MaID = reader["MaID"]?.ToString(),
                    ChuyendeID = reader["ChuyendeID"]?.ToString(),
                    GiangvienID = reader["GiangvienID"]?.ToString()
                });
            }

            return list;
        }

        public async Task<TopicTeacherModel?> GetByIDAsync(SqlConnection conn, string? MaID)
        {
            var query = @"SELECT TOP 1 MaID, ChuyendeID, GiangvienID
                  FROM dbo.Chuyende_Giangvien
                  WHERE MaID = @MaID";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@MaID", MaID ?? (object)DBNull.Value);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new TopicTeacherModel
                {
                    MaID = reader["MaID"]?.ToString(),
                    ChuyendeID = reader["ChuyendeID"]?.ToString(),
                    GiangvienID = reader["GiangvienID"]?.ToString()
                };
            }

            return null;
        }

        public async Task<List<TopicsTeacherModel>> GetByTeacherIDAsync(SqlConnection conn, string? GiangvienID)
        {
            var queryGetByTeacherID = @" SELECT cdgv.MaID, cdgv.GiangvienID, cdgv.ChuyendeID, cd.Ten
                                        FROM dbo.Chuyende_Giangvien cdgv
                                        JOIN dbo.Chuyende cd ON cdgv.ChuyendeID = cd.MaID
                                        WHERE cdgv.GiangvienID = @GiangvienID";

            using var cmd = new SqlCommand(queryGetByTeacherID, conn);

            cmd.Parameters.AddWithValue("@GiangvienID", GiangvienID ?? (object)DBNull.Value);
            using var reader = await cmd.ExecuteReaderAsync();

            var list = new List<TopicsTeacherModel>();

            while (await reader.ReadAsync())
            {
                list.Add(new TopicsTeacherModel
                {
                    MaID = reader["MaID"]?.ToString(),
                    GiangvienID = reader["GiangvienID"]?.ToString(),
                    ChuyendeID = reader["ChuyendeID"]?.ToString(),
                    TenChuyende = reader["Ten"]?.ToString()
                });
            }

            return list;
        }

        public async Task UpdateAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string? GiangvienID, string? ChuyendeID)
        {
            var query = @"UPDATE dbo.Chuyende_Giangvien
                  SET GiangvienID = @GiangvienID,
                      ChuyendeID = @ChuyendeID
                  WHERE MaID = @MaID";

            using var cmd = new SqlCommand(query, conn, trans);
            cmd.Parameters.AddWithValue("@MaID", MaID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@GiangvienID", GiangvienID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ChuyendeID", ChuyendeID ?? (object)DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }
    }

}

