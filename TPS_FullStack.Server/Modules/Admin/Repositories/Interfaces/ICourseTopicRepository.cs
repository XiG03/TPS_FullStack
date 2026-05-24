using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ICourseTopicRepository
    {
        public Task CreateAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string? KhoahocID, string? ChuyendeID, decimal? Socauhoi);
        public Task<List<CourseTopicModel>> GetAllAsync (SqlConnection conn);
        public Task<CourseTopicModel> GetByIDAsync(SqlConnection conn, string MaID);
        public Task UpdateAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string? KhoahocID, string? ChuyendeID, decimal? Socauhoi);
        public Task DeleteAsync(SqlConnection conn, SqlTransaction trans, string? MaID);
    }

}

