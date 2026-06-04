using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public interface IAddressRepository
    {
        public Task<List<AddressModel>> GetAllAsync(SqlConnection conn);
        public Task<AddressModel> GetByIDAsync(SqlConnection conn,string MaID);
        public Task CreateAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string? Ten, string? Diachi, string? Phonghoc);
        public Task UpdateAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string? Ten, string? Diachi, string? Phonghoc);
        public Task DeleteAsync(SqlConnection conn, SqlTransaction trans, string? MaID);
    }
}
