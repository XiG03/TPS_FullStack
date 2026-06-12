using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public interface IExamQuestionRepository
    {
        public Task<List<ExamQuestionModel>> GetAllAsync(SqlConnection conn);
        public Task<ExamQuestionModel> GetByIDAsync(SqlConnection conn, string? MaID);
        public Task CreateAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string? BaithuhoachID, string? KhoahocID, string? ChuyendeID,
                                string? CauhoiID, string? Tencauhoi);
        public Task UpdateAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string? BaithuhoachID, string? KhoahocID, string? ChuyendeID,
                                string? CauhoiID, string? Tencauhoi, string? TraloiID, string? NdTraloi, string? DapanID, string? Nddapan, bool? Dung);
        public Task DeleteAsync(SqlConnection conn, SqlTransaction trans, string? MaID);

        public Task<List<Question>> GetByCourseIDAsync(SqlConnection conn, string? KhoahocID);
        public Task<List<ExamQuestion>> GetByExamIDAsync(SqlConnection conn, string? BaithuhoachID);
    }

}
