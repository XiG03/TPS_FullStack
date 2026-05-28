using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class ScheduleTeacherAttendanceRepository : IScheduleTeacherAttendanceRepository
    {
        public async Task CreateAsync(
            SqlConnection conn,
            SqlTransaction trans,
            string? MaID,
            string? LichhocID,
            string? GiangvienID,
            string? KhoahocID,
            DateTime? Vaoluc,
            DateTime? Ngaydiemdanh)
        {
            string query = @"
                INSERT INTO Lichhoc_Giangvien_Diemdanh
                (
                    MaID,
                    LichhocID,
                    GiangvienID,
                    KhoahocID,
                    Vaoluc,
                    Ngaydiemdanh
                )
                VALUES
                (
                    @MaID,
                    @LichhocID,
                    @GiangvienID,
                    @KhoahocID,
                    @Vaoluc,
                    @Ngaydiemdanh
                )";

            using SqlCommand cmd = new SqlCommand(query, conn, trans);

            cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@LichhocID", (object?)LichhocID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@GiangvienID", (object?)GiangvienID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@KhoahocID", (object?)KhoahocID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Vaoluc", (object?)Vaoluc ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Ngaydiemdanh", (object?)Ngaydiemdanh ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(
            SqlConnection conn,
            SqlTransaction trans,
            string? MaID)
        {
            string query = @"
                DELETE FROM Lichhoc_Giangvien_Diemdanh
                WHERE MaID = @MaID";

            using SqlCommand cmd = new SqlCommand(query, conn, trans);

            cmd.Parameters.AddWithValue("@MaID", MaID);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<ScheduleTeacherAttendanceModel>> GetAllAsync(SqlConnection conn)
        {
            List<ScheduleTeacherAttendanceModel> list = new();

            string query = @"
                SELECT 
                    MaID,
                    LichhocID,
                    GiangvienID,
                    KhoahocID,
                    Vaoluc,
                    Ngaydiemdanh
                FROM Lichhoc_Giangvien_Diemdanh";

            using SqlCommand cmd = new SqlCommand(query, conn);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new ScheduleTeacherAttendanceModel
                {
                    MaID = reader["MaID"]?.ToString(),
                    LichhocID = reader["LichhocID"]?.ToString(),
                    GiangvienID = reader["GiangvienID"]?.ToString(),
                    KhoahocID = reader["KhoahocID"]?.ToString(),
                    Vaoluc = reader["Vaoluc"] as DateTime?,
                    Ngaydiemdanh = reader["Ngaydiemdanh"] as DateTime?
                });
            }

            return list;
        }

        public async Task<ScheduleTeacherAttendanceModel> GetByIDAsync(
            SqlConnection conn,
            string? MaID)
        {
            string query = @"
                SELECT 
                    MaID,
                    LichhocID,
                    GiangvienID,
                    KhoahocID,
                    Vaoluc,
                    Ngaydiemdanh
                FROM Lichhoc_Giangvien_Diemdanh
                WHERE MaID = @MaID";

            using SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@MaID", MaID);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new ScheduleTeacherAttendanceModel
                {
                    MaID = reader["MaID"]?.ToString(),
                    LichhocID = reader["LichhocID"]?.ToString(),
                    GiangvienID = reader["GiangvienID"]?.ToString(),
                    KhoahocID = reader["KhoahocID"]?.ToString(),
                    Vaoluc = reader["Vaoluc"] as DateTime?,
                    Ngaydiemdanh = reader["Ngaydiemdanh"] as DateTime?
                };
            }

            return null!;
        }

        public async Task UpdateAsync(
            SqlConnection conn,
            SqlTransaction trans,
            string? MaID,
            string? LichhocID,
            string? GiangvienID,
            string? KhoahocID,
            DateTime? Vaoluc,
            DateTime? Ngaydiemdanh)
        {
            string query = @"
                UPDATE Lichhoc_Giangvien_Diemdanh
                SET
                    LichhocID = @LichhocID,
                    GiangvienID = @GiangvienID,
                    KhoahocID = @KhoahocID,
                    Vaoluc = @Vaoluc,
                    Ngaydiemdanh = @Ngaydiemdanh
                WHERE MaID = @MaID";

            using SqlCommand cmd = new SqlCommand(query, conn, trans);

            cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@LichhocID", (object?)LichhocID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@GiangvienID", (object?)GiangvienID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@KhoahocID", (object?)KhoahocID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Vaoluc", (object?)Vaoluc ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Ngaydiemdanh", (object?)Ngaydiemdanh ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}