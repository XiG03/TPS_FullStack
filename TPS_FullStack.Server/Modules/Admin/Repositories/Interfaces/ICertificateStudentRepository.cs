using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ICertificateStudentRepository
    {
        public Task<List<CertificateStudentModel>> GetAllAsync(SqlConnection conn);
        public Task<CertificateStudentModel> GetByIDAsync(SqlConnection conn, string? MaID);
        public Task<List<CertificateStudentModel>> GetByStudentIDAsync(SqlConnection conn, string? HocvienID);
        public Task CreateAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string? ChungchiID, string? HocvienID, string? Mota, string? Donvicap, DateTime? Ngaycap, DateTime? Ngayhethan,
                                DateTime? CreatedAt, string? CreatedBy, DateTime? UpdatedAt, string? UpdatedBy, DateTime? DeletedAt, string? DeletedBy);
        public Task UpdateAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string? ChungchiID, string? HocvienID, string? Mota, string? Donvicap, DateTime? Ngaycap, DateTime? Ngayhethan,
                                DateTime? CreatedAt, string? CreatedBy, DateTime? UpdatedAt, string? UpdatedBy, DateTime? DeletedAt, string? DeletedBy);
        public Task DeleteAsync(SqlConnection conn, SqlTransaction trans, string? MaID);
    }

}

