using Microsoft.Data.SqlClient;

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
                FROM dbo.Lichhoc";

            using var cmd = new SqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            var list = new List<ScheduleModel>();

            while (await reader.ReadAsync())
            {
                list.Add(new ScheduleModel
                {
                    LichhocID = reader["MaID"]?.ToString(),
                    KhoahocID = reader["KhoahocID"]?.ToString(),
                    ChuyendeID = reader["ChuyendeID"]?.ToString(),
                    GiangvienID = reader["GiangvienID"]?.ToString(),

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
                FROM dbo.Lichhoc
                WHERE MaID = @LichhocID";

            using var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@LichhocID", LichhocID ?? (object)DBNull.Value);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new ScheduleModel
                {
                    LichhocID = reader["MaID"]?.ToString(),
                    KhoahocID = reader["KhoahocID"]?.ToString(),
                    ChuyendeID = reader["ChuyendeID"]?.ToString(),
                    GiangvienID = reader["GiangvienID"]?.ToString(),

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

        public async Task UpdateAsync(
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
                UPDATE dbo.Lichhoc
                SET
                    KhoahocID = @KhoahocID,
                    ChuyendeID = @ChuyendeID,
                    GiangvienID = @GiangvienID,
                    Ngaydukien = @Ngaydukien,
                    Batdaudukien = @Batdaudukien,
                    Ketthucdukien = @Ketthucdukien,
                    Ngaythucte = @Ngaythucte,
                    Batdauthucte = @Batdauthucte,
                    Ketthucthucte = @Ketthucthucte
                WHERE MaID = @LichhocID";

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
    }
}