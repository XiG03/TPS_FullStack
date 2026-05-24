using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class CourseStudentRepository : ICourseStudentRepository
    {
        public async Task CreateAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string? KhoahocID, string? HocvienID, decimal? Diem, decimal? Dieuchinh)
        {
            var query = @"
                INSERT INTO dbo.Khoahoc_Hocvien
                    (MaID, KhoahocID, HocvienID, Diem, Dieuchinh)
                VALUES
                    (@MaID, @KhoahocID, @HocvienID, @Diem, @Dieuchinh)";

            using var cmd = new SqlCommand(query, conn, trans);
            cmd.Parameters.AddWithValue("@MaID", MaID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@KhoahocID", KhoahocID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@HocvienID", HocvienID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Diem", Diem ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Dieuchinh", Dieuchinh ?? (object)DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(SqlConnection conn, SqlTransaction trans, string? MaID)
        {
            var query = @"DELETE FROM dbo.Khoahoc_Hocvien WHERE MaID = @MaID";

            using var cmd = new SqlCommand(query, conn, trans);
            cmd.Parameters.AddWithValue("@MaID", MaID ?? (object)DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<CourseStudentModel>> GetAllAsync(SqlConnection conn)
        {
            var query = @"
                SELECT MaID, KhoahocID, HocvienID, Diem, Dieuchinh
                FROM dbo.Khoahoc_Hocvien";

            using var cmd = new SqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            var list = new List<CourseStudentModel>();

            while (await reader.ReadAsync())
            {
                list.Add(new CourseStudentModel
                {
                    MaID = reader["MaID"]?.ToString(),
                    KhoahocID = reader["KhoahocID"]?.ToString(),
                    HocvienID = reader["HocvienID"]?.ToString(),
                    Diem = reader["Diem"] == DBNull.Value ? null : Convert.ToDecimal(reader["Diem"]),
                    Dieuchinh = reader["Dieuchinh"] == DBNull.Value ? null : Convert.ToDecimal(reader["Dieuchinh"])
                });
            }

            return list;
        }

        public async Task<CourseStudentModel?> GetByIDAsync(SqlConnection conn, string? MaID)
        {
            var query = @"
                SELECT TOP 1 MaID, KhoahocID, HocvienID, Diem, Dieuchinh
                FROM dbo.Khoahoc_Hocvien
                WHERE MaID = @MaID";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@MaID", MaID ?? (object)DBNull.Value);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new CourseStudentModel
                {
                    MaID = reader["MaID"]?.ToString(),
                    KhoahocID = reader["KhoahocID"]?.ToString(),
                    HocvienID = reader["HocvienID"]?.ToString(),
                    Diem = reader["Diem"] == DBNull.Value ? null : Convert.ToDecimal(reader["Diem"]),
                    Dieuchinh = reader["Dieuchinh"] == DBNull.Value ? null : Convert.ToDecimal(reader["Dieuchinh"])
                };
            }

            return null;
        }

        public async Task UpdateAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string? KhoahocID, string? HocvienID, decimal? Diem, decimal? Dieuchinh)
        {
            var query = @"
                UPDATE dbo.Khoahoc_Hocvien
                SET 
                    KhoahocID = @KhoahocID,
                    HocvienID = @HocvienID,
                    Diem = @Diem,
                    Dieuchinh = @Dieuchinh
                WHERE MaID = @MaID";

            using var cmd = new SqlCommand(query, conn, trans);
            cmd.Parameters.AddWithValue("@MaID", MaID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@KhoahocID", KhoahocID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@HocvienID", HocvienID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Diem", Diem ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Dieuchinh", Dieuchinh ?? (object)DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}