using Microsoft.Data.SqlClient;
using TPS_FullStack.Server.Entities;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class SchedulesRepository : ISchedulesRepository
    {
        public async Task CreateAsync(
            SqlConnection conn,
            SqlTransaction trans,
            string? LichhocID,
            string? KhoahocID,
            string? ChuyendeID,
            string? GiangvienID,
            DateTime? Ngaydukien,
            DateTime? Batdaudukien,
            DateTime? Ketthucdukien,
            DateTime? Ngaythucte,
            DateTime? Batdauthucte,
            DateTime? Ketthucthucte)
        {
            var query = @"
                INSERT INTO dbo.Lichhoc
                (
                    MaID,
                    KhoahocID,
                    ChuyendeID,
                    GiangvienID,
                    Ngaydukien,
                    Batdaudukien,
                    Ketthucdukien,
                    Ngaythucte,
                    Batdauthucte,
                    Ketthucthucte
                )
                VALUES
                (
                    @LichhocID,
                    @KhoahocID,
                    @ChuyendeID,
                    @GiangvienID,
                    @Ngaydukien,
                    @Batdaudukien,
                    @Ketthucdukien,
                    @Ngaythucte,
                    @Batdauthucte,
                    @Ketthucthucte
                )";

            using var cmd = new SqlCommand(query, conn, trans);

            cmd.Parameters.AddWithValue("@LichhocID", LichhocID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@KhoahocID", KhoahocID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ChuyendeID", ChuyendeID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@GiangvienID", GiangvienID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Ngaydukien", Ngaydukien ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Batdaudukien", Batdaudukien ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Ketthucdukien", Ketthucdukien ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Ngaythucte", Ngaythucte ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Batdauthucte", Batdauthucte ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Ketthucthucte", Ketthucthucte ?? (object)DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(
            SqlConnection conn,
            SqlTransaction trans,
            string? LichhocID)
        {
            var query = @"
                DELETE FROM dbo.Lichhoc
                WHERE MaID = @LichhocID";

            using var cmd = new SqlCommand(query, conn, trans);

            cmd.Parameters.AddWithValue("@LichhocID", LichhocID ?? (object)DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<ScheduleModel>> GetAllAsync(SqlConnection conn)
        {
            var query = @"
        SELECT 
            lh.MaID,

            lh.KhoahocID,
            kh.Ten AS TenKhoahoc,

            lh.ChuyendeID,
            cd.Ten AS TenChuyende,

            lh.GiangvienID,
            gv.Hoten AS TenGiangvien,

            lh.Ngaydukien,
            lh.Batdaudukien,
            lh.Ketthucdukien,

            lh.Ngaythucte,
            lh.Batdauthucte,
            lh.Ketthucthucte

        FROM dbo.Lichhoc lh
        LEFT JOIN dbo.Khoahoc kh 
            ON kh.MaID = lh.KhoahocID

        LEFT JOIN dbo.Chuyende cd 
            ON cd.MaID = lh.ChuyendeID

        LEFT JOIN dbo.Giangvien gv 
            ON gv.MaID = lh.GiangvienID";

            using var cmd = new SqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            var list = new List<ScheduleModel>();

            while (await reader.ReadAsync())
            {
                list.Add(new ScheduleModel
                {
                    LichhocID = reader["MaID"]?.ToString(),

                    KhoahocID = reader["KhoahocID"]?.ToString(),
                    TenKhoahoc = reader["TenKhoahoc"]?.ToString(),

                    ChuyendeID = reader["ChuyendeID"]?.ToString(),
                    TenChuyende = reader["TenChuyende"]?.ToString(),

                    GiangvienID = reader["GiangvienID"]?.ToString(),
                    TenGiangvien = reader["TenGiangvien"]?.ToString(),

                    Ngaydukien = reader["Ngaydukien"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["Ngaydukien"]),

                    Batdaudukien = reader["Batdaudukien"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["Batdaudukien"]),

                    Ketthucdukien = reader["Ketthucdukien"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["Ketthucdukien"]),

                    Ngaythucte = reader["Ngaythucte"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["Ngaythucte"]),

                    Batdauthucte = reader["Batdauthucte"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["Batdauthucte"]),

                    Ketthucthucte = reader["Ketthucthucte"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["Ketthucthucte"])
                });
            }

            return list;
        }

        public async Task<ScheduleModel?> GetByIDAsync(
    SqlConnection conn,
    string? LichhocID)
        {
            var query = @"
        SELECT TOP 1
            lh.MaID,

            lh.KhoahocID,
            kh.Ten AS TenKhoahoc,

            lh.ChuyendeID,
            cd.Ten AS TenChuyende,

            lh.GiangvienID,
            gv.Hoten AS TenGiangvien,

            lh.Ngaydukien,
            lh.Batdaudukien,
            lh.Ketthucdukien,

            lh.Ngaythucte,
            lh.Batdauthucte,
            lh.Ketthucthucte

        FROM dbo.Lichhoc lh

        LEFT JOIN dbo.Khoahoc kh
            ON kh.MaID = lh.KhoahocID

        LEFT JOIN dbo.Chuyende cd
            ON cd.MaID = lh.ChuyendeID

        LEFT JOIN dbo.Giangvien gv
            ON gv.MaID = lh.GiangvienID

        WHERE lh.MaID = @LichhocID";

            using var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue(
                "@LichhocID",
                LichhocID ?? (object)DBNull.Value);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new ScheduleModel
                {
                    LichhocID = reader["MaID"]?.ToString(),

                    KhoahocID = reader["KhoahocID"]?.ToString(),
                    TenKhoahoc = reader["TenKhoahoc"]?.ToString(),

                    ChuyendeID = reader["ChuyendeID"]?.ToString(),
                    TenChuyende = reader["TenChuyende"]?.ToString(),

                    GiangvienID = reader["GiangvienID"]?.ToString(),
                    TenGiangvien = reader["TenGiangvien"]?.ToString(),

                    Ngaydukien = reader["Ngaydukien"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["Ngaydukien"]),

                    Batdaudukien = reader["Batdaudukien"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["Batdaudukien"]),

                    Ketthucdukien = reader["Ketthucdukien"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["Ketthucdukien"]),

                    Ngaythucte = reader["Ngaythucte"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["Ngaythucte"]),

                    Batdauthucte = reader["Batdauthucte"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["Batdauthucte"]),

                    Ketthucthucte = reader["Ketthucthucte"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["Ketthucthucte"])
                };
            }

            return null;
        }

        public async Task<List<ScheduleModel>> GetByStudentIDAsync(SqlConnection conn, string? HocvienID)
        {
            var query = @"SELECT lh.*, kh.Ten AS TenKhoahoc
                        FROM dbo.Lichhoc lh
                        JOIN dbo.Khoahoc kh ON lh.KhoahocID = kh.MaID
                        JOIN dbo.Khoahoc_Hocvien khhv ON khhv.KhoahocID = kh.MaID
                        WHERE khhv.HocvienID = @HocvienID";
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@HocvienID", HocvienID ?? (object)DBNull.Value);
                var Schedules = new List<ScheduleModel>();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Schedules.Add(new ScheduleModel
                        {
                            LichhocID = reader["MaID"]?.ToString(),

                            KhoahocID = reader["KhoahocID"]?.ToString(),
                            TenKhoahoc = reader["TenKhoahoc"]?.ToString(),

                            ChuyendeID = reader["ChuyendeID"]?.ToString(),
                            // TenChuyende = reader["TenChuyende"]?.ToString(),

                            GiangvienID = reader["GiangvienID"]?.ToString(),
                            // TenGiangvien = reader["TenGiangvien"]?.ToString(),

                            Ngaydukien = reader["Ngaydukien"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["Ngaydukien"]),

                            Batdaudukien = reader["Batdaudukien"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["Batdaudukien"]),

                            Ketthucdukien = reader["Ketthucdukien"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["Ketthucdukien"]),

                            Ngaythucte = reader["Ngaythucte"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["Ngaythucte"]),

                            Batdauthucte = reader["Batdauthucte"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["Batdauthucte"]),

                            Ketthucthucte = reader["Ketthucthucte"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["Ketthucthucte"])
                        });
                    }
                }
                return Schedules;
            }
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(SqlConnection conn, SqlTransaction trans, string? LichhocID, string? KhoahocID, string? ChuyendeID, string? GiangvienID,
                                DateTime? Ngaydukien, DateTime? Batdaudukien, DateTime? Ketthucdukien, DateTime? Ngaythucte, DateTime? Batdauthucte, DateTime? Ketthucthucte)
        {
            List<string> updates = new();

            using SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.Transaction = trans;

            if (KhoahocID != null)
            {
                updates.Add("KhoahocID = @KhoahocID");
                cmd.Parameters.AddWithValue("@KhoahocID", KhoahocID);
            }

            if (ChuyendeID != null)
            {
                updates.Add("ChuyendeID = @ChuyendeID");
                cmd.Parameters.AddWithValue("@ChuyendeID", ChuyendeID);
            }

            if (GiangvienID != null)
            {
                updates.Add("GiangvienID = @GiangvienID");
                cmd.Parameters.AddWithValue("@GiangvienID", GiangvienID);
            }

            if (Ngaydukien != null)
            {
                updates.Add("Ngaydukien = @Ngaydukien");
                cmd.Parameters.AddWithValue("@Ngaydukien", Ngaydukien);
            }

            if (Batdaudukien != null)
            {
                updates.Add("Batdaudukien = @Batdaudukien");
                cmd.Parameters.AddWithValue("@Batdaudukien", Batdaudukien);
            }

            if (Ketthucdukien != null)
            {
                updates.Add("Ketthucdukien = @Ketthucdukien");
                cmd.Parameters.AddWithValue("@Ketthucdukien", Ketthucdukien);
            }

            if (Ngaythucte != null)
            {
                updates.Add("Ngaythucte = @Ngaythucte");
                cmd.Parameters.AddWithValue("@Ngaythucte", Ngaythucte);
            }

            if (Batdauthucte != null)
            {
                updates.Add("Batdauthucte = @Batdauthucte");
                cmd.Parameters.AddWithValue("@Batdauthucte", Batdauthucte);
            }

            if (Ketthucthucte != null)
            {
                updates.Add("Ketthucthucte = @Ketthucthucte");
                cmd.Parameters.AddWithValue("@Ketthucthucte", Ketthucthucte);
            }

            // Không có field nào để update
            if (updates.Count == 0)
                return;

            string query = $@"
        UPDATE dbo.Lichhoc
        SET {string.Join(", ", updates)}
        WHERE MaID = @LichhocID";

            cmd.Parameters.AddWithValue("@LichhocID", LichhocID);

            cmd.CommandText = query;

            await cmd.ExecuteNonQueryAsync();
        }
    }
}