using System.Runtime.ConstrainedExecution;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging.Abstractions;
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

        public async Task<bool> CertificateDeleteAsync(string MaID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryDelete = @"UPDATE dbo.Chungchi
                                SET Khongsudung = 1,
                                    DeletedAt = SYSDATETIME(),
                                    DeletedBy = ''
                                WHERE MaID = @MaID;";
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = new SqlCommand(queryDelete, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaID", MaID);
                        if(await cmd.ExecuteNonQueryAsync() < 0){
                            return false;
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
            throw new NotImplementedException();
        }

        public async Task<CertificateDetailDto> CertificateFindByIdAsync(string MaID)
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
            }
            catch (Exception ex)
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

        public async Task<bool> CertificateInsertAsync(CertificateCreateDto createDTO)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryCertificateInsert = @"INSERT INTO dbo.Chungchi (MaID, Ten, Mota, Donvicap, Thoigiansudung, Khongsudung, CreatedAt, CreatedBy)
                                        VALUES (@MaID, @Ten, @Mota, @Donvicap, @Thoigiansudung, @Khongsudung, SYSDATETIME(), '')";

            var queryCerCourseUpdate = @"UPDATE dbo.Khoahoc
                                        SET
                                        	ChungchiID = @MaID,
                                        	UpdatedAt = SYSDATETIME(),
                                        	UpdatedBy = @User
                                        WHERE
                                        	MaID = @KhoahocID;";
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using var trans = conn.BeginTransaction();
                try
                {
                    
                    // Step 1: Tao bang chung chi
                    using (var cmd = new SqlCommand(queryCertificateInsert, conn, trans))
                    {
                        cmd.Parameters.AddWithValue("@MaID", createDTO.certificateInfo.MaID = Guid.NewGuid().ToString());
                        cmd.Parameters.AddWithValue("@Ten", createDTO.certificateInfo.Ten);
                        cmd.Parameters.AddWithValue("@Mota", createDTO.certificateInfo.Mota);
                        cmd.Parameters.AddWithValue("@Donvicap", createDTO.certificateInfo.Donvicap);
                        cmd.Parameters.AddWithValue("@Thoigiansudung", createDTO.certificateInfo.Thoigiansudung);
                        cmd.Parameters.AddWithValue("@Khongsudung", createDTO.certificateInfo.Khongsudung == null ? false : createDTO.certificateInfo.Khongsudung);

                        if (await cmd.ExecuteNonQueryAsync() < 0)
                        {
                            await trans.RollbackAsync();
                            return false;
                        }
                    }

                    // Step 2: Update bang Khoahoc co chung chi nao
                    foreach (var course in createDTO.certificateCourses)
                    {
                        using (var cmd = new SqlCommand(queryCerCourseUpdate, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@KhoahocID", course.KhoahocID);
                            cmd.Parameters.AddWithValue("@MaID", createDTO.certificateInfo.MaID);
                            cmd.Parameters.AddWithValue("@User", "");

                            if (await cmd.ExecuteNonQueryAsync() < 0)
                            {
                                await trans.RollbackAsync();
                                return false;
                            }
                        }
                    }
                    await trans.CommitAsync();
                    return true;

                }
                catch (Exception ex)
                {
                    await trans.RollbackAsync();
                    _logger.LogError(ex.Message);
                    return false;
                }
            }
            throw new NotImplementedException();
        }

        public async Task<bool> CertificateUpdateAsync(CertificateUpdateDto updateDto)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryCertiInfoUpdate = @"UPDATE dbo.Chungchi
                                    SET
                                    	Ten = @Ten,
                                    	Mota = @Mota,
                                    	Donvicap = @Donvicap,
                                    	Thoigiansudung = @Thoigiansudung,
                                    	UpdatedAt = SYSDATETIME(),
                                    	UpdatedBy = @User
                                    WHERE MaID = @MaID";
            var queryCerCourseRemove = @"UPDATE dbo.Khoahoc
                                        SET 
                                            ChungchiID = NULL,
                                            UpdatedAt = SYSDATETIME(),
                                            UpdatedBy = @User
                                        WHERE ChungchiID = @MaID;";
            var queryCerCourseUpdate = @"UPDATE dbo.Khoahoc
                                        SET
                                        	ChungchiID = @MaID,
                                        	UpdatedAt = SYSDATETIME(),
                                        	UpdatedBy = @User
                                        WHERE
                                        	MaID = @KhoahocID";
            var queryCerStudentUpdate = @"UPDATE dbo.Chungchi_Hocvien
                                            SET
                                            	ChungchiID = @MaID,
                                            	Chungchi_Ten = @Ten,
                                            	Chungchi_Mota = @Mota,
                                            	Chungchi_Donvicap = @Donvicap,
                                            	UpdatedAt = SYSDATETIME(),
                                            	UpdatedBy = @User
                                            WHERE ChungchiID = @MaID";
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using var trans = conn.BeginTransaction();
                try
                {
                    // Step 1: Update Certificate information
                    using (var cmd = new SqlCommand(queryCertiInfoUpdate, conn, trans))
                    {
                        cmd.Parameters.AddWithValue("@MaID", updateDto.certificateInfo.MaID);
                        cmd.Parameters.AddWithValue("@Ten", updateDto.certificateInfo.Ten);
                        cmd.Parameters.AddWithValue("@Mota", updateDto.certificateInfo.Mota);
                        cmd.Parameters.AddWithValue("@Donvicap", updateDto.certificateInfo.Donvicap);
                        cmd.Parameters.AddWithValue("@Thoigiansudung", updateDto.certificateInfo.Thoigiansudung);
                        cmd.Parameters.AddWithValue("@User", null);

                        if (await cmd.ExecuteNonQueryAsync() < 0)
                        {
                            await trans.RollbackAsync();
                            return false;
                        }
                    }
                    // Step 2: Remove ChungchiID in Khoahoc table if have ChungchiID
                    using (var cmd = new SqlCommand(queryCerCourseRemove, conn, trans))
                    {
                        cmd.Parameters.AddWithValue("@MaID", updateDto.certificateInfo.MaID);
                        cmd.Parameters.AddWithValue("@User", null);

                        if (await cmd.ExecuteNonQueryAsync() < 0)
                        {
                            await trans.RollbackAsync();
                            return false;
                        }
                    }
                    // Step 3: Update ChungchiIDs in Khoahoc if have
                    foreach (var course in updateDto.certificateCourses)
                    {
                        using (var cmd = new SqlCommand(queryCerCourseUpdate, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@MaID", updateDto.certificateInfo.MaID);
                            cmd.Parameters.AddWithValue("@User", null);
                            cmd.Parameters.AddWithValue("@KhoahocID", course.KhoahocID);

                            if (await cmd.ExecuteNonQueryAsync() < 0)
                            {
                                await trans.RollbackAsync();
                                return false;
                            }
                        }
                    }
                    // Step 4: Update ChungchiInfo in Chungchi_Hocvien 
                    using (var cmd = new SqlCommand(queryCerStudentUpdate, conn, trans))
                    {
                        cmd.Parameters.AddWithValue("@MaID", updateDto.certificateInfo.MaID);
                        cmd.Parameters.AddWithValue("@Ten", updateDto.certificateInfo.Ten);
                        cmd.Parameters.AddWithValue("@Mota", updateDto.certificateInfo.Mota);
                        cmd.Parameters.AddWithValue("@Donvicap", updateDto.certificateInfo.Donvicap);
                        cmd.Parameters.AddWithValue("@User", null);

                        if (await cmd.ExecuteNonQueryAsync() < 0)
                        {
                            await trans.RollbackAsync();
                            return false;
                        }
                    }
                    await trans.CommitAsync();
                    return true;

                }
                catch (Exception ex)
                {
                    await trans.RollbackAsync();
                    _logger.LogError(ex.Message);
                    return false;
                }
            }
            throw new NotImplementedException();
        }
        public async Task<bool> CertificateStuAcceptAsync(Cert_StudentCreateDto createDto)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryInsert = @"INSERT INTO dbo.Chungchi_Hocvien (MaID, ChungchiID, HocvienID, Chungchi_Ten, Chungchi_Mota, Chungchi_Donvicap, Ngaycap, Ngayhethan, Khongsudung, CreatedAt, CreatedBy)
                                VALUES (@MaID, @ChungchiID, @HocvienID, @ChungchiTen, @ChungchiMota, @Donvicap, @Ngaycap, @Ngayhethan, @Khongsudung, SYSDATETIME(), @CreatedBy)";

            using(var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using(var cmd = new SqlCommand(queryInsert, conn))
                {
                    cmd.Parameters.AddWithValue("@MaID", createDto.MaID = Guid.NewGuid().ToString());
                    cmd.Parameters.AddWithValue("@ChungchiID", createDto.ChungchiID);
                    cmd.Parameters.AddWithValue("@HocvienID", createDto.HocvienID);
                    cmd.Parameters.AddWithValue("@ChungchiTen", createDto.Ten);
                    cmd.Parameters.AddWithValue("@ChungchiMota", createDto.Mota);
                    cmd.Parameters.AddWithValue("@Donvicap", createDto.Donvicap);
                    cmd.Parameters.AddWithValue("@Ngaycap", createDto.Ngaycap);
                    cmd.Parameters.AddWithValue("@Ngayhethan", createDto.Ngayhethan);
                    cmd.Parameters.AddWithValue("@Khongsudung", createDto.Khongsudung == null ? false: createDto.Khongsudung);
                    cmd.Parameters.AddWithValue("@CreatedBy", null);

                    if(await cmd.ExecuteNonQueryAsync() < 0)
                    {
                        return false;
                    }
                }
                return true;
            }
            throw new NotImplementedException();
        }

        public async Task<bool> CertificateStuRevokeAsync(string MaID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryRevoke = @"UPDATE dbo.Chungchi_Hocvien
                                SET Khongsudung = 1
                                WHERE MaID = @MaID";
            using(var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using(var cmd = new SqlCommand(queryRevoke, conn))
                {
                    cmd.Parameters.AddWithValue("@MaID", MaID);
                    if(await cmd.ExecuteNonQueryAsync() < 0)
                    {
                        return false;
                    }
                }
                return true;
            }
            throw new NotImplementedException();
        }
    }

}

