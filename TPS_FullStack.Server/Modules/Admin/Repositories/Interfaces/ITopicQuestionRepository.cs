using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ITopicQuestionRepository
    {
        public Task<List<QuestionTopicModel>> GetAllAsync(SqlConnection conn);
        public Task<QuestionTopicModel> GetByIdAsync(SqlConnection conn, string? MaID, string ChuyendeID);
        public Task<List<QuestionTopicModel>> GetByTopicIdAsync(SqlConnection conn, string? ChuyendeID); 
        public Task CreateAsync(SqlConnection conn, SqlTransaction trans, string MaID, string ChuyendeID, string Ten);
        public Task UpdateAsync(SqlConnection conn, SqlTransaction trans, string MaID, string ChuyendeID, string Ten);
        public Task DeleteAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string ChuyendeID);
    }

}

