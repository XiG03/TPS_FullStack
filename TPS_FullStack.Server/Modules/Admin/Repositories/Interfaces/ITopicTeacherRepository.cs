using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ITopicTeacherRepository
    {
        public Task CreateAsync (SqlConnection conn, SqlTransaction trans, 
                                string? MaID, string? GiangvienID, string? ChuyendeID);
        public Task<TopicTeacherModel> GetByIDAsync (SqlConnection conn, string? MaID);
        public Task <List<TopicTeacherModel>> GetAllAsync(SqlConnection conn);
        public Task UpdateAsync(SqlConnection conn, SqlTransaction trans, 
                                string? MaID, string? GiangvienID, string? ChuyendeID);
        public Task DeleteAsync(SqlConnection conn, SqlTransaction trans,
                                string? MaID);

        // Phan them cho chuc nang giang vien
        public Task <List<TopicsTeacherModel>> GetByTeacherIDAsync (SqlConnection conn, string? GiangvienID);
    }

}

