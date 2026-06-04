using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class AddressRepository : IAddressRepository
    {
        public async Task CreateAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string? Ten, string? Diachi, string? Phonghoc)
        {
            var query = @"INSERT INTO dbo.Diadiem (MaID, Ten, Diachi, Phonghoc) VALUES (@MaID, @Ten, @Diachi, @Phonghoc)";
            using (var cmd = new SqlCommand(query, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaID", MaID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Ten", Ten ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Diachi", Diachi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Phonghoc", Phonghoc ?? (object)DBNull.Value);
                await cmd.ExecuteNonQueryAsync();
            }
            
        }

        public async Task DeleteAsync(SqlConnection conn, SqlTransaction trans, string? MaID)
        {
            var query = @"DELETE FROM dbo.Diadiem WHERE MaID = @MaID";
            using (var cmd = new SqlCommand(query, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaID", MaID ?? (object)DBNull.Value);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<AddressModel>> GetAllAsync(SqlConnection conn)
        {
            var query = @"SELECT MaID, Ten, Diachi, Phonghoc FROM dbo.Diadiem";
            using( var cmd = new SqlCommand(query, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    var list = new List<AddressModel>();
                    while (reader.Read())
                    {
                        var item = new AddressModel
                        {
                            MaID = reader["MaID"].ToString(),
                            Ten = reader["Ten"].ToString(),
                            Diachi = reader["Diachi"].ToString(),
                            Phonghoc = reader["Phonghoc"].ToString()
                        };
                        list.Add(item);
                    }
                    return list;
                }
            }
            throw new NotImplementedException();
        }

        public Task<AddressModel> GetByIDAsync(SqlConnection conn, string MaID)
        {
            var query = @"SELECT MaID, Ten, Diachi, Phonghoc FROM dbo.Diadiem WHERE MaID = @MaID";
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaID", MaID);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var item = new AddressModel
                        {
                            MaID = reader["MaID"].ToString(),
                            Ten = reader["Ten"].ToString(),
                            Diachi = reader["Diachi"].ToString(),
                            Phonghoc = reader["Phonghoc"].ToString()
                        };
                        return Task.FromResult(item);
                    }
                    else
                    {
                        return Task.FromResult<AddressModel>(null);
                    }
                }
            } 
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string? Ten, string? Diachi, string? Phonghoc)
        {
            var query = @"UPDATE dbo.Diadiem SET Ten = @Ten, Diachi = @Diachi, Phonghoc = @Phonghoc WHERE MaID = @MaID";
            using(var cmd = new SqlCommand(query, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaID", MaID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Ten", Ten ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Diachi", Diachi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Phonghoc", Phonghoc ?? (object)DBNull.Value);
                await cmd.ExecuteNonQueryAsync();
            }
            throw new NotImplementedException();
        }
    }
}
