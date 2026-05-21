using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class LabRepository : ILabRepository
    {
        private readonly IConfiguration _configuration;
        public LabRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task LabInsertAsync(SqlConnection conn, SqlTransaction trans, 
                                        string ThuchanhID, string KhoahocID, string GiangvienID, 
                                        string LichhocID, string ChuyendeID, string Diachi, decimal? Soluongtoida)
        {
            var queryLabInsert = @"INSERT INTO dbo.Thuchanh (MaID, KhoahocID, GiangvienID, LichhocID, ChuyendeID, Diachi, Soluongtoida)
                                    VALUES (@MaID, @KhoahocID, @GiangvienID, @LichhocID, @ChuyendeID, @Diachi, @Soluongtoida)";

            using (var cmd = new SqlCommand (queryLabInsert, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaID", ThuchanhID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@KhoahocID", KhoahocID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@GiangvienID", GiangvienID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@LichhocID", LichhocID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Diachi", Diachi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Soluongtoida", (object?)Soluongtoida ?? (object)DBNull.Value);

                await cmd.ExecuteNonQueryAsync();

            }
            throw new NotImplementedException();
        }
    }

}

