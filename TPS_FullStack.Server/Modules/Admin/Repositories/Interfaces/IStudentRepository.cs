using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public interface IStudentRepository
    {
        public Task CreateAsync(SqlConnection conn, SqlTransaction trans, string? HocvienID, string? Hoten, DateTime? Ngaysinh, string? Gioitinh, string? Email, string? Diachi, string? Dienthoai,
                                DateTime? CreatedAt, string? CreatedBy, DateTime? UpdatedAt, string? UpdatedBy, DateTime? DeletedAt, string? DeletedBy);
        public Task<List<StudentModel>> GetAllAsync(SqlConnection conn);
        public Task<StudentModel> GetByIDAsync(SqlConnection conn, string? MaID);
        public Task UpdateAsync(SqlConnection conn, SqlTransaction trans, string? HocvienID, string? Hoten, DateTime? Ngaysinh, string? Gioitinh, string? Email, string? Diachi, string? Dienthoai,
                                DateTime? CreatedAt, string? CreatedBy, DateTime? UpdatedAt, string? UpdatedBy, DateTime? DeletedAt, string? DeletedBy);
        public Task DeleteAsync(SqlConnection conn, SqlTransaction trans, string HocvienID, DateTime? DeletedAt, string? DeletedBy);
    }

}

