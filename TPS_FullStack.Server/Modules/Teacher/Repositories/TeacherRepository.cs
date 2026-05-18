using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Teacher
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly IConfiguration _configuration;
        public TeacherRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<teacherInformationDto> GetTeacherInformationAsync(string teacherId)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryGetTeacherInfo = @"SELECT 
                                        gv.MaID, gv.Hoten, gv.Ngaysinh, gv.Gioitinh, gv.Email, gv.Diachi, gv.Dienthoai,
                                        cd.MaID AS ChuyendeID, cd.Ten AS ChuyendeTen, 
                                        kh.MaID AS KhoahocID, kh.Ten AS KhoahocTen
                                        FROM dbo.Giangvien gv
                                        JOIN dbo.Giangvien_Chuyende gvc ON gv.MaID = gvc.GiangvienID
                                        JOIN dbo.Chuyende cd ON gvc.ChuyendeID = cd.MaID
                                        JOIN dbo.Khoahoc_Giangvien khgv ON gv.MaID = khgv.GiangvienID
                                        JOIN dbo.Khoahoc kh ON khgv.KhoahocID = kh.MaID
                                        WHERE gv.MaID = @MaID";

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = new SqlCommand(queryGetTeacherInfo, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaID", teacherId);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                var teacherInfo = new teacherInfo();
                                var teacherTopics = new List<teacherTopics>();
                                var teacherCourses = new List<teacherCourses>();
                                var teacherSchedules = new List<teacherSchedules>();

                                while (await reader.ReadAsync())
                                {
                                    if (teacherInfo.MaID == null)
                                    {
                                        teacherInfo.MaID = reader["MaID"].ToString();
                                        teacherInfo.Hoten = reader["Hoten"].ToString();
                                        teacherInfo.Ngaysinh = Convert.ToDateTime(reader["Ngaysinh"]);
                                        teacherInfo.Gioitinh = reader["Gioitinh"].ToString();
                                        teacherInfo.Email = reader["Email"].ToString();
                                        teacherInfo.Diachi = reader["Diachi"].ToString();
                                        teacherInfo.Dienthoai = reader["Dienthoai"].ToString();
                                    }
                                    var topicId = reader["ChuyendeID"].ToString();
                                    var topicName = reader["ChuyendeTen"].ToString();
                                    if (!teacherTopics.Any(t => t.KhoahocID == topicId))
                                    {
                                        teacherTopics.Add(new teacherTopics
                                        {
                                            KhoahocID = topicId,
                                            TenChuyende = topicName
                                        });
                                    }
                                    var courseId = reader["KhoahocID"].ToString();
                                    var courseName = reader["KhoahocTen"].ToString();
                                    if (!teacherCourses.Any(c => c.KhoahocID == courseId))
                                    {
                                        teacherCourses.Add(new teacherCourses
                                        {
                                            KhoahocID = courseId,
                                            TenKhoahoc = courseName
                                        });
                                    }
                                }
                                return new teacherInformationDto
                                {
                                    TeacherInfo = teacherInfo,
                                    TeacherTopics = teacherTopics,
                                    TeacherCourses = teacherCourses,
                                    TeacherSchedules = teacherSchedules
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                Console.WriteLine($"Error fetching teacher information: {ex.Message}");
            }
            throw new NotImplementedException();
        }

        public async Task<ICollection<teacherSchedules>> GetTeacherSchedulesAsync(string teacherId)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryGetTeacherSchedules = @"SELECT 
                                            lh.MaID AS LichhocID, lh.Ngaydukien, lh.Batdaudukien, lh.Ketthucdukien, lh.Ngaythucte, lh.Batdauthucte, lh.Ketthucthucte,
                                            lh.KhoahocID, kh.Ten AS TenKhoahoc,
                                            lh.ChuyendeID, cd.Ten AS TenChuyende
                                            FROM dbo.Lichhoc lh
                                            JOIN dbo.Khoahoc kh ON lh.KhoahocID = kh.MaID
                                            JOIN dbo.Khoahoc_Giangvien khgv ON kh.MaID = khgv.KhoahocID
                                            JOIN dbo.Chuyende cd ON lh.ChuyendeID = cd.MaID
                                            WHERE lh.GiangvienID = @MaID";

            var teacherSchedules = new List<teacherSchedule>();
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = new SqlCommand(queryGetTeacherSchedules, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaID", teacherId);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                teacherSchedules.Add(new teacherSchedule
                                {
                                    LichhocID = reader["LichhocID"].ToString(),
                                    KhoahocID = reader["KhoahocID"].ToString(),
                                    TenKhoahoc = reader["TenKhoahoc"].ToString(),
                                    ChuyendeID = reader["ChuyendeID"].ToString(),
                                    TenChuyende = reader["TenChuyende"].ToString(),
                                    Ngaydukien = Convert.ToDateTime(reader["Ngaydukien"]),
                                    Batdaudukien = Convert.ToDateTime(reader["Batdaudukien"]),
                                    Ketthucdukien = Convert.ToDateTime(reader["Ketthucdukien"]),
                                    Ngaythucte = Convert.ToDateTime(reader["Ngaythucte"]),
                                    Batdautheothucte = Convert.ToDateTime(reader["Batdauthucte"]),
                                    Ketthuctheothucte = Convert.ToDateTime(reader["Ketthucthucte"])
                                });
                            }
                        }
                    }
                }
                return await Task.FromResult((ICollection<teacherSchedules>)teacherSchedules);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error fetching teacher schedules: {ex.Message}");
            }
            throw new NotImplementedException();
        }

        public async Task<bool> TeacherAttendanceAsync(teacherAttendance attendance)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryInsertAttendance = @"INSERT INTO dbo.Lichhoc_Giangvien_Diemdanh(MaID, LichhocID, GiangvienID, KhoahocID)
                                        VALUES (@MaID, @LichhocID, @GiangvienID, @KhoahocID)";
            var queryUpdateSchedule = @"UPDATE dbo.Lichhoc
                                        SET 
                                            Ngaythucte = @Ngaythucte,
                                            Batdauthucte = @Batdauthucte, 
                                            Ketthucthucte = @Ketthucthucte
                                        WHERE MaID = @LichhocID";

            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (var cmdInsert = new SqlCommand(queryInsertAttendance, conn, transaction))
                        {
                            cmdInsert.Parameters.AddWithValue("@MaID", Guid.NewGuid().ToString());
                            cmdInsert.Parameters.AddWithValue("@LichhocID", attendance.LichhocID);
                            cmdInsert.Parameters.AddWithValue("@GiangvienID", attendance.GiangvienID);
                            cmdInsert.Parameters.AddWithValue("@KhoahocID", attendance.KhoahocID);
                            cmdInsert.ExecuteNonQuery();
                        }
                        using (var cmdUpdate = new SqlCommand(queryUpdateSchedule, conn, transaction))
                        {
                            cmdUpdate.Parameters.AddWithValue("@Ngaythucte", attendance.Ngaythucte);
                            cmdUpdate.Parameters.AddWithValue("@Batdauthucte", attendance.Batdauthucte);
                            cmdUpdate.Parameters.AddWithValue("@Ketthucthucte", attendance.Ketthucthucte);
                            cmdUpdate.Parameters.AddWithValue("@LichhocID", attendance.LichhocID);
                            cmdUpdate.ExecuteNonQuery();
                        }
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        // Log the exception
                        Console.WriteLine($"Error recording teacher attendance: {ex.Message}");
                    }
                }
                return await Task.FromResult(true);
            }
            throw new NotImplementedException();
        }
    }
}


