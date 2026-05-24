using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly IConfiguration _configuration;
        public ScheduleRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> ScheduleDeleteAsync(string MaID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryScheduleDelete = @"DELETE dbo.Lichhoc WHERE MaID = @MaID";
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(queryScheduleDelete, conn))
                {
                    cmd.Parameters.AddWithValue("@MaID", MaID);

                    if (await cmd.ExecuteNonQueryAsync() < 0)
                    {
                        return false;
                    }
                }
            }
            return true;
            throw new NotImplementedException();
        }

        public async Task<List<ScheduleGetAllDto>> ScheduleGetAllAsync()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryGetAll = @"SELECT lh.MaID, lh.KhoahocID, kh.Ten, lh.Ngaydukien, lh.Batdaudukien, lh.Ketthucdukien, lh.Ngaythucte, lh.Batdauthucte, lh.Ketthucthucte 
                                FROM dbo.Lichhoc lh
                                LEFT JOIN dbo.Khoahoc kh ON kh.MaID = lh.KhoahocID";
            var list = new List<ScheduleGetAllDto>();
            using(var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using(var cmd = new SqlCommand(queryGetAll, conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while(await reader.ReadAsync())
                        {
                            list.Add(new ScheduleGetAllDto
                            {
                                MaID = reader["MaID"].ToString(),
                                KhoahocID = reader["KhoahocID"].ToString(),
                                TenKhoahoc = reader["Ten"].ToString(),
                                Ngaydukien = (DateTime)reader["Ngaydukien"],
                                Batdaudukien = (DateTime)reader["Batdaudukien"],
                                Ketthucdukien = (DateTime)reader["Ketthucdukien"],
                                Ngaythucte = (DateTime)reader["Ngaythucte"],
                                Batdauthucte = (DateTime)reader["Batdauthucte"],
                                Ketthucthucte = (DateTime)reader["Ketthucthucte"]
                            });    
                        }
                    }
                }
            }
            return list;
            throw new NotImplementedException();
        }

        public Task<ScheduleDetailDto> ScheduleGetById(string MaID)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ScheduleInsertAsync(ScheduleCreateDto createDto)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryScheduleInsert = @"INSERT INTO dbo.Lichhoc(MaID, KhoahocID, Ngaydukien, Batdaudukien, Ketthucdukien)
                                        VALUES (@MaID, @KhoahocID, @Ngaydukien, @Batdaudukien, @Ketthucdukien)";

            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(queryScheduleInsert, conn))
                {
                    cmd.Parameters.AddWithValue("@MaID", createDto.MaID = Guid.NewGuid().ToString());
                    cmd.Parameters.AddWithValue("@KhoahocID", createDto.KhoahocID);
                    cmd.Parameters.AddWithValue("@Ngaydukien", createDto.Ngaydukien);
                    cmd.Parameters.AddWithValue("@Batdaudukien", createDto.Batdaudukien);
                    cmd.Parameters.AddWithValue("@Ketthucdukien", createDto.Ketthucdukien);

                    if (await cmd.ExecuteNonQueryAsync() < 0)
                    {
                        return false;
                    }
                }
            }
            return true;
            throw new NotImplementedException();
        }

        public async Task<bool> ScheduleStudentAttendanceAsync(ScheduleStudentAttendanceDto attendanceDto)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryAttendance = @"INSERT INTO dbo.Lichhoc_Hocvien_Diemdanh(MaID, HocvienID, LichhocID)
                                    VALUES(@MaID, @HocvienID, @LichhocID)";
            using(var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using(var cmd = new SqlCommand(queryAttendance, conn))
                {
                    cmd.Parameters.AddWithValue("@MaID", attendanceDto.MaID = Guid.NewGuid().ToString());
                    cmd.Parameters.AddWithValue("@HocvienID", attendanceDto.HocvienID);
                    cmd.Parameters.AddWithValue("@LichhocID", attendanceDto.LichhocID);

                    if(await cmd.ExecuteNonQueryAsync() <= 0)
                    {
                        return false;
                    }
                }
            }
            return true;
            throw new NotImplementedException();
        }

        public async Task<bool> ScheduleTeacherAttandanceAsync(ScheduleTeacherAttendanceDto attendanceDto)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryAttendance = @"INSERT INTO dbo.Lichhoc_Giangvien_Diemdanh(MaID, GiangvienID, LichhocID)
                                    VALUES (@MaID, @GiangvienID, @LichhhocID)";
            using(var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using(var cmd = new SqlCommand(queryAttendance, conn))
                {
                    cmd.Parameters.AddWithValue("@MaID", attendanceDto.MaID = Guid.NewGuid().ToString());
                    cmd.Parameters.AddWithValue("@GiangvienID", attendanceDto.GiangvienID);
                    cmd.Parameters.AddWithValue("@LichhocID", attendanceDto.LichhocID);

                    if(await cmd.ExecuteNonQueryAsync() <= 0)
                    {
                        return false;
                    }
                }
            }
            return true;
            throw new NotImplementedException();
        }

        public async Task<bool> ScheduleUpdateAsync(ScheduleUpdateDto updateDto)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryUpdate = @"UPDATE dbo.Lichhoc
                                SET
                                	Ngaydukien = @Ngaydukien,
                                	Batdaudukien = @Batdaudukien,
                                	Ketthucdukien = @Ketthucdukien,
                                	Ngaythucte = @Ngaythucte,
                                	Batdauthucte = @Batdauthucte,
                                	Ketthucthucte = @Ketthucthucte
                                WHERE MaID = @MaID";
            using(var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using(var cmd = new SqlCommand(queryUpdate, conn))
                {
                    cmd.Parameters.AddWithValue("@Ngaydukien", updateDto.Ngaydukien);
                    cmd.Parameters.AddWithValue("@Batdaudukien", updateDto.Batdaudukien);
                    cmd.Parameters.AddWithValue("@Ketthucdukien", updateDto.Ketthucdukien);
                    cmd.Parameters.AddWithValue("@Ngaythucte", updateDto.Ngaythucte);
                    cmd.Parameters.AddWithValue("@Batdauthucte", updateDto.Batdauthucte);
                    cmd.Parameters.AddWithValue("@Ketthucthucte", updateDto.Ketthucthucte);
                    cmd.Parameters.AddWithValue("@MaID", updateDto.MaID);

                    if(await cmd.ExecuteNonQueryAsync() <= 0)
                    {
                        return false;
                    }
                }
            }
            return true;
            throw new NotImplementedException();
        }
    }

}

