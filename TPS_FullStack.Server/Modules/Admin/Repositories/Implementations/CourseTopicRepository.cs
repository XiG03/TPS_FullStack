using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class CourseTopicRepository : ICourseTopicRepository
    {
        public async Task CreateAsync(
            SqlConnection conn,
            SqlTransaction trans,
            string? MaID,
            string? KhoahocID,
            string? ChuyendeID,
            decimal? Socauhoi)
        {
            var query = @"
                INSERT INTO dbo.Khoahoc_Chuyende
                    (MaID, KhoahocID, ChuyendeID, Socauhoi)
                VALUES
                    (@MaID, @KhoahocID, @ChuyendeID, @Socauhoi)";

            using var cmd = new SqlCommand(query, conn, trans);

            cmd.Parameters.AddWithValue("@MaID", MaID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@KhoahocID", KhoahocID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ChuyendeID", ChuyendeID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Socauhoi", Socauhoi ?? (object)DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(
            SqlConnection conn,
            SqlTransaction trans,
            string? MaID)
        {
            var query = @"
                DELETE FROM dbo.Khoahoc_Chuyende
                WHERE MaID = @MaID";

            using var cmd = new SqlCommand(query, conn, trans);

            cmd.Parameters.AddWithValue("@MaID", MaID ?? (object)DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<CourseTopicModel>> GetAllAsync(SqlConnection conn)
        {
            var query = @"
                SELECT
                    MaID,
                    KhoahocID,
                    ChuyendeID,
                    Socauhoi
                FROM dbo.Khoahoc_Chuyende";

            using var cmd = new SqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            var list = new List<CourseTopicModel>();

            while (await reader.ReadAsync())
            {
                list.Add(new CourseTopicModel
                {
                    MaID = reader["MaID"]?.ToString(),
                    KhoahocID = reader["KhoahocID"]?.ToString(),
                    ChuyendeID = reader["ChuyendeID"]?.ToString(),
                    Socauhoi = reader["Socauhoi"] == DBNull.Value
                        ? null
                        : Convert.ToDecimal(reader["Socauhoi"])
                });
            }

            return list;
        }

        public async Task<CourseTopicModel?> GetByIDAsync(
            SqlConnection conn,
            string? MaID)
        {
            var query = @"
                SELECT TOP 1
                    MaID,
                    KhoahocID,
                    ChuyendeID,
                    Socauhoi
                FROM dbo.Khoahoc_Chuyende
                WHERE MaID = @MaID";

            using var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@MaID", MaID ?? (object)DBNull.Value);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new CourseTopicModel
                {
                    MaID = reader["MaID"]?.ToString(),
                    KhoahocID = reader["KhoahocID"]?.ToString(),
                    ChuyendeID = reader["ChuyendeID"]?.ToString(),
                    Socauhoi = reader["Socauhoi"] == DBNull.Value
                        ? null
                        : Convert.ToDecimal(reader["Socauhoi"])
                };
            }

            return null;
        }

        public async Task UpdateAsync(
            SqlConnection conn,
            SqlTransaction trans,
            string? MaID,
            string? KhoahocID,
            string? ChuyendeID,
            decimal? Socauhoi)
        {
            var query = @"
                UPDATE dbo.Khoahoc_Chuyende
                SET
                    KhoahocID = @KhoahocID,
                    ChuyendeID = @ChuyendeID,
                    Socauhoi = @Socauhoi
                WHERE MaID = @MaID";

            using var cmd = new SqlCommand(query, conn, trans);

            cmd.Parameters.AddWithValue("@MaID", MaID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@KhoahocID", KhoahocID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ChuyendeID", ChuyendeID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Socauhoi", Socauhoi ?? (object)DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}