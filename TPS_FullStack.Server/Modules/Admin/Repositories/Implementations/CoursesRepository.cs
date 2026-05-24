using Microsoft.Data.SqlClient;
using TPS_FullStack.Server.Entities;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class CoursesRepository : ICoursesRepository
    {
        public async Task CreateAsync(SqlConnection conn, SqlTransaction trans,
            string? KhoahocID, string? Ten, string? Mota, decimal? Diemdat,
            string? ChungchiID, string? Thu, decimal? Thoiluonghoc,
            decimal? Sobuoihoc, decimal? Thoiluongthi, DateTime? Ngaybatdau,
            decimal? Socauhoi, DateTime? CreatedAt, string? CreatedBy,
            DateTime? UpdatedAt, string? UpdatedBy, DateTime? DeletedAt,
            string? DeletedBy)
        {
            var query = @"
                INSERT INTO dbo.Khoahoc
                (
                    MaID, Ten, Mota, Diemdat, ChungchiID, Thu,
                    Thoiluonghoc, Sobuoihoc, Thoiluongthi, Ngaybatdau,
                    Socauhoi, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy,
                    DeletedAt, DeletedBy, Khongsudung
                )
                VALUES
                (
                    @MaID, @Ten, @Mota, @Diemdat, @ChungchiID, @Thu,
                    @Thoiluonghoc, @Sobuoihoc, @Thoiluongthi, @Ngaybatdau,
                    @Socauhoi, @CreatedAt, @CreatedBy, @UpdatedAt, @UpdatedBy,
                    @DeletedAt, @DeletedBy, 0
                )";

            using var cmd = new SqlCommand(query, conn, trans);

            AddParameters(cmd, KhoahocID, Ten, Mota, Diemdat, ChungchiID, Thu,
                Thoiluonghoc, Sobuoihoc, Thoiluongthi, Ngaybatdau, Socauhoi,
                CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, DeletedAt, DeletedBy);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(SqlConnection conn, SqlTransaction trans,
            string? KhoahocID, DateTime? DeletedAt, string? DeletedBy)
        {
            var query = @"
                UPDATE dbo.Khoahoc
                SET
                    Khongsudung = 1,
                    DeletedAt = @DeletedAt,
                    DeletedBy = @DeletedBy
                WHERE MaID = @MaID";

            using var cmd = new SqlCommand(query, conn, trans);

            cmd.Parameters.AddWithValue("@MaID", (object?)KhoahocID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DeletedAt", (object?)DeletedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DeletedBy", (object?)DeletedBy ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<CourseModel>> GetAllAsync(SqlConnection conn)
        {
            var query = @"
                SELECT 
                    MaID, Ten, Mota, Diemdat, ChungchiID, Thu,
                    Thoiluonghoc, Sobuoihoc, Ngaybatdau,
                    Thoiluongthi, Socauhoi
                FROM dbo.Khoahoc
                WHERE Khongsudung = 0";

            var list = new List<CourseModel>();

            using var cmd = new SqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(MapCourse(reader));
            }

            return list;
        }

        public async Task<CourseModel?> GetByIDAsync(
            SqlConnection conn,
            string? KhoahocID,
            string? ChungchiID)
        {
            var query = @"
                SELECT TOP 1
                    MaID, Ten, Mota, Diemdat, ChungchiID, Thu,
                    Thoiluonghoc, Sobuoihoc, Ngaybatdau,
                    Thoiluongthi, Socauhoi
                FROM dbo.Khoahoc
                WHERE Khongsudung = 0 
                  AND MaID = @KhoahocID";

            using var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@KhoahocID", (object?)KhoahocID ?? DBNull.Value);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapCourse(reader);
            }

            return null;
        }

        public async Task UpdateAsync(SqlConnection conn, SqlTransaction trans,
            string? KhoahocID, string? Ten, string? Mota, decimal? Diemdat,
            string? ChungchiID, string? Thu, decimal? Thoiluonghoc,
            decimal? Sobuoihoc, decimal? Thoiluongthi, DateTime? Ngaybatdau,
            decimal? Socauhoi, DateTime? CreatedAt, string? CreatedBy,
            DateTime? UpdatedAt, string? UpdatedBy, DateTime? DeletedAt,
            string? DeletedBy)
        {
            var query = @"
                UPDATE dbo.Khoahoc
                SET
                    Ten = @Ten,
                    Mota = @Mota,
                    Diemdat = @Diemdat,
                    ChungchiID = @ChungchiID,
                    Thu = @Thu,
                    Thoiluonghoc = @Thoiluonghoc,
                    Sobuoihoc = @Sobuoihoc,
                    Ngaybatdau = @Ngaybatdau,
                    Thoiluongthi = @Thoiluongthi,
                    Socauhoi = @Socauhoi,
                    UpdatedAt = @UpdatedAt,
                    UpdatedBy = @UpdatedBy
                WHERE MaID = @MaID";

            using var cmd = new SqlCommand(query, conn, trans);

            AddParameters(cmd, KhoahocID, Ten, Mota, Diemdat, ChungchiID, Thu,
                Thoiluonghoc, Sobuoihoc, Thoiluongthi, Ngaybatdau, Socauhoi,
                CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, DeletedAt, DeletedBy);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateCourseIDAsync(
            SqlConnection conn,
            SqlTransaction trans,
            string? KhoahocID,
            string? ChungchiID,
            DateTime? UpdatedAt,
            string? UpdatedBy)
        {
            var query = @"
                UPDATE dbo.Khoahoc
                SET
                    ChungchiID = @ChungchiID,
                    UpdatedAt = @UpdatedAt,
                    UpdatedBy = @UpdatedBy
                WHERE MaID = @KhoahocID";

            using var cmd = new SqlCommand(query, conn, trans);

            cmd.Parameters.AddWithValue("@KhoahocID", (object?)KhoahocID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ChungchiID", (object?)ChungchiID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdatedAt", (object?)UpdatedAt ?? DateTime.Now);
            cmd.Parameters.AddWithValue("@UpdatedBy", (object?)UpdatedBy ?? "system");

            await cmd.ExecuteNonQueryAsync();
        }

        private static CourseModel MapCourse(SqlDataReader reader)
        {
            return new CourseModel
            {
                KhoahocID = reader["MaID"]?.ToString(),
                Ten = reader["Ten"]?.ToString(),
                Mota = reader["Mota"]?.ToString(),
                Diemdat = reader["Diemdat"] == DBNull.Value ? null : Convert.ToDecimal(reader["Diemdat"]),
                ChungchiID = reader["ChungchiID"]?.ToString(),
                Thu = reader["Thu"]?.ToString(),
                Thoiluonghoc = reader["Thoiluonghoc"] == DBNull.Value ? null : Convert.ToDecimal(reader["Thoiluonghoc"]),
                Sobuoihoc = reader["Sobuoihoc"] == DBNull.Value ? null : Convert.ToDecimal(reader["Sobuoihoc"]),
                Ngaybatdau = reader["Ngaybatdau"] == DBNull.Value ? null : Convert.ToDateTime(reader["Ngaybatdau"]),
                Thoiluongthi = reader["Thoiluongthi"] == DBNull.Value ? null : Convert.ToDecimal(reader["Thoiluongthi"]),
                Socauhoi = reader["Socauhoi"] == DBNull.Value ? null : Convert.ToDecimal(reader["Socauhoi"])
            };
        }

        private static void AddParameters(
            SqlCommand cmd,
            string? KhoahocID,
            string? Ten,
            string? Mota,
            decimal? Diemdat,
            string? ChungchiID,
            string? Thu,
            decimal? Thoiluonghoc,
            decimal? Sobuoihoc,
            decimal? Thoiluongthi,
            DateTime? Ngaybatdau,
            decimal? Socauhoi,
            DateTime? CreatedAt,
            string? CreatedBy,
            DateTime? UpdatedAt,
            string? UpdatedBy,
            DateTime? DeletedAt,
            string? DeletedBy)
        {
            cmd.Parameters.AddWithValue("@MaID", (object?)KhoahocID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Ten", (object?)Ten ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Mota", (object?)Mota ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Diemdat", (object?)Diemdat ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ChungchiID", (object?)ChungchiID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Thu", (object?)Thu ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Thoiluonghoc", (object?)Thoiluonghoc ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Sobuoihoc", (object?)Sobuoihoc ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Thoiluongthi", (object?)Thoiluongthi ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Ngaybatdau", (object?)Ngaybatdau ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Socauhoi", (object?)Socauhoi ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedAt", (object?)CreatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedBy", (object?)CreatedBy ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdatedAt", (object?)UpdatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdatedBy", (object?)UpdatedBy ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DeletedAt", (object?)DeletedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DeletedBy", (object?)DeletedBy ?? DBNull.Value);
        }
    }
}