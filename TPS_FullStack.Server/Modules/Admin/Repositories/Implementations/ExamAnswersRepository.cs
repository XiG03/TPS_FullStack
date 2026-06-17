using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class ExamAnswersRepository : IExamAnswersRepository
    {
        public async Task CreateAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string? BaithuhoachID, string? Baithuhoach_CauhoiID, string? NdTraloi, bool? Dung, bool? Chon)
        {
            var query = @"
                INSERT INTO Baithuhoach_Traloi (MaID, BaithuhoachID, Baithuhoach_CauhoiID, NdTraloi, Dung, Chon)
                VALUES (@MaID, @BaithuhoachID, @Baithuhoach_CauhoiID, @NdTraloi, @Dung, @Chon)";
            using (var cmd = new SqlCommand(query, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaID",
        (object?)MaID ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@BaithuhoachID",
                    (object?)BaithuhoachID ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@Baithuhoach_CauhoiID",
                    (object?)Baithuhoach_CauhoiID ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@NdTraloi",
                    (object?)NdTraloi ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@Dung",
                    (object?)Dung ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@Chon",
                    (object?)Chon ?? DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }
            //throw new NotImplementedException();
        }

        public async Task UpdateAsync( SqlConnection conn, SqlTransaction trans, string? MaID, string? BaithuhoachID, string? Baithuhoach_CauhoiID, string? NdTraloi, bool? Dung, bool? Chon)
        {
            var query = @"UPDATE Baithuhoach_Traloi
                            SET Chon = @Chon
                            WHERE MaID = @MaID";

            using (var cmd = new SqlCommand(query, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaID", MaID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Chon", Chon ?? (object)DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
