using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging.Abstractions;

namespace TPS_FullStack.Server.Modules.Student
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IConfiguration _configuration;
        public StudentRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<studentCourseInfo> GetStudentCourseInfoAsync(string HocvienID, string KhoahocID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryGetInfo = @"SELECT 
                                kh.MaID AS KhoahocID, kh.Ten AS TenKhoahoc, kh.Mota, kh.Diemdat, kh.Thoiluongthi, kh.Socauhoi
                                cd.MaID AS ChuyendeID, cd.Ten AS TenChuyende, cd.Mota, khcd.Socauhoi AS SocauhoiChuyende
                                cdtl.MaID AS TailieuID, cdtl.Tieude 
                                FROM dbo.Khoahoc AS kh
                                JOIN dbo.Khoahoc_Hocvien AS khhv ON kh.MaID = khhv.KhoahocID
                                JOIN dbo.Khoahoc_Chuyende AS khcd ON kh.MaID = khcd.KhoahocID
                                JOIN dbo.Chuyende AS cd ON khcd.ChuyendeID = cd.MaID
                                JOIN dbo.Chuyende_Tailieu AS cdtl ON cd.MaID = cdtl.ChuyendeID
                                WHERE kh.MaID = @KhoahocID AND khhv.HocvienID = @HocvienID";
            using (var conn = new SqlConnection(connectionString))
            {
                var courseInfo = new courseInfo();
                var topicInfos = new List<topicInfo>();
                var topicDocInfos = new List<topicDocInfo>();
                var courseExamInfo = new courseExamInfo();

                var command = new SqlCommand(queryGetInfo, conn);
                command.Parameters.AddWithValue("@KhoahocID", KhoahocID);
                command.Parameters.AddWithValue("@HocvienID", HocvienID);

                await conn.OpenAsync();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (courseInfo.MaID == null)
                        {
                            courseInfo.MaID = reader["KhoahocID"].ToString();
                            courseInfo.TenKhoaHoc = reader["TenKhoahoc"].ToString();
                            courseInfo.Mota = reader["Mota"].ToString();
                            courseInfo.Diemdat = reader["Diemdat"].ToString();
                            courseExamInfo.Thoiluongthi = Convert.ToDecimal(reader["Thoiluongthi"]);
                            courseExamInfo.Socauhoi = Convert.ToDecimal(reader["Socauhoi"]);
                        }

                        var topicInfo = new topicInfo
                        {
                            ChuyendeID = reader["ChuyendeID"].ToString(),
                            TenChuyende = reader["TenChuyende"].ToString(),
                            Mota = reader["Mota"].ToString(),
                            Socauhoi = Convert.ToDecimal(reader["SocauhoiChuyende"])
                        };
                        topicInfos.Add(topicInfo);

                        var topicDocInfo = new topicDocInfo
                        {
                            TailieuID = reader["TailieuID"].ToString(),
                            Tentailieu = reader["Tieude"].ToString()
                        };
                        topicDocInfos.Add(topicDocInfo);
                    }
                }

                var studentCourseInfo = new studentCourseInfo
                {
                    courseInfo = courseInfo,
                    topicInfos = topicInfos,
                    topicDocInfos = topicDocInfos,
                    courseExamInfo = new List<courseExamInfo> { courseExamInfo }
                };

                return await Task.FromResult(studentCourseInfo);
            }
            throw new NotImplementedException();
        }

        public async Task<ICollection<studentCourse>> GetStudentCoursesAsync(string MaID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryGetCourses = @"SELECT kh.MaID, kh.Ten
                                FROM dbo.Khoahoc kh
                                JOIN dbo.Khoahoc_Hocvien khhv ON kh.MaID = khhv.KhoahocID
                                WHERE khhv.HocvienID = @MaID";
            using(var conn = new SqlConnection(connectionString))
            {
                var courses = new List<studentCourse>();
                var command = new SqlCommand(queryGetCourses, conn);
                command.Parameters.AddWithValue("@MaID", MaID);

                await conn.OpenAsync();
                using(var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        var course = new studentCourse
                        {
                            MaID = reader["MaID"].ToString(),
                            TenKhoaHoc = reader["Ten"].ToString()
                        };
                        courses.Add(course);
                    }
                }
                return await Task.FromResult((ICollection<studentCourse>)courses);
            }
            throw new NotImplementedException();
        }

        public async Task<studentInfo> GetStudentInfoAsync(string MaID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryGetInfo = @"SELECT 
                                hv.MaID, hv.Hoten, hv.Ngaysinh, hv.Gioitinh, hv.Email, hv.Diachi, hv.Dienthoai,
                                kh.MaID AS KhoahocID, kh.Ten AS KhoahocTen,
                                cchv.MaID AS ChungchiHocvienID, cchv.ChungchiID, cchv.Chungchi_Ten, cchv.Chungchi_Mota, cchv.Chungchi_Donvicap, cchv.Ngaycap, cchv.Ngayhethan
                                FROM dbo.Hocvien hv
                                JOIN Khoahoc_Hocvien khhv ON hv.MaID = khhv.HocvienID
                                JOIN Khoahoc kh ON khhv.KhoahocID = kh.MaID
                                JOIN Chungchi_Hocvien cchv ON hv.MaID = cchv.HocvienID
                                WHERE hv.MaID = @MaID";

            using (var connection = new SqlConnection(connectionString))
            {
                var studentInfo = new studentInfo();
                var studentInfoDto = new studentInfoDto();
                var studentCourseDtos = new List<studentCourseDto>();
                var studentCertificateDtos = new List<studentCertificateDto>();

                var command = new SqlCommand(queryGetInfo, connection);
                command.Parameters.AddWithValue("@MaID", MaID);

                await connection.OpenAsync();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (studentInfo.studentInfoDto == null)
                        {
                            studentInfoDto.MaID = reader["MaID"].ToString();
                            studentInfoDto.studentName = reader["Hoten"].ToString();
                            studentInfoDto.Ngaysinh = Convert.ToDateTime(reader["Ngaysinh"]);
                            studentInfoDto.Gioitinh = reader["Gioitinh"].ToString();
                            studentInfoDto.Diachi = reader["Diachi"].ToString();
                            studentInfoDto.Dienthoai = reader["Dienthoai"].ToString();
                            studentInfo.studentInfoDto = studentInfoDto;
                        }

                        if (reader["KhoahocID"] != DBNull.Value)
                        {
                            var courseDto = new studentCourseDto
                            {
                                MaID = reader["KhoahocID"].ToString(),
                                TenKhoaHoc = reader["KhoahocTen"].ToString()
                            };
                            studentCourseDtos.Add(courseDto);
                        }

                        if (reader["ChungchiHocvienID"] != DBNull.Value)
                        {
                            var certificateDto = new studentCertificateDto
                            {
                                ChungchiID = reader["ChungchiID"].ToString(),
                                Tenchungchi = reader["Chungchi_Ten"].ToString(),
                                Mota = reader["Chungchi_Mota"].ToString(),
                                Donvicap = reader["Chungchi_Donvicap"].ToString(),
                                Ngaycap = Convert.ToDateTime(reader["Ngaycap"]),
                                Ngayhethan = Convert.ToDateTime(reader["Ngayhethan"])
                            };
                            studentCertificateDtos.Add(certificateDto);
                        }
                    }
                }

                studentInfo.studentCourseDtos = studentCourseDtos;
                studentInfo.studentCertificateDtos = studentCertificateDtos;

                return await Task.FromResult(studentInfo);
            }
            throw new NotImplementedException();
        }

        public async Task<ICollection<studentSchedule>> GetStudentScheduleAsync(string MaID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryGetSchedule = @"SELECT 
                                    lh.MaID AS LichhocID, lh.Ngaydukien, lh.Batdaudukien, lh.Ketthucdukien, lh.Ngaythucte, lh.Batdauthucte, lh.Ketthucthucte,
                                    lh.KhoahocID, kh.Ten AS TenKhoahoc,
                                    lh.ChuyendeID, cd.Ten AS TenChuyende,
                                    lhhv.HocvienID
                                    FROM dbo.Lichhoc lh
                                    JOIN dbo.Lichhoc_Hocvien_Diemdanh lhhv ON lh.MaID = lhhv.LichhocID
                                    JOIN dbo.Khoahoc kh ON lh.KhoahocID = kh.MaID
                                    JOIN dbo.Khoahoc_Hocvien khhv ON kh.MaID = khhv.KhoahocID
                                    JOIN dbo.Chuyende cd ON lh.ChuyendeID = cd.MaID
                                    WHERE khhv.HocvienID = @MaID";   

            using(var conn = new SqlConnection(connectionString))
            {
                var scheduleList = new List<studentSchedule>();
                var command = new SqlCommand(queryGetSchedule, conn);
                command.Parameters.AddWithValue("@MaID", MaID);

                await conn.OpenAsync();
                using(var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        var schedule = new studentSchedule
                        {
                            LichhocID = reader["LichhocID"].ToString(),
                            Ngaydukien = Convert.ToDateTime(reader["Ngaydukien"]),
                            Batdaudukien = Convert.ToDateTime(reader["Batdaudukien"]),
                            Ketthucdukien = Convert.ToDateTime(reader["Ketthucdukien"]),
                            Ngaythucte = Convert.ToDateTime(reader["Ngaythucte"]),
                            Batdautheothucte = Convert.ToDateTime(reader["Batdauthucte"]),
                            Ketthuctheothucte = Convert.ToDateTime(reader["Ketthucthucte"]),
                            KhoahocID = reader["KhoahocID"].ToString(),
                            TenKhoahoc = reader["TenKhoahoc"].ToString(),
                            ChuyendeID = reader["ChuyendeID"].ToString(),
                            TenChuyende = reader["TenChuyende"].ToString(),
                            Trangthai = reader["HocvienID"].ToString() == null ? "Chua diem danh" : "Diem danh"
                        };
                        scheduleList.Add(schedule);
                    }
                }
                return await Task.FromResult(scheduleList);
            }
            throw new NotImplementedException();
        }

        public async Task<bool> StudentCheckInAsync(studentAttendance attendance)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryCheckIn = @"INSERT INTO dbo.Lichhoc_Hocvien_Diemdanh(MaID, LichhocID, HocvienID, KhoahocID)
                                VALUES (@MaID, @LichhocID, @HocvienID, @KhoahocID)";


            using (var conn = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryCheckIn, conn);
                command.Parameters.AddWithValue("@MaID", Guid.NewGuid().ToString());
                command.Parameters.AddWithValue("@LichhocID", attendance.LichhocID);
                command.Parameters.AddWithValue("@HocvienID", attendance.HocvienID);
                command.Parameters.AddWithValue("@KhoahocID", attendance.KhoahocID);

                await conn.OpenAsync();
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
            throw new NotImplementedException();
        }
    }

}

