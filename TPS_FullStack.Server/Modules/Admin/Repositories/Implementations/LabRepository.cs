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
        public Task DeleteAsync(SqlConnection conn, SqlTransaction trans, string? ThuchanhID)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(SqlConnection conn, SqlTransaction trans,
                                        string ThuchanhID, string KhoahocID, string GiangvienID,
                                        string LichhocID, string ChuyendeID, string Diachi, decimal? Soluongtoida)
        {
            var queryLabInsert = @"INSERT INTO dbo.Thuchanh (MaID, KhoahocID, GiangvienID, LichhocID, ChuyendeID, Diachi, Soluongtoida)
                                    VALUES (@MaID, @KhoahocID, @GiangvienID, @LichhocID, @ChuyendeID, @Diachi, @Soluongtoida)";

            using (var cmd = new SqlCommand(queryLabInsert, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaID",string.IsNullOrWhiteSpace(ThuchanhID)? DBNull.Value: ThuchanhID);
                cmd.Parameters.AddWithValue( "@KhoahocID",string.IsNullOrWhiteSpace(KhoahocID)? DBNull.Value: KhoahocID);
                cmd.Parameters.AddWithValue("@GiangvienID", string.IsNullOrWhiteSpace(GiangvienID) ? DBNull.Value: GiangvienID);
                cmd.Parameters.AddWithValue("@LichhocID", string.IsNullOrWhiteSpace(LichhocID) ? DBNull.Value: LichhocID);
                cmd.Parameters.AddWithValue("@ChuyendeID", string.IsNullOrWhiteSpace(ChuyendeID)? DBNull.Value: ChuyendeID);
                cmd.Parameters.AddWithValue("@Diachi",string.IsNullOrWhiteSpace(Diachi)? DBNull.Value: Diachi);
                cmd.Parameters.AddWithValue("@Soluongtoida", (object?)Soluongtoida ?? (object)DBNull.Value);

                await cmd.ExecuteNonQueryAsync();

            }

        }

        public Task UpdateAsync(SqlConnection conn, SqlTransaction trans, string ThuchanhID, string KhoahocID, string GiangvienID, string LichhocID, string ChuyendeID, string Diachi, decimal? Soluongtoida)
        {
            throw new NotImplementedException();
        }
    }

}

