using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class TopicQuestionRepository : ITopicQuestionRepository
    {
        public async Task CreateAsync(SqlConnection conn, SqlTransaction trans, string MaID, string ChuyendeID, string Ten)
        {
            var queryCreate = @"INSERT INTO dbo.Chuyende_Cauhoi(MaID, ChuyendeID, Ten)
                                VALUES (@MaID, @ChuyendeID, @Ten)";
            using (var cmd = new SqlCommand(queryCreate, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ChuyendeID", (object?)ChuyendeID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Ten", (object?)Ten ?? DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }
            // throw new NotImplementedException();
        }

        public async Task DeleteAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string ChuyendeID)
        {
            var queryDelete = @"DELETE dbo.Chuyende_Cauhoi
                                WHERE
                                    (@MaID IS NOT NULL AND MaID = @MaID)
                                    OR
                                    (@ChuyendeID IS NOT NULL AND ChuyendeID = @ChuyendeID)";
            using (var cmd = new SqlCommand(queryDelete, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ChuyendeID", (object?)ChuyendeID ?? DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }
            // throw new NotImplementedException();
        }

        public async Task<List<QuestionTopicModel>> GetAllAsync(SqlConnection conn)
        {
            var queryGetAll = @"SELECT MaID, ChuyendeID, Ten FROM dbo.Chuyende_Cauhoi";
            using (var cmd = new SqlCommand(queryGetAll, conn))
            {
                using var reader = await cmd.ExecuteReaderAsync();

                var result = new List<QuestionTopicModel>();

                while (await reader.ReadAsync())
                {
                    result.Add(new QuestionTopicModel
                    {
                        MaID = reader["MaID"].ToString(),
                        ChuyendeID = reader["ChuyendeID"].ToString(),
                        Ten = reader["Ten"].ToString(),
                    });
                }
                return result;

            }
            // throw new NotImplementedException();
        }

        public async Task<QuestionTopicModel> GetByIdAsync(SqlConnection conn, string? MaID, string ChuyendeID)
        {
            var queryGetById = @"SELECT Top 1 * FROM dbo.Chuyende_Cauhoi
                                WHERE (@MaID IS NOT NULL AND MaID = @MaID) OR (@ChuyendeID IS NOT NULL AND @ChuyendeID = ChuyendeID)";
            using (var cmd = new SqlCommand(queryGetById, conn))
            {
                cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ChuyendeID", (object?)ChuyendeID ?? DBNull.Value);

                using var reader = await cmd.ExecuteReaderAsync();
                var result = new QuestionTopicModel();
                if (await reader.ReadAsync())
                {
                    result.MaID = MaID = reader["MaID"].ToString();
                    result.ChuyendeID = reader["ChuyendeID"].ToString();
                    result.Ten = reader["Ten"].ToString();
                }
                else
                {
                    return null;
                }
                return result;
            }
            // throw new NotImplementedException();
        }

        public async Task UpdateAsync(SqlConnection conn, SqlTransaction trans, string MaID, string ChuyendeID, string Ten)
        {
            var queryUpdate = @"UPDATE dbo.Chuyende_Cauhoi
                                    SET
                                        Ten = @Ten
                                WHERE (@MaID IS NOT NULL AND MaID = @MaID) OR (@ChuyendeID IS NOT NULL AND @ChuyendeID = ChuyendeID)";
            using (var cmd = new SqlCommand(queryUpdate, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ChuyendeID", (object?)ChuyendeID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Ten", (object?)Ten ?? DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }
            // throw new NotImplementedException();
        }
    }
}


