using Microsoft.Data.SqlClient;
using System.Data;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class ExamQuestionRepository : IExamQuestionRepository
    {
        public async Task CreateAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string? BaithuhoachID, string? KhoahocID, string? ChuyendeID, string? CauhoiID, string? Tencauhoi)
        {
            const string sql = @"
                INSERT INTO Baithuhoach_Cauhoi
                (
                    MaID,
                    BaithuhoachID,
                    KhoahocID,
                    ChuyendeID,
                    CauhoiID,
                    Tencauhoi
                )
                VALUES
                (
                    @MaID,
                    @BaithuhoachID,
                    @KhoahocID,
                    @ChuyendeID,
                    @CauhoiID,
                    @Tencauhoi
                )";

            using SqlCommand cmd = new(sql, conn, trans);

            cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BaithuhoachID", (object?)BaithuhoachID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@KhoahocID", (object?)KhoahocID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ChuyendeID", (object?)ChuyendeID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CauhoiID", (object?)CauhoiID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Tencauhoi", (object?)Tencauhoi ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
            // throw new NotImplementedException();
        }

        public async Task DeleteAsync(SqlConnection conn, SqlTransaction trans, string? MaID)
        {
            const string sql = @"
        DELETE FROM Baithuhoach_Cauhoi
        WHERE MaID = @MaID";

            using SqlCommand cmd = new(sql, conn, trans);

            cmd.Parameters.AddWithValue("@MaID", MaID);

            await cmd.ExecuteNonQueryAsync();
            // throw new NotImplementedException();
        }

        public async Task<List<ExamQuestionModel>> GetAllAsync(SqlConnection conn)
        {
            List<ExamQuestionModel> result = [];

            const string sql = @"
        SELECT *
        FROM Baithuhoach_Cauhoi";

            using SqlCommand cmd = new(sql, conn);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(new ExamQuestionModel
                {
                    MaID = reader["MaID"]?.ToString(),
                    Tencauhoi = reader["Tencauhoi"]?.ToString(),
                });
            }

            return result;
            throw new NotImplementedException();
        }

        public async Task<List<Question>> GetByCourseIDAsync(
    SqlConnection conn,
    string? KhoahocID)
        {
            var questions = new Dictionary<string, Question>();

            try
            {
                using var cmd = new SqlCommand("sp_KhoahocBaithuhoach", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@MaKhoahoc", SqlDbType.NVarChar, 50)
                    .Value = KhoahocID ?? (object)DBNull.Value;

                if (conn.State != ConnectionState.Open)
                {
                    await conn.OpenAsync();
                }

                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var cauhoiID = reader["MaCauhoi"]?.ToString();

                    if (string.IsNullOrEmpty(cauhoiID))
                        continue;

                    questions[cauhoiID] = new Question
                    {
                        CauhoiID = cauhoiID,
                        Ten = reader["TenCauhoi"]?.ToString(),
                        ChuyendeID = reader["MaChuyende"]?.ToString(),
                        questionAnswers = new List<QuestionAnswer>()
                    };
                }

                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var cauhoiID = reader["MaCauhoi"]?.ToString();

                        if (string.IsNullOrEmpty(cauhoiID))
                            continue;

                        if (questions.TryGetValue(cauhoiID, out var question))
                        {
                            question.questionAnswers.Add(new QuestionAnswer
                            {
                                CauhoiID = cauhoiID,
                                Ten = reader["TenDapan"]?.ToString(),
                                Dung = reader["Dung"] != DBNull.Value
                                    ? Convert.ToBoolean(reader["Dung"])
                                    : null
                            });
                        }
                    }
                }

                var result = new List<Question>();

                foreach (var question in questions.Values)
                {
                    result.Add(question);
                }

                return result;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ExamQuestion>> GetByExamIDAsync(
    SqlConnection conn,
    string? BaithuhoachID)
        {
            var questions = new List<ExamQuestion>();
            var questionMap = new Dictionary<string, ExamQuestion>();

            if (string.IsNullOrWhiteSpace(BaithuhoachID))
                return questions;

            using var cmd = new SqlCommand("sp_Baithuhoach_Cauhoi_Traloi", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@MaBaithuhoach", SqlDbType.NVarChar, 50).Value = BaithuhoachID;

            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var question = new ExamQuestion
                {
                    MaID = reader["MaID"] as string,
                    BaithuhoachID = reader["BaithuhoachID"] as string,
                    CauhoiID = reader["CauhoiID"] as string,
                    Tencauhoi = reader["Tencauhoi"] as string,
                    ChuyendeID = reader["ChuyendeID"] as string,
                    KhoahocID = reader["KhoahocID"] as string,
                    examAnswers = new List<ExamAnswers>()
                };

                questions.Add(question);

                if (!string.IsNullOrEmpty(question.MaID))
                    questionMap[question.MaID] = question;
            }

            await reader.NextResultAsync();

            while (await reader.ReadAsync())
            {
                var answer = new ExamAnswers
                {
                    MaID = reader["MaID"] as string,
                    BaithuhoachID = reader["BaithuhoachID"] as string,
                    Baithuhoach_CauhoiID = reader["Baithuhoach_CauhoiID"] as string,
                    NdTraloi = reader["NdTraloi"] as string,
                    Dung = reader["Dung"] == DBNull.Value ? null : Convert.ToBoolean(reader["Dung"]),
                    Chon = reader["Chon"] == DBNull.Value ? null : Convert.ToBoolean(reader["Chon"])
                };

                if (!string.IsNullOrEmpty(answer.Baithuhoach_CauhoiID)
                    && questionMap.TryGetValue(answer.Baithuhoach_CauhoiID, out var question))
                {
                    question.examAnswers!.Add(answer);
                }
            }

            return questions;
        }

        public async Task<ExamQuestionModel> GetByIDAsync(SqlConnection conn, string? MaID)
        {
            const string sql = @"
        SELECT *
        FROM Baithuhoach_Cauhoi
        WHERE MaID = @MaID";

            using SqlCommand cmd = new(sql, conn);

            cmd.Parameters.AddWithValue("@MaID", MaID);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new ExamQuestionModel
                {
                    MaID = reader["MaID"]?.ToString(),
                    Tencauhoi = reader["Tencauhoi"]?.ToString(),
                };
            }

            return null!;
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string? BaithuhoachID, string? KhoahocID, string? ChuyendeID, string? CauhoiID, string? Tencauhoi, string? TraloiID, string? NdTraloi, string? DapanID, string? Nddapan, bool? Dung)
        {
            const string sql = @"
        UPDATE Baithuhoach_Cauhoi
        SET
            BaithuhoachID = @BaithuhoachID,
            KhoahocID = @KhoahocID,
            ChuyendeID = @ChuyendeID,
            CauhoiID = @CauhoiID,
            Tencauhoi = @Tencauhoi,
            TraloiID = @TraloiID,
            NdTraloi = @NdTraloi,
            DapanID = @DapanID,
            Nddapan = @Nddapan,
            Dung = @Dung
        WHERE MaID = @MaID";

            using SqlCommand cmd = new(sql, conn, trans);

            cmd.Parameters.AddWithValue("@MaID", MaID);
            cmd.Parameters.AddWithValue("@BaithuhoachID", (object?)BaithuhoachID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@KhoahocID", (object?)KhoahocID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ChuyendeID", (object?)ChuyendeID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CauhoiID", (object?)CauhoiID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Tencauhoi", (object?)Tencauhoi ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@TraloiID", (object?)TraloiID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@NdTraloi", (object?)NdTraloi ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DapanID", (object?)DapanID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Nddapan", (object?)Nddapan ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Dung", (object?)Dung ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
            // throw new NotImplementedException();
        }
    }

}
