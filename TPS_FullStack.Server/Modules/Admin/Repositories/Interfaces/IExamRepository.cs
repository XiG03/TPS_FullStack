using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public interface IExamRepository
    {
        public Task<List<ExamModel>> GetAllAsync(SqlConnection conn);
        public Task<ExamModel> GetByIDAsync(SqlConnection conn, string? MaID);
        public Task CreateAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string? KhoahocID, string? HocvienID, DateTime? Batdauthi, DateTime? Ketthucthi,
                                DateTime? CreatedAt, string? CreatedBy, DateTime? UpdatedAt, string? UpdatedBy, DateTime? DeletedAt, string? DeletedBy);
        public Task UpdateAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string? KhoahocID, string? HocvienID, DateTime? Batdauthi, DateTime? Ketthucthi,
                                DateTime? CreatedAt, string? CreatedBy, DateTime? UpdatedAt, string? UpdatedBy, DateTime? DeletedAt, string? DeletedBy);
        public Task DeleteAsync(SqlConnection conn, SqlTransaction trans, string? MaID);
    }

}

