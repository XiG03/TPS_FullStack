using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ICoursesRepository
    {
        public Task<List<CourseModel>> GetAllAsync(SqlConnection conn);
        public Task<CourseModel> GetByIDAsync(SqlConnection conn, string? KhoahocID, string? ChungchiID);
        public Task CreateAsync(SqlConnection conn, SqlTransaction trans, string? KhoahocID, string? Ten, string? Mota, decimal? Diemdat,
                                string? ChungchiID, string? Thu, decimal? Thoiluonghoc, decimal? Sobuoihoc, decimal? Thoiluongthi, DateTime? Ngaybatdau, decimal? Socauhoi,
                                DateTime? CreatedAt, string? CreatedBy, DateTime? UpdatedAt, string? UpdatedBy, DateTime? DeletedAt, string? DeletedBy);
        public Task UpdateAsync(SqlConnection conn, SqlTransaction trans, string? KhoahocID, string? Ten, string? Mota, decimal? Diemdat,
                                string? ChungchiID, string? Thu, decimal? Thoiluonghoc, decimal? Sobuoihoc, decimal? Thoiluongthi, DateTime? Ngaybatdau, decimal? Socauhoi,
                                DateTime? CreatedAt, string? CreatedBy, DateTime? UpdatedAt, string? UpdatedBy, DateTime? DeletedAt, string? DeletedBy);
        public Task UpdateCourseIDAsync (SqlConnection conn, SqlTransaction trans, string? KhoahocID, string? ChungchiID, DateTime? UpdatedAt, string? UpdatedBy);
        public Task DeleteAsync(SqlConnection conn, SqlTransaction trans, string? KhoahocID, DateTime? DeletedAt, string? DeletedBy);
        
    }

}

