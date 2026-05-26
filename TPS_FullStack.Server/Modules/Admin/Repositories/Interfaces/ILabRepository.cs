using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ILabRepository
    {
        public Task CreateAsync(SqlConnection conn, SqlTransaction trans,
                                string ThuchanhID, string KhoahocID, string GiangvienID, string LichhocID,
                                string ChuyendeID, string Diachi, decimal? Soluongtoida);
        public Task UpdateAsync(SqlConnection conn, SqlTransaction trans,
                                string ThuchanhID, string KhoahocID, string GiangvienID, string LichhocID,
                                string ChuyendeID, string Diachi, decimal? Soluongtoida);
        public Task DeleteAsync(SqlConnection conn, SqlTransaction trans, string? ThuchanhID);
    }

}

