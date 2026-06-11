using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ICourseStudentRepository
    {
        public Task<List<CourseStudentModel>> GetAllAsync (SqlConnection conn);
        public Task<CourseStudentModel> GetByIDAsync (SqlConnection conn, string? MaID);
        public Task CreateAsync (SqlConnection conn, SqlTransaction trans, string? MaID, string? KhoahocID, string? HocvienID, decimal? Diem, decimal? Dieuchinh);
        public Task UpdateAsync (SqlConnection conn, SqlTransaction trans, string? MaID, string? KhoahocID, string? HocvienID, decimal? Diem, decimal? Dieuchinh);
        public Task DeleteAsync (SqlConnection conn, SqlTransaction trans, string? MaID);
        public Task<CourseStudentModel> CheckStudentByCourseIDAsync(SqlConnection conn, string? KhoahocID, string? HocvienID);
    }

}

