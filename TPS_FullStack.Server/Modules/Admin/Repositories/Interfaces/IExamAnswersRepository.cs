using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public interface IExamAnswersRepository
    {
        public Task CreateAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string? BaithuhoachID, string? Baithuhoach_CauhoiID, string? NdTraloi, bool? Dung, bool? Chon);
        public Task UpdateAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string? BaithuhoachID, string? Baithuhoach_CauhoiID, string? NdTraloi, bool? Dung, bool? Chon);
    }
}
