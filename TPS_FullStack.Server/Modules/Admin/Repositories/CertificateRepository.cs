using System.Runtime.ConstrainedExecution;
using Microsoft.Data.SqlClient;
using TPS_FullStack.Server.Entities;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class CertificateRepository : ICertificateRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AppUser> _logger;
        public CertificateRepository(IConfiguration configuration, ILogger<AppUser> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<CertificateDetailDto> CertificateFindById(string MaID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryCerficate = @"SELECT cc.MaID, cc.Ten, cc.Mota, cc.Thoigiansudung, cc.Donvicap
                                        FROM dbo.Chungchi cc
                                        WHERE cc.MaID = @MaID AND cc.Khongsudung = 0;"
                                    + @"SELECT kh.MaID, kh.Ten
                                        FROM dbo.Khoahoc kh
                                        WHERE kh.ChungchiID = @MaID and kh.Khongsudung = 0;"
                                    + @"SELECT cc_hv.MaID, hv.Hoten, cc_hv.Ngaycap, cc_hv.Ngayhethan
                                        FROM dbo.Chungchi_Hocvien cc_hv
                                        JOIN dbo.Hocvien hv ON hv.MaID = cc_hv.HocvienID 
                                        WHERE cc_hv.ChungchiID = @MaID AND cc_hv.Khongsudung = 0";
            var certificateDetail = new CertificateDetailDto
            {
                certificateInfo = new certificateInfo(),
                certificateCourses = new List<certificateCourses>(),
                certificateStudents = new List<certificateStudents>()
            };
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = new SqlCommand(queryCerficate, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaID", MaID);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                certificateDetail.certificateInfo.MaID = reader["MaID"].ToString();
                                certificateDetail.certificateInfo.Ten = reader["Ten"].ToString();
                                certificateDetail.certificateInfo.Mota = reader["Mota"].ToString();
                                certificateDetail.certificateInfo.Thoigiansudung = (decimal)reader["MaID"];
                                certificateDetail.certificateInfo.Donvicap = reader["Donvicap"].ToString();
                            }

                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    certificateDetail.certificateCourses.Add(new certificateCourses
                                    {
                                        KhoahocID = reader["MaID"].ToString(),
                                        Ten = reader["Ten"].ToString()
                                    });
                                }
                            }
                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    certificateDetail.certificateStudents.Add(new certificateStudents
                                    {
                                        MaID = reader["MaID"].ToString(),
                                        ChungchiID = certificateDetail.certificateInfo.MaID,
                                        HocvienID = reader["Hoten"].ToString(),
                                        Ngaycap = (DateTime)reader["Ngaycap"],
                                        Ngayhethan = (DateTime)reader["Ngayhethan"]
                                    }
                                    );
                                }
                            }
                        }
                    }
                }
                return certificateDetail;
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
            throw new NotImplementedException();
        }

        public async Task<List<CertificateGetAllDto>> CertificateGetAllAsync() // Mark as done
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryList = @"SELECT cc.MaID, cc.Ten, cc.Mota, cc.Thoigiansudung
                            FROM dbo.Chungchi cc
                            WHERE Khongsudung = 0 ";
            var list = new List<CertificateGetAllDto>();
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = new SqlCommand(queryList, conn))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                list.Add(new CertificateGetAllDto
                                {
                                    MaID = reader["MaID"].ToString(),
                                    Ten = reader["Ten"].ToString(),
                                    Mota = reader["Mota"].ToString(),
                                    Thoigiansudung = (decimal)reader["Thoigiansudung"]
                                });
                            }
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return null;
            }
            throw new NotImplementedException();
        }
    }

}

