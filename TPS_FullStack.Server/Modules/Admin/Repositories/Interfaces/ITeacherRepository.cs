using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ITeacherRepository
    {
        public Task CreateAsync(SqlConnection conn, SqlTransaction trans,string? GiangvienID, string? Hoten, DateTime? Ngaysinh, string? Gioitinh, string? Email, string? Diachi, string? Dienthoai,
                                DateTime? CreatedAt, string? CreatedBy, DateTime? UpdatedAt, string? UpdatedBy,DateTime? DeletedAt, string? DeletedBy);
        public Task<List<TeacherModel>> GetAllAsync(SqlConnection conn);
        public Task<TeacherModel> GetByIDAsync(SqlConnection conn, string? MaID);
        public Task UpdateAsync(SqlConnection conn, SqlTransaction trans,string? GiangvienID, string? Hoten, DateTime? Ngaysinh, string? Gioitinh, string? Email, string? Diachi, string? Dienthoai,
                                DateTime? CreatedAt, string? CreatedBy, DateTime? UpdatedAt, string? UpdatedBy,DateTime? DeletedAt, string? DeletedBy);
        public Task DeleteAsync(SqlConnection conn, SqlTransaction trans, string GiangvienID, DateTime? DeletedAt, string? DeletedBy);
    }

}

