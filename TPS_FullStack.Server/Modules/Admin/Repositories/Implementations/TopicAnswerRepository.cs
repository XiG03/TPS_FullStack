using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class TopicAnswerRepository : ITopicAnswerRepository
    {
        public async Task CreateAsync(SqlConnection conn, SqlTransaction trans, string MaID, string ChuyendeID, string Chuyende_CauhoiID, string Ten, bool Dung)
        {
            var queryCreate = @"INSERT INTO dbo.Chuyende_Dapan (MaID, ChuyendeID, Chuyende_CauhoiID, Ten, Dung)
                                VALUES (@MaID, @ChuyendeID, @Chuyende_CauhoiID, @Ten, @Dung)";
            using (var cmd = new SqlCommand(queryCreate, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ChuyendeID", (object?)ChuyendeID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Chuyende_CauhoiID", (object?)Chuyende_CauhoiID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Ten", (object?)Ten ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Dung", (object?)Dung ?? DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }
            // throw new NotImplementedException();
        }

        public async Task DeleteAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string ChuyendeID, string Chuyende_CauhoiID)
        {
            var queryDelete = @"DELETE dbo.Chuyende_Dapan
                                WHERE (@MaID IS NOT NULL AND MaID = @MaID) OR (@ChuyendeID IS NOT NULL AND @ChuyendeID = ChuyendeID)";
            using (var cmd = new SqlCommand(queryDelete, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ChuyendeID", (object?)ChuyendeID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Chuyende_CauhoiID", (object?)Chuyende_CauhoiID ?? DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }
            // throw new NotImplementedException();
        }

        public async Task<List<TopicAnswerModel>> GetAllAsync(SqlConnection conn)
        {
            var queryGetAll = @"SELECT MaID, ChuyendeID, Chuyende_CauhoiID, Ten, Dung
                                FROM dbo.Chuyende_Dapan";
            using (var cmd = new SqlCommand(queryGetAll, conn))
            {
                using var reader = await cmd.ExecuteReaderAsync();

                var result = new List<TopicAnswerModel>();

                while (await reader.ReadAsync())
                {
                    result.Add(new TopicAnswerModel
                    {
                        MaID = reader["MaID"].ToString(),
                        ChuyendeID = reader["ChuyendeID"].ToString(),
                        Chuyende_CauhoiID = reader["Chuyende_CauhoiID"].ToString(),
                        Ten = reader["Ten"].ToString(),
                        Dung = Convert.ToBoolean(reader["Dung"])
                    });
                }
                return result;
            }
            // throw new NotImplementedException();
        }

        public async Task<TopicAnswerModel> GetByIdAsync(SqlConnection conn, string? MaID, string ChuyendeID)
        {
            var queryGetById = @"SELECT MaID, ChuyendeID, Chuyende_CauhoiID, Ten, Dung FROM dbo.Chuyende_Dapan
                                WHERE (@MaID IS NOT NULL AND MaID = @MaID) OR (@ChuyendeID IS NOT NULL AND @ChuyendeID = ChuyendeID)";
            using (var cmd = new SqlCommand(queryGetById, conn))
            {
                cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ChuyendeID", (object?)ChuyendeID ?? DBNull.Value);

                using var reader = await cmd.ExecuteReaderAsync();
                var result = new TopicAnswerModel();
                if (await reader.ReadAsync())
                {
                    result.MaID = MaID = reader["MaID"].ToString();
                    result.ChuyendeID = reader["ChuyendeID"].ToString();
                    result.Chuyende_CauhoiID = reader["Chuyende_CauhoiID"].ToString();
                    result.Ten = reader["Ten"].ToString();
                    result.Dung = Convert.ToBoolean(reader["Dung"]);
                }
                else
                {
                    return null;
                }
                return result;
            }
            // throw new NotImplementedException();
        }

        public async Task<List<TopicAnswerModel>> GetByTopicIdAsync(SqlConnection conn, string? ChuyendeID)
        {
            var query = @"SELECT MaID, ChuyendeID, Chuyende_CauhoiID, Ten, Dung FROM dbo.Chuyende_Dapan
                                WHERE  ChuyendeID = @ChuyendeID";
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ChuyendeID", (object?)ChuyendeID ?? DBNull.Value);
                using var reader = await cmd.ExecuteReaderAsync();
                var result = new List<TopicAnswerModel>();
                while (await reader.ReadAsync())
                {
                    result.Add(new TopicAnswerModel
                    {
                        MaID = reader["MaID"].ToString(),
                        ChuyendeID = reader["ChuyendeID"].ToString(),
                        Chuyende_CauhoiID = reader["Chuyende_CauhoiID"].ToString(),
                        Ten = reader["Ten"].ToString(),
                        Dung = Convert.ToBoolean(reader["Dung"])
                    });
                }
            }
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(SqlConnection conn, SqlTransaction trans, string MaID, string ChuyendeID, string Chuyende_CauhoiID, string Ten, bool Dung)
        {
            var queryUpdate = @"UPDATE dbo.Chuyende_Dapan
                                SET
                                    Ten = @Ten,
                                    Dung = @Dung
                                WHERE (@MaID IS NOT NULL AND MaID = @MaID) OR (@ChuyendeID IS NOT NULL AND @ChuyendeID = ChuyendeID)";

            using (var cmd = new SqlCommand(queryUpdate, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ChuyendeID", (object?)ChuyendeID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Chuyende_CauhoiID", (object?)Chuyende_CauhoiID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Ten", (object?)Ten ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Dung", (object?)Dung ?? DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }
            // throw new NotImplementedException();
        }
    }

}

