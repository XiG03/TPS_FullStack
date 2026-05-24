using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ILabRepository
    {
        public Task LabInsertAsync ( SqlConnection conn, SqlTransaction trans,
                                        string ThuchanhID, string KhoahocID, string GiangvienID, string LichhocID, 
                                        string ChuyendeID, string Diachi, decimal? Soluongtoida);
    }

}

