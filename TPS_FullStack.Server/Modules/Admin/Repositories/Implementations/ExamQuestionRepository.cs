using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class ExamQuestionRepository : IExamQuestionRepository
    {
        public async Task CreateAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string? BaithuhoachID, string? KhoahocID, string? ChuyendeID, string? CauhoiID, string? Tencauhoi, string? TraloiID, string? NdTraloi, string? DapanID, string? Nddapan, bool? Dung)
        {
            const string sql = @"
                INSERT INTO Baithuhoach_Cauhoi
                (
                    MaID,
                    BaithuhoachID,
                    KhoahocID,
                    ChuyendeID,
                    CauhoiID,
                    Tencauhoi,
                    TraloiID,
                    NdTraloi,
                    DapanID,
                    Nddapan,
                    Dung
                )
                VALUES
                (
                    @MaID,
                    @BaithuhoachID,
                    @KhoahocID,
                    @ChuyendeID,
                    @CauhoiID,
                    @Tencauhoi,
                    @TraloiID,
                    @NdTraloi,
                    @DapanID,
                    @Nddapan,
                    @Dung
                )";

            using SqlCommand cmd = new(sql, conn, trans);

            cmd.Parameters.AddWithValue("@MaID", (object?)MaID ?? DBNull.Value);
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
