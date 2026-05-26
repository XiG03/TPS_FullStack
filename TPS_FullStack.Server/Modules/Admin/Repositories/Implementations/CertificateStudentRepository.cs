using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class CertificateStudentRepository : ICertificateStudentRepository
    {
        public async Task CreateAsync(
            SqlConnection conn,
            SqlTransaction trans,
            string? MaID,
            string? ChungchiID,
            string? HocvienID,
            string? Mota,
            string? Donvicap,
            DateTime? Ngaycap,
            DateTime? Ngayhethan,
            DateTime? CreatedAt,
            string? CreatedBy,
            DateTime? UpdatedAt,
            string? UpdatedBy,
            DateTime? DeletedAt,
            string? DeletedBy)
        {
            var queryCreate = @"
                INSERT INTO dbo.Chungchi_Hocvien
                (
                    MaID,
                    ChungchiID,
                    HocvienID,
                    Mota,
                    Donvicap,
                    Ngaycap,
                    Ngayhethan,
                    CreatedAt,
                    CreatedBy,
                    UpdatedAt,
                    UpdatedBy,
                    DeletedAt,
                    DeletedBy,
                    Khongsudung
                )
                VALUES
                (
                    @MaID,
                    @ChungchiID,
                    @HocvienID,
                    @Mota,
                    @Donvicap,
                    @Ngaycap,
                    @Ngayhethan,
                    @CreatedAt,
                    @CreatedBy,
                    @UpdatedAt,
                    @UpdatedBy,
                    @DeletedAt,
                    @DeletedBy,
                    0
                )";

            using (var cmd = new SqlCommand(queryCreate, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ChungchiID", (object?)ChungchiID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@HocvienID", (object?)HocvienID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Mota", (object?)Mota ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Donvicap", (object?)Donvicap ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Ngaycap", (object?)Ngaycap ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Ngayhethan", (object?)Ngayhethan ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedAt", (object?)CreatedAt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedBy", (object?)CreatedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UpdatedAt", (object?)UpdatedAt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UpdatedBy", (object?)UpdatedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DeletedAt", (object?)DeletedAt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DeletedBy", (object?)DeletedBy ?? DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAsync(
            SqlConnection conn,
            SqlTransaction trans,
            string? MaID)
        {
            var queryDelete = @"
                UPDATE dbo.Chungchi_Hocvien
                SET
                    Khongsudung = 1
                WHERE MaID = @MaID";

            using (var cmd = new SqlCommand(queryDelete, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<CertificateStudentModel>> GetAllAsync(
            SqlConnection conn)
        {
            var queryGetAll = @"
                SELECT
                    cchv.MaID,

                    cchv.ChungchiID,
                    cc.Ten AS TenChungchi,

                    cchv.HocvienID,
                    hv.Hoten AS TenHocvien,

                    cchv.Mota,
                    cchv.Donvicap,
                    cchv.Ngaycap,
                    cchv.Ngayhethan

                FROM dbo.Chungchi_Hocvien cchv

                JOIN dbo.Chungchi cc
                    ON cc.MaID = cchv.ChungchiID

                JOIN dbo.Hocvien hv
                    ON hv.MaID = cchv.HocvienID

                WHERE cchv.Khongsudung = 0";

            var list = new List<CertificateStudentModel>();

            using (var cmd = new SqlCommand(queryGetAll, conn))
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(new CertificateStudentModel
                        {
                            MaID = reader["MaID"]?.ToString(),

                            ChungchiID = reader["ChungchiID"]?.ToString(),
                            TenChungchi = reader["TenChungchi"]?.ToString(),

                            HocvienID = reader["HocvienID"]?.ToString(),
                            TenHocvien = reader["TenHocvien"]?.ToString(),

                            Mota = reader["Mota"]?.ToString(),
                            Donvicap = reader["Donvicap"]?.ToString(),

                            Ngaycap = reader["Ngaycap"] == DBNull.Value
                                ? null
                                : Convert.ToDateTime(reader["Ngaycap"]),

                            Ngayhethan = reader["Ngayhethan"] == DBNull.Value
                                ? null
                                : Convert.ToDateTime(reader["Ngayhethan"])
                        });
                    }
                }
            }

            return list;
        }

        public async Task<CertificateStudentModel?> GetByIDAsync(
            SqlConnection conn,
            string? MaID)
        {
            var queryGetByID = @"
                SELECT TOP 1
                    cchv.MaID,

                    cchv.ChungchiID,
                    cchv.Chungchi_Ten AS TenChungchi,

                    cchv.HocvienID,
                    hv.Hoten AS TenHocvien,

                    cchv.Chungchi_Mota AS Mota,
                    cchv.Chungchi_Donvicap AS Dongvicap,
                    cchv.Ngaycap,
                    cchv.Ngayhethan

                FROM dbo.Chungchi_Hocvien cchv
                JOIN dbo.Hocvien hv
                    ON hv.MaID = cchv.HocvienID
                WHERE cchv.MaID = @MaID
                    AND cchv.Khongsudung = 0";

            CertificateStudentModel? model = null;

            using (var cmd = new SqlCommand(queryGetByID, conn))
            {
                cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        model = new CertificateStudentModel
                        {
                            MaID = reader["MaID"]?.ToString(),

                            ChungchiID = reader["ChungchiID"]?.ToString(),
                            TenChungchi = reader["TenChungchi"]?.ToString(),

                            HocvienID = reader["HocvienID"]?.ToString(),
                            TenHocvien = reader["TenHocvien"]?.ToString(),

                            Mota = reader["Mota"]?.ToString(),
                            Donvicap = reader["Donvicap"]?.ToString(),

                            Ngaycap = reader["Ngaycap"] == DBNull.Value
                                ? null
                                : Convert.ToDateTime(reader["Ngaycap"]),

                            Ngayhethan = reader["Ngayhethan"] == DBNull.Value
                                ? null
                                : Convert.ToDateTime(reader["Ngayhethan"])
                        };
                    }
                }
            }

            return model;
        }

        public async Task<List<CertificateStudentModel>> GetByStudentIDAsync(
            SqlConnection conn,
            string? HocvienID)
        {
            var queryGetByStudentID = @"
                SELECT
                    cchv.MaID,

                    cchv.ChungchiID,
                    cchv.Chungchi_Ten AS TenChungchi,

                    cchv.HocvienID,
                    hv.Hoten AS TenHocvien,

                    cchv.Chungchi_Mota AS Mota,
                    cchv.Chungchi_Donvicap AS Dongvicap,
                    cchv.Ngaycap,
                    cchv.Ngayhethan

                FROM dbo.Chungchi_Hocvien cchv

                JOIN dbo.Hocvien hv
                    ON hv.MaID = cchv.HocvienID

                WHERE cchv.HocvienID = @HocvienID
                    AND cchv.Khongsudung = 0";

            var list = new List<CertificateStudentModel>();

            using (var cmd = new SqlCommand(queryGetByStudentID, conn))
            {
                cmd.Parameters.AddWithValue(
                    "@HocvienID",
                    (object?)HocvienID ?? DBNull.Value);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(new CertificateStudentModel
                        {
                            MaID = reader["MaID"]?.ToString(),

                            ChungchiID = reader["ChungchiID"]?.ToString(),
                            TenChungchi = reader["TenChungchi"]?.ToString(),

                            HocvienID = reader["HocvienID"]?.ToString(),
                            TenHocvien = reader["TenHocvien"]?.ToString(),

                            Mota = reader["Mota"]?.ToString(),
                            Donvicap = reader["Donvicap"]?.ToString(),

                            Ngaycap = reader["Ngaycap"] == DBNull.Value
                                ? null
                                : Convert.ToDateTime(reader["Ngaycap"]),

                            Ngayhethan = reader["Ngayhethan"] == DBNull.Value
                                ? null
                                : Convert.ToDateTime(reader["Ngayhethan"])
                        });
                    }
                }
            }

            return list;
        }

        public async Task UpdateAsync(
            SqlConnection conn,
            SqlTransaction trans,
            string? MaID,
            string? ChungchiID,
            string? HocvienID,
            string? Mota,
            string? Donvicap,
            DateTime? Ngaycap,
            DateTime? Ngayhethan,
            DateTime? CreatedAt,
            string? CreatedBy,
            DateTime? UpdatedAt,
            string? UpdatedBy,
            DateTime? DeletedAt,
            string? DeletedBy)
        {
            var queryUpdate = @"
                UPDATE dbo.Chungchi_Hocvien
                SET
                    ChungchiID = @ChungchiID,
                    HocvienID = @HocvienID,
                    Chungchi_Mota = @Mota,
                    Chungchi_Donvicap = @Donvicap,
                    Ngaycap = @Ngaycap,
                    Ngayhethan = @Ngayhethan,
                    UpdatedAt = @UpdatedAt,
                    UpdatedBy = @UpdatedBy
                WHERE MaID = @MaID";

            using (var cmd = new SqlCommand(queryUpdate, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ChungchiID", (object?)ChungchiID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@HocvienID", (object?)HocvienID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Mota", (object?)Mota ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Donvicap", (object?)Donvicap ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Ngaycap", (object?)Ngaycap ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Ngayhethan", (object?)Ngayhethan ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UpdatedAt", (object?)UpdatedAt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UpdatedBy", (object?)UpdatedBy ?? DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}