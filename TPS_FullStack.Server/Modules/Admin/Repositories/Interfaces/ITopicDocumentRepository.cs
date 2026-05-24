using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ITopicDocumentRepository
    {
        public Task<List<TopicDocumentModel>> GetAllAsync(SqlConnection conn);
        public Task<TopicDocumentModel> GetByIdAsync(SqlConnection conn, string? MaID, string ChuyendeID);
        public Task CreateAsync(SqlConnection conn, SqlTransaction trans, string MaID, string ChuyendeID, string Tieude, DateTime Ngaytao, string Loaitailieu, decimal Kichthuoc);
        public Task UpdateAsync(SqlConnection conn, SqlTransaction trans, string MaID, string ChuyendeID, string Tieude, DateTime Ngaytao, string Loaitailieu, decimal Kichthuoc);
        public Task DeleteAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string ChuyendeID);
    }

}

