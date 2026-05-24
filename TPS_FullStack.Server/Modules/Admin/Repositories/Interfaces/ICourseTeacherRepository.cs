using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ICourseTeacherRepository
    {
        public Task<List<CourseTeacherModel>> GetAllAsync (SqlConnection conn);
        public Task<CourseTeacherModel> GetByIDAsync (SqlConnection conn, string? MaID);
        public Task CreateAsync (SqlConnection conn, SqlTransaction trans, string? MaID, string? KhoahocID, string? GiangvienID);
        public Task UpdateAsync (SqlConnection conn, SqlTransaction trans, string? MaID, string? KhoahocID, string? GiangvienID);
        public Task DeleteAsync (SqlConnection conn, SqlTransaction trans, string? MaID);
    }

}

