using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class LabService : ILabService
    {
        private readonly IConfiguration _configuration;
        private readonly ILabRepository _labrepository;
        public LabService(IConfiguration configuration, ILabRepository labRepository)
        {
            _configuration = configuration;
            _labrepository = labRepository;
        }
        public async Task<ServiceDefault<LabInsert>> CreateLabAsync(LabInsert labInsert)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        // Create lab Id
                        labInsert.lab.ThuchanhID = Guid.NewGuid().ToString();

                        // insert lab to database
                        await _labrepository.LabInsertAsync(conn, trans, labInsert.lab.ThuchanhID,
                                                            labInsert.lab.KhoahocID, labInsert.lab.GiangvienID, labInsert.schedule.LichhocID,
                                                            labInsert.lab.ChuyendeID, labInsert.lab.Diachi, labInsert.lab.Soluongtoida);

                        await trans.CommitAsync();
                        return new ServiceDefault<LabInsert>
                        {
                            statusCode = StatusCodes.Status200OK,
                            Message = "Tao buoi thuc hanh thanh cong",
                            Data = labInsert
                        };
                    }catch (Exception ex)
                    {
                        await trans.RollbackAsync();
                        return new ServiceDefault<LabInsert>
                        {
                            statusCode = StatusCodes.Status500InternalServerError,
                            Message = "Server khong tao duoc buoi thuc hanh",
                            Data = null
                        };
                    }
                }
            }
            throw new NotImplementedException();
        }
    }

}

