using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ITopicRepository
    {
        public Task<List<TopicModel>> GetAllAsync(SqlConnection conn);
        public Task<TopicModel> GetByIdAsync(SqlConnection conn, string? MaID);
        public Task CreateAsync(SqlConnection conn, SqlTransaction trans, string MaID, string Ten, string Mota, bool Khongsudung, DateTime? CreatedAt, string? CreatedBy, DateTime? UpdatedAt, string? UpdatedBy,DateTime? DeletedAt, string? DeletedBy);
        public Task UpdateAsync(SqlConnection conn, SqlTransaction trans, string MaID, string Ten, string Mota, DateTime UpdatedAt, string UpdatedBy);
        public Task DeleteAsync(SqlConnection conn, SqlTransaction trans, string? MaID, DateTime DeletedAt, string DeletedBy);
    }

}

