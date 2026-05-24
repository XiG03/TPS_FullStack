using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class CourseTeacherRepository : ICourseTeacherRepository
    {
        public async Task CreateAsync(
            SqlConnection conn,
            SqlTransaction trans,
            string? MaID,
            string? KhoahocID,
            string? GiangvienID)
        {
            var query = @"
                INSERT INTO dbo.Khoahoc_Giangvien
                    (MaID, KhoahocID, GiangvienID)
                VALUES
                    (@MaID, @KhoahocID, @GiangvienID)";

            using var cmd = new SqlCommand(query, conn, trans);

            cmd.Parameters.AddWithValue("@MaID", MaID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@KhoahocID", KhoahocID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@GiangvienID", GiangvienID ?? (object)DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(
            SqlConnection conn,
            SqlTransaction trans,
            string? MaID)
        {
            var query = @"
                DELETE FROM dbo.Khoahoc_Giangvien
                WHERE MaID = @MaID";

            using var cmd = new SqlCommand(query, conn, trans);

            cmd.Parameters.AddWithValue("@MaID", MaID ?? (object)DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<CourseTeacherModel>> GetAllAsync(SqlConnection conn)
        {
            var query = @"
                SELECT 
                    MaID,
                    KhoahocID,
                    GiangvienID
                FROM dbo.Khoahoc_Giangvien";

            using var cmd = new SqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            var list = new List<CourseTeacherModel>();

            while (await reader.ReadAsync())
            {
                list.Add(new CourseTeacherModel
                {
                    MaID = reader["MaID"]?.ToString(),
                    KhoahocID = reader["KhoahocID"]?.ToString(),
                    GiangvienID = reader["GiangvienID"]?.ToString()
                });
            }

            return list;
        }

        public async Task<CourseTeacherModel?> GetByIDAsync(
            SqlConnection conn,
            string? MaID)
        {
            var query = @"
                SELECT TOP 1
                    MaID,
                    KhoahocID,
                    GiangvienID
                FROM dbo.Khoahoc_Giangvien
                WHERE MaID = @MaID";

            using var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@MaID", MaID ?? (object)DBNull.Value);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new CourseTeacherModel
                {
                    MaID = reader["MaID"]?.ToString(),
                    KhoahocID = reader["KhoahocID"]?.ToString(),
                    GiangvienID = reader["GiangvienID"]?.ToString()
                };
            }

            return null;
        }

        public async Task UpdateAsync(
            SqlConnection conn,
            SqlTransaction trans,
            string? MaID,
            string? KhoahocID,
            string? GiangvienID)
        {
            var query = @"
                UPDATE dbo.Khoahoc_Giangvien
                SET
                    KhoahocID = @KhoahocID,
                    GiangvienID = @GiangvienID
                WHERE MaID = @MaID";

            using var cmd = new SqlCommand(query, conn, trans);

            cmd.Parameters.AddWithValue("@MaID", MaID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@KhoahocID", KhoahocID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@GiangvienID", GiangvienID ?? (object)DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}