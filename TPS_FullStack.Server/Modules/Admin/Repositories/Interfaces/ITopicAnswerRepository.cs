using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ITopicAnswerRepository
    {
        public Task<List<TopicAnswerModel>> GetAllAsync(SqlConnection conn);
        public Task<TopicAnswerModel> GetByIdAsync(SqlConnection conn, string? MaID, string ChuyendeID);
        public Task<List<TopicAnswerModel>> GetByTopicIdAsync(SqlConnection conn, string? ChuyendeID);
        public Task CreateAsync(SqlConnection conn, SqlTransaction trans, string MaID, string ChuyendeID, string Chuyende_CauhoiID,string Ten, bool Dung);
        public Task UpdateAsync(SqlConnection conn, SqlTransaction trans, string MaID, string ChuyendeID, string Chuyende_CauhoiID,string Ten, bool Dung);
        public Task DeleteAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string ChuyendeID, string Chuyende_CauhoiID);
    }

}

