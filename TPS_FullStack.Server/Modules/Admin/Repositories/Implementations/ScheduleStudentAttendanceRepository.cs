using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class ScheduleStudentAttendanceRepository : IScheduleStudentAttendanceRepository
    {
        public async Task CreateAsync(
            SqlConnection conn,
            SqlTransaction trans,
            string? MaID,
            string? LichhocID,
            string? HocvienID,
            string? KhoahocID,
            DateTime? Vaoluc,
            DateTime? Ngaydiemdanh)
        {
            string query = @"
                INSERT INTO Lichhoc_Hocvien_Diemdanh
                (
                    MaID,
                    LichhocID,
                    HocvienID,
                    KhoahocID,
                    Vaoluc,
                    Ngaydiemdanh
                )
                VALUES
                (
                    @MaID,
                    @LichhocID,
                    @HocvienID,
                    @KhoahocID,
                    @Vaoluc,
                    @Ngaydiemdanh
                )";

            using SqlCommand cmd = new SqlCommand(query, conn, trans);

            cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@LichhocID", (object?)LichhocID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@HocvienID", (object?)HocvienID ?? DBNull.Value);
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
                DELETE FROM Lichhoc_Hocvien_Diemdanh
                WHERE MaID = @MaID";

            using SqlCommand cmd = new SqlCommand(query, conn, trans);

            cmd.Parameters.AddWithValue("@MaID", MaID);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<ScheduleStudentAttendanceModel>> GetAllAsync(SqlConnection conn)
        {
            List<ScheduleStudentAttendanceModel> list = new();

            string query = @"
                SELECT
                    MaID,
                    LichhocID,
                    HocvienID,
                    KhoahocID,
                    Vaoluc,
                    Ngaydiemdanh
                FROM Lichhoc_Hocvien_Diemdanh";

            using SqlCommand cmd = new SqlCommand(query, conn);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new ScheduleStudentAttendanceModel
                {
                    MaID = reader["MaID"]?.ToString(),
                    LichhocID = reader["LichhocID"]?.ToString(),
                    HocvienID = reader["HocvienID"]?.ToString(),
                    KhoahocID = reader["KhoahocID"]?.ToString(),
                    Vaoluc = reader["Vaoluc"] as DateTime?,
                    Ngaydiemdanh = reader["Ngaydiemdanh"] as DateTime?
                });
            }

            return list;
        }

        public async Task<ScheduleStudentAttendanceModel> GetByIDAsync(
            SqlConnection conn,
            string? MaID)
        {
            string query = @"
                SELECT
                    MaID,
                    LichhocID,
                    HocvienID,
                    KhoahocID,
                    Vaoluc,
                    Ngaydiemdanh
                FROM Lichhoc_Hocvien_Diemdanh
                WHERE MaID = @MaID";

            using SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@MaID", MaID);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new ScheduleStudentAttendanceModel
                {
                    MaID = reader["MaID"]?.ToString(),
                    LichhocID = reader["LichhocID"]?.ToString(),
                    HocvienID = reader["HocvienID"]?.ToString(),
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
            string? HocvienID,
            string? KhoahocID,
            DateTime? Vaoluc,
            DateTime? Ngaydiemdanh)
        {
            string query = @"
                UPDATE Lichhoc_Hocvien_Diemdanh
                SET
                    LichhocID = @LichhocID,
                    HocvienID = @HocvienID,
                    KhoahocID = @KhoahocID,
                    Vaoluc = @Vaoluc,
                    Ngaydiemdanh = @Ngaydiemdanh
                WHERE MaID = @MaID";

            using SqlCommand cmd = new SqlCommand(query, conn, trans);

            cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@LichhocID", (object?)LichhocID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@HocvienID", (object?)HocvienID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@KhoahocID", (object?)KhoahocID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Vaoluc", (object?)Vaoluc ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Ngaydiemdanh", (object?)Ngaydiemdanh ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}