using Microsoft.Data.SqlClient;
using TPS_FullStack.Server.Entities;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class ExamRepository : IExamRepository
    {
        public async Task CreateAsync(
            SqlConnection conn,
            SqlTransaction trans,
            string? MaID,
            string? KhoahocID,
            string? HocvienID,
            DateTime? Batdauthi,
            DateTime? Ketthucthi,
            DateTime? CreatedAt,
            string? CreatedBy,
            DateTime? UpdatedAt,
            string? UpdatedBy,
            DateTime? DeletedAt,
            string? DeletedBy)
        {
            var query = @"
                INSERT INTO dbo.Baithuhoach
                (
                    MaID,
                    KhoahocID,
                    HocvienID,
                    Batdauthi,
                    Ketthucthi,
                    CreatedAt,
                    CreatedBy,
                    UpdatedAt,
                    UpdatedBy,
                    DeletedAt,
                    DeletedBy
                )
                VALUES
                (
                    @MaID,
                    @KhoahocID,
                    @HocvienID,
                    @Batdauthi,
                    @Ketthucthi,
                    @CreatedAt,
                    @CreatedBy,
                    @UpdatedAt,
                    @UpdatedBy,
                    @DeletedAt,
                    @DeletedBy
                )";

            using var cmd = new SqlCommand(query, conn, trans);

            cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@KhoahocID", (object?)KhoahocID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@HocvienID", (object?)HocvienID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Batdauthi", (object?)Batdauthi ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Ketthucthi", (object?)Ketthucthi ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedAt", (object?)CreatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedBy", (object?)CreatedBy ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdatedAt", (object?)UpdatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdatedBy", (object?)UpdatedBy ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DeletedAt", (object?)DeletedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DeletedBy", (object?)DeletedBy ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(SqlConnection conn, SqlTransaction trans, string? MaID)
        {
            var query = @"
                UPDATE dbo.Baithuhoach
                SET Khongsudung = 1
                WHERE MaID = @MaID";

            using var cmd = new SqlCommand(query, conn, trans);
            cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<ExamModel>> GetAllAsync(SqlConnection conn)
        {
            var query = @"
                        SELECT
                            e.MaID,

                            e.KhoahocID,
                            kh.Ten AS TenKhoahoc,

                            e.HocvienID,
                            hv.Hoten AS TenHocvien,

                            e.Batdauthi,
                            e.Ketthucthi,

                            dbo.fnc_Baithuhoach_Tinhdiem(e.MaID) AS Diem

                        FROM dbo.Baithuhoach e
                        JOIN dbo.Khoahoc kh ON kh.MaID = e.KhoahocID
                        JOIN dbo.Hocvien hv ON hv.MaID = e.HocvienID";

            var list = new List<ExamModel>();

            using var cmd = new SqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new ExamModel
                {
                    MaID = reader["MaID"]?.ToString(),
                    KhoahocID = reader["KhoahocID"]?.ToString(),
                    TenKhoahoc = reader["TenKhoahoc"]?.ToString(),
                    HocvienID = reader["HocvienID"]?.ToString(),
                    TenHocvien = reader["TenHocvien"]?.ToString(),

                    Batdauthi = reader["Batdauthi"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["Batdauthi"]),

                    Ketthucthi = reader["Ketthucthi"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["Ketthucthi"]),
                    Diem = reader["Diem"] == DBNull.Value
                        ? null
                        : Convert.ToDecimal(reader["Diem"]),
                });
            }

            return list;
        }

        public async Task<ExamModel?> GetByIDAsync(SqlConnection conn, string? MaID)
        {
            var query = @"
                SELECT TOP 1
                    e.MaID,

                    e.KhoahocID,
                    kh.Ten AS TenKhoahoc,

                    e.HocvienID,
                    hv.Hoten AS TenHocvien,

                    e.Batdauthi,
                    e.Ketthucthi,
                    
                    dbo.fnc_Baithuhoach_Tinhdiem(e.MaID) AS Diem

                FROM dbo.Baithuhoach e

                JOIN dbo.Khoahoc kh ON kh.MaID = e.KhoahocID
                JOIN dbo.Hocvien hv ON hv.MaID = e.HocvienID

                WHERE e.MaID = @MaID";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new ExamModel
                {
                    MaID = reader["MaID"]?.ToString(),
                    KhoahocID = reader["KhoahocID"]?.ToString(),
                    TenKhoahoc = reader["TenKhoahoc"]?.ToString(),
                    HocvienID = reader["HocvienID"]?.ToString(),
                    TenHocvien = reader["TenHocvien"]?.ToString(),

                    Batdauthi = reader["Batdauthi"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["Batdauthi"]),

                    Ketthucthi = reader["Ketthucthi"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["Ketthucthi"]),
                    Diem = reader["Diem"] == DBNull.Value
                        ? null
                        : Convert.ToDecimal(reader["Diem"])
                };
            }

            return null;
        }

        public async Task<List<ExamModel>> GetByStudentIDAsync(SqlConnection conn, string? HocvienID)
        {
            var query = @"SELECT
                    e.MaID,

                    e.KhoahocID,
                    kh.Ten AS TenKhoahoc,

                    e.HocvienID,
                    hv.Hoten AS TenHocvien,

                    e.Batdauthi,
                    e.Ketthucthi,
                    
                    dbo.fnc_Baithuhoach_Tinhdiem(e.MaID) AS Diem

                FROM dbo.Baithuhoach e

                JOIN dbo.Khoahoc kh ON kh.MaID = e.KhoahocID
                JOIN dbo.Hocvien hv ON hv.MaID = e.HocvienID

                WHERE e.HocvienID = @HocvienID";
            
            using(var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@HocvienID", (object?)HocvienID ?? DBNull.Value);

                using var reader = await cmd.ExecuteReaderAsync();
                var list = new List<ExamModel>();

                while(await reader.ReadAsync())
                {
                    list.Add(new ExamModel
                    {
                        MaID = reader["MaID"]?.ToString(),
                    KhoahocID = reader["KhoahocID"]?.ToString(),
                    TenKhoahoc = reader["TenKhoahoc"]?.ToString(),
                    HocvienID = reader["HocvienID"]?.ToString(),
                    TenHocvien = reader["TenHocvien"]?.ToString(),

                    Batdauthi = reader["Batdauthi"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["Batdauthi"]),

                    Ketthucthi = reader["Ketthucthi"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["Ketthucthi"]),
                    Diem = reader["Diem"] == DBNull.Value
                        ? null
                        : Convert.ToDecimal(reader["Diem"]) 
                    });
                }
                if(list.Count == 0)
                {
                    return null;
                }
                return list;
            }
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(
            SqlConnection conn,
            SqlTransaction trans,
            string? MaID,
            string? KhoahocID,
            string? HocvienID,
            DateTime? Batdauthi,
            DateTime? Ketthucthi,
            DateTime? CreatedAt,
            string? CreatedBy,
            DateTime? UpdatedAt,
            string? UpdatedBy,
            DateTime? DeletedAt,
            string? DeletedBy)
        {
            var query = @"
                UPDATE dbo.Baithuhoach
                SET
                    KhoahocID = @KhoahocID,
                    HocvienID = @HocvienID,
                    Batdauthi = @Batdauthi,
                    Ketthucthi = @Ketthucthi,
                    UpdatedAt = @UpdatedAt,
                    UpdatedBy = @UpdatedBy
                WHERE MaID = @MaID";

            using var cmd = new SqlCommand(query, conn, trans);

            cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@KhoahocID", (object?)KhoahocID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@HocvienID", (object?)HocvienID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Batdauthi", (object?)Batdauthi ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Ketthucthi", (object?)Ketthucthi ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdatedAt", (object?)UpdatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdatedBy", (object?)UpdatedBy ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}