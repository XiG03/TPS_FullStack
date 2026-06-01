using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class TopicDocumentRepository : ITopicDocumentRepository
    {
        private static async Task EnsureFileColumnsAsync(SqlConnection conn, SqlTransaction? trans = null)
        {
            var query = @"
IF COL_LENGTH('dbo.Chuyende_Tailieu', 'Duongdan') IS NULL
BEGIN
    ALTER TABLE dbo.Chuyende_Tailieu ADD Duongdan NVARCHAR(500) NULL
END

IF COL_LENGTH('dbo.Chuyende_Tailieu', 'TentepGoc') IS NULL
BEGIN
    ALTER TABLE dbo.Chuyende_Tailieu ADD TentepGoc NVARCHAR(255) NULL
END";

            using var cmd = trans == null ? new SqlCommand(query, conn) : new SqlCommand(query, conn, trans);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task CreateAsync(SqlConnection conn, SqlTransaction trans, string MaID, string ChuyendeID, string Tieude, DateTime Ngaytao, string Loaitailieu, decimal Kichthuoc, string? Duongdan, string? TentepGoc)
        {
            await EnsureFileColumnsAsync(conn, trans);

            var queryCreate = @"INSERT INTO dbo.Chuyende_Tailieu (MaID, ChuyendeID, Tieude, Ngaytao, Loaitailieu, Kichthuoc, Duongdan, TentepGoc)
                                VALUES (@MaID, @ChuyendeID, @Tieude, @Ngaytao, @Loaitailieu, @Kichthuoc, @Duongdan, @TentepGoc)";
            using (var cmd = new SqlCommand(queryCreate, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ChuyendeID", (object?)ChuyendeID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Tieude", (object?)Tieude ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Ngaytao", (object?)Ngaytao ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Loaitailieu", (object?)Loaitailieu ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Kichthuoc", (object?)Kichthuoc ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Duongdan", (object?)Duongdan ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TentepGoc", (object?)TentepGoc ?? DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }
            // throw new NotImplementedException();
        }

        public async Task DeleteAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string ChuyendeID)
        {
            var queryDelete = @"DELETE dbo.Chuyende_Tailieu
                                WHERE
                                    (@MaID IS NOT NULL AND MaID = @MaID)
                                    OR
                                    (@MaID IS NULL AND @ChuyendeID IS NOT NULL AND ChuyendeID = @ChuyendeID)";
            using (var cmd = new SqlCommand(queryDelete, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ChuyendeID", (object?)ChuyendeID ?? DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }
            // throw new NotImplementedException();
        }

        public async Task<List<TopicDocumentModel>> GetAllAsync(SqlConnection conn)
        {
            await EnsureFileColumnsAsync(conn);

            var queryGetAll = @"SELECT MaID, ChuyendeID, Tieude, Ngaytao, Loaitailieu, Kichthuoc, Duongdan, TentepGoc
                                FROM dbo.Chuyende_Tailieu";
            using (var cmd = new SqlCommand(queryGetAll, conn))
            {
                using var reader = await cmd.ExecuteReaderAsync();

                var result = new List<TopicDocumentModel>();

                while (await reader.ReadAsync())
                {
                    result.Add(new TopicDocumentModel
                    {
                        MaID = reader["MaID"].ToString(),
                        ChuyendeID = reader["ChuyendeID"].ToString(),
                        Tieude = reader["Tieude"].ToString(),
                        Ngaytao = Convert.ToDateTime(reader["Ngaytao"]),
                        Loaitailieu = reader["Loaitailieu"].ToString(),
                        Kichthuoc = Convert.ToDecimal(reader["Kichthuoc"]),
                        Duongdan = reader["Duongdan"] == DBNull.Value ? null : reader["Duongdan"].ToString(),
                        TentepGoc = reader["TentepGoc"] == DBNull.Value ? null : reader["TentepGoc"].ToString()
                    });
                }
                return result;
            }
            // throw new NotImplementedException();
        }

        public async Task<TopicDocumentModel> GetByIdAsync(SqlConnection conn, string? MaID, string? ChuyendeID)
        {
            await EnsureFileColumnsAsync(conn);

            var queryGetById = @"SELECT Top 1 * FROM dbo.Chuyende_Tailieu
                                WHERE (@MaID IS NOT NULL AND MaID = @MaID) OR (@MaID IS NULL AND @ChuyendeID IS NOT NULL AND @ChuyendeID = ChuyendeID)";
            using (var cmd = new SqlCommand(queryGetById, conn))
            {
                cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ChuyendeID", (object?)ChuyendeID ?? DBNull.Value);

                using var reader = await cmd.ExecuteReaderAsync();
                var result = new TopicDocumentModel();
                if (await reader.ReadAsync())
                {
                    result.MaID = MaID = reader["MaID"].ToString();
                    result.ChuyendeID = reader["ChuyendeID"].ToString();
                    result.Tieude = reader["Tieude"].ToString();
                    result.Ngaytao = Convert.ToDateTime(reader["Ngaytao"]);
                    result.Loaitailieu = reader["Loaitailieu"].ToString();
                    result.Kichthuoc = Convert.ToDecimal(reader["Kichthuoc"]);
                    result.Duongdan = reader["Duongdan"] == DBNull.Value ? null : reader["Duongdan"].ToString();
                    result.TentepGoc = reader["TentepGoc"] == DBNull.Value ? null : reader["TentepGoc"].ToString();
                }
                else
                {
                    return null;
                }
                return result;
            }
            // throw new NotImplementedException();
        }

        public async Task UpdateAsync(SqlConnection conn, SqlTransaction trans, string MaID, string ChuyendeID, string Tieude, DateTime Ngaytao, string Loaitailieu, decimal Kichthuoc, string? Duongdan, string? TentepGoc)
        {
            await EnsureFileColumnsAsync(conn, trans);

            var queryUpdate = @"UPDATE dbo.Chuyende_Tailieu
                                    SET
                                        Tieude = @Tieude,
                                        Ngaytao = @Ngaytao,
                                        Loaitailieu = @Loaitailieu,
                                        Kichthuoc = @Kichthuoc,
                                        Duongdan = COALESCE(@Duongdan, Duongdan),
                                        TentepGoc = COALESCE(@TentepGoc, TentepGoc)
                                WHERE (@MaID IS NOT NULL AND MaID = @MaID) OR (@MaID IS NULL AND @ChuyendeID IS NOT NULL AND @ChuyendeID = ChuyendeID)";
            using (var cmd = new SqlCommand(queryUpdate, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ChuyendeID", (object?)ChuyendeID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Tieude", (object?)Tieude ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Ngaytao", (object?)Ngaytao ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Loaitailieu", (object?)Loaitailieu ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Kichthuoc", (object?)Kichthuoc ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Duongdan", (object?)Duongdan ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TentepGoc", (object?)TentepGoc ?? DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }
            // throw new NotImplementedException();
        }
    }

}

