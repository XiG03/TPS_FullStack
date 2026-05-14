using System.CodeDom.Compiler;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using Microsoft.OpenApi.Validations.Rules;
using TPS_FullStack.Server.Entities;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class CourseRepository : ICourseRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AppUser> _logger;
        public CourseRepository(IConfiguration configuration, ILogger<AppUser> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<List<CourseInfo>> CourseGetAllAsync()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryCourseGetAll = @"SELECT kh.MaID, kh.Ten, kh.Mota
                                    FROM dbo.Khoahoc kh WHERE Khongsudung = 0";

            var courses = new List<CourseInfo>();
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(queryCourseGetAll, conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            courses.Add(new CourseInfo
                            {
                                MaID = reader["MaID"].ToString(),
                                Ten = reader["Ten"].ToString(),
                                Mota = reader["Mota"].ToString()
                            });
                        }
                    }
                }
            }
            return courses;
            throw new NotImplementedException();
        }

        public async Task<CourseDetailDto> CourseGetByIdAsync(string MaID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            // Missing certification name
            var queryCourseInfo = @"SELECT kh.MaID, kh.Ten, kh.Mota, kh.Diemdat, kh.Sobuoihoc, kh.Thu, kh.Thoiluonghoc, kh.Thoiluongthi, kh.Socauhoi, kh.Ngaybatdau
                                    FROM dbo.Khoahoc kh
                                    WHERE kh.MaID = @MaID AND kh.Khongsudung = 0";
            var queryCourseTeachers = @"SELECT kh_gv.MaID, gv.GiangvienID, gv.Hoten
                                        FROM dbo.Giangvien gv
                                        JOIN Khoahoc_Giangvien kh_gv ON kh_gv.GiangvienID = gv.MaID
                                        WHERE kh_gv.KhoahocID = @MaID";
            var queryCourseTopics = @"SELECT kh_cd.MaID, kh_cd.ChuyendeID , cd.Ten, kh_cd.Socauhoi
                                        FROM dbo.Chuyende cd
                                        JOIN Khoahoc_Chuyende kh_cd ON kh_cd.ChuyendeID = cd.MaID
                                        WHERE kh_cd.KhoahocID = @MaID";
            var queryCourseStudents = @"SELECT kh_hv.MaID, kh_hv.HocvienID, hv.Hoten
                                        FROM dbo.Hocvien hv
                                        JOIN  Khoahoc_Hocvien kh_hv ON kh_hv.HocvienID = hv.MaID
                                        WHERE kh_hv.KhoahocID = @MaID";
            var queryCourseSchedules = @"SELECT lh.MaID, lh.Ngaydukien, lh.Ngaythucte, lh.Tugio, lh.Dengio
                                        FROM dbo.Lichhoc lh
                                        WHERE lh.KhoahocID = @MaID";
            // Danh muc cac giang vien va hoc vien diem danh
            var queryScheduleTeacherAttendance = @"";
            var queryScheduleStudentAttendance = @"";
            // var queryCourseExams -- Lay danh muc cac bai thi da duoc thuc hien
            var courseDetail = new CourseDetailDto
            {
                courseInfo = new courseInfo(),
                courseTopics = new List<courseTopics>(),
                courseTeachers = new List<courseTeachers>(),
                courseStudents = new List<courseStudents>(),
                courseSchedules = new List<courseSchedules>()
            };
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                //#1: find course info
                using (var cmd = new SqlCommand(queryCourseInfo, conn))
                {
                    cmd.Parameters.AddWithValue("@MaID", MaID);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            courseDetail.courseInfo.MaID = reader["MaID"].ToString();
                            courseDetail.courseInfo.Ten = reader["Ten"].ToString();
                            courseDetail.courseInfo.Mota = reader["Mota"].ToString();
                            courseDetail.courseInfo.Sobuoihoc = (decimal)reader["Sobuoihoc"];
                            courseDetail.courseInfo.Thoiluonghoc = (decimal)reader["Thoiluonghoc"];
                            courseDetail.courseInfo.Thoiluongthi = (decimal)reader["Thoiluongthi"];
                            courseDetail.courseInfo.Socauhoi = (decimal)reader["Socauhoi"];
                            courseDetail.courseInfo.Thu = reader["Thu"].ToString();
                            courseDetail.courseInfo.Ngaybatdau = (DateTime)reader["Ngaybatdau"];
                        }
                    }
                }
                //#2 find Teachers
                using (var cmd = new SqlCommand(queryCourseTeachers, conn))
                {
                    cmd.Parameters.AddWithValue("@MaID", MaID);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            courseDetail.courseTeachers.Add(new courseTeachers
                            {
                                MaID = reader["MaID"].ToString(),
                                GiangvienID = reader["GiangvienID"].ToString(),
                                Hoten = reader["Hoten"].ToString()
                            });
                        }
                    }
                }
                //#3 find Topics
                using (var cmd = new SqlCommand(queryCourseTopics, conn))
                {
                    cmd.Parameters.AddWithValue("@MaID", MaID);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            courseDetail.courseTopics.Add(new courseTopics
                            {
                                MaID = reader["MaID"].ToString(),
                                ChuyendeID = reader["ChuyendeID"].ToString(),
                                Ten = reader["Ten"].ToString(),
                                SoCauhoi = (decimal)reader["SoCauhoi"]
                            });
                        }
                    }
                }
                //#4 find Students
                using (var cmd = new SqlCommand(queryCourseStudents, conn))
                {
                    cmd.Parameters.AddWithValue("@MaID", MaID);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            courseDetail.courseStudents.Add(new courseStudents
                            {
                                MaID = reader["MaID"].ToString(),
                                HocvienID = reader["HocvienID"].ToString(),
                                Hoten = reader["Hoten"].ToString()
                            });
                        }
                    }
                }
                //#5 find Schedules
                using (var cmd = new SqlCommand(queryCourseSchedules, conn))
                {
                    cmd.Parameters.AddWithValue("@MaID", MaID);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            courseDetail.courseSchedules.Add(new courseSchedules
                            {
                                MaID = reader["MaID"].ToString(),
                                Ngaydukien = (DateTime)reader["Ngaydukien"],
                                Ngaythucte = (DateTime)reader["Ngaythucte"],
                                Tugio = (DateTime)reader["Tugio"],
                                Dengio = (DateTime)reader["Dengio"]
                            });
                        }
                    }
                }
            }
            return courseDetail;
            throw new NotImplementedException();
        }

        public async Task<bool> CourseDeleteByIdAsync(string MaId)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryCourseDelete = @"UPDATE dbo.Khoahoc
                                        SET
                                        	DeletedAt = SYSDATETIME(),
                                        	DeletedBy = ''
                                        WHERE MaID = @KhoahocID";


            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(queryCourseDelete, conn))
                {
                    cmd.Parameters.AddWithValue("@KhoahocID", MaId);

                    if (await cmd.ExecuteNonQueryAsync() < 0)
                    {
                        return false;
                    }
                }
            }
            return true;
            throw new NotImplementedException();
        }
        // Warning
        public async Task<bool> CourseInsertAsync(CourseCreateDto createDto)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryCourseInsert = @"INSERT INTO dbo.Khoahoc(MaID, Ten, Mota, Diemdat, ChungchiID, Khongsudung, CreatedAt, CreatedBy, Ngaybatdau, Thu, Thoiluonghoc, Sobuoihoc, Thoiluongthi, Socauhoi)
                                    VALUES (@MaID, @Ten, @Mota, @Diemdat, @ChungchiID, 0, SYSDATETIME(), '', @Ngaybatdau, @Thu, @Thoiluonghoc, @Sobuoihoc, @Thoiluongthi, @Socauhoi)";
            var queryCourseTopicInsert = @"INSERT INTO dbo.Khoahoc_Chuyende(MaID, KhoahocID, ChuyendeID, Socauhoi)
                                            VALUES (@MaID, @KhoahocID, @ChuyendeID, @Socauhoi)";
            var queryCourseTeacherInsert = @"INSERT INTO dbo.Khoahoc_Giangvien(MaID, KhoahocID, GiangvienID)
                                            VALUES (@MaID, @KhoahocID, @GiangvienID)";
            var queryCourseStudentInsert = @"INSERT INTO dbo.Khoahoc_Hocvien(MaID, KhoahocID, HocvienID)
                                            VALUES (@MaID, @KhoahocID, @HocvienID, @Diem, @Hieuchinh)";
            var queryCourseScheduleInsert = @"INSERT INTO dbo.Lichhoc (MaID, KhoahocID, Ngaydukien, Ngaythucte, Tugio, Dengio)
                                            VALUES (@MaID, @KhoahocID, @Ngaydukien, @Ngaythucte, @Tugio, @Dengio)";


            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var transaction = conn.BeginTransaction();

                try
                {
                    //Step 1: Insert course - mark done 
                    using (var cmd = new SqlCommand(queryCourseInsert, conn, transaction))
                    {
                        createDto.courseDto.MaID = Guid.NewGuid().ToString();
                        cmd.Parameters.AddWithValue("@MaID", createDto.courseDto.MaID == null ? Guid.NewGuid().ToString()
                                                                                            : createDto.courseDto.MaID);
                        cmd.Parameters.AddWithValue("@Ten", createDto.courseDto.Ten);
                        cmd.Parameters.AddWithValue("@Mota", createDto.courseDto.Mota);
                        cmd.Parameters.AddWithValue("@Diemdat", createDto.courseDto.Diemdat);
                        cmd.Parameters.AddWithValue("@ChungchiID", createDto.courseDto.ChungchiID);
                        cmd.Parameters.AddWithValue("@Ngaybatdau", createDto.courseDto.Ngaybatdau);
                        cmd.Parameters.AddWithValue("@Thu", createDto.courseDto.Thu);
                        cmd.Parameters.AddWithValue("@Thoiluonghoc", createDto.courseDto.Thoiluonghoc);
                        cmd.Parameters.AddWithValue("@Sobuoihoc", createDto.courseDto.Sobuoihoc);
                        cmd.Parameters.AddWithValue("@Thoiluongthi", createDto.courseDto.Thoiluongthi);
                        cmd.Parameters.AddWithValue("@Socauhoi", createDto.courseDto.Socauhoi);
                        if (await cmd.ExecuteNonQueryAsync() < 0)
                        {
                            await transaction.RollbackAsync();
                            return false;
                        }
                    }
                    //Step 2: Insert topics 
                    foreach (var topic in createDto.courseTopicDtos)
                    {
                        using (var cmd = new SqlCommand(queryCourseTopicInsert, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@MaID", topic.MaID == null ? Guid.NewGuid().ToString()
                                                                                    : topic.MaID);
                            cmd.Parameters.AddWithValue("@KhoahocID", createDto.courseDto.MaID);
                            cmd.Parameters.AddWithValue("@ChuyendeID", topic.ChuyendeID);
                            cmd.Parameters.AddWithValue("@Socauhoi", topic.SoCauhoi);

                            if (await cmd.ExecuteNonQueryAsync() < 0)
                            {
                                await transaction.RollbackAsync();
                                return false;
                            }
                        }
                    }
                    //Step 3: Insert Teachers list
                    foreach (var teacher in createDto.courseTeacherDtos)
                    {
                        using (var cmd = new SqlCommand(queryCourseTeacherInsert, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@MaID", teacher.MaID == null ? Guid.NewGuid().ToString()
                                                                                    : teacher.MaID);
                            cmd.Parameters.AddWithValue("@KhoahocID", createDto.courseDto.MaID);
                            cmd.Parameters.AddWithValue("@GiangvienID", teacher.GiangvienID);

                            if (await cmd.ExecuteNonQueryAsync() < 0)
                            {
                                await transaction.RollbackAsync();
                                return false;
                            }
                        }
                    }
                    //Step 4: Insert Students list
                    foreach (var student in createDto.courseStudentDtos)
                    {
                        using (var cmd = new SqlCommand(queryCourseStudentInsert, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@MaID", student.MaID == null ? Guid.NewGuid().ToString()
                                                                                    : student.MaID);
                            cmd.Parameters.AddWithValue("@KhoahocID", createDto.courseDto.MaID);
                            cmd.Parameters.AddWithValue("@HocvienID", student.HocvienID);
                            cmd.Parameters.AddWithValue("@Diem", student.Diem);
                            cmd.Parameters.AddWithValue("@Hieuchinh", student.Hieuchinh);

                            if (await cmd.ExecuteNonQueryAsync() < 0)
                            {
                                await transaction.RollbackAsync();
                                return false;
                            }
                        }
                    }
                    //Step 5: Insert Schedules list
                    foreach (var schedule in createDto.courseScheduleDtos)
                    {
                        using (var cmd = new SqlCommand(queryCourseScheduleInsert, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@MaID", schedule.MaID == null ? Guid.NewGuid().ToString()
                                                                                        : schedule.MaID);
                            cmd.Parameters.AddWithValue("@KhoahocID", createDto.courseDto.MaID);
                            cmd.Parameters.AddWithValue("@Ngaydukien", schedule.Ngaydukien);
                            cmd.Parameters.AddWithValue("@Ngaythucte", schedule.Ngaythucte);
                            cmd.Parameters.AddWithValue("@Tugio", schedule.Tugio);
                            cmd.Parameters.AddWithValue("@Dengio", schedule.Dengio);

                            if (await cmd.ExecuteNonQueryAsync() < 0)
                            {
                                await transaction.RollbackAsync();
                                return false;
                            }
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {

                    await transaction.RollbackAsync();
                    _logger.LogError(ex.Message);
                    return false;
                }

            }
            return true;
            throw new NotImplementedException();
        }

        public async Task<bool> CourseUpdateAsync(CourseUpdateDto updateDto)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var querycourseUpdate = @"UPDATE dbo.Khoahoc
                                    SET
                                    	Ten = @Ten,
                                    	Mota = @Mota,
                                    	Diemdat = @Diemdat,
                                        Sobuoihoc = @Sobuoihoc,
                                        Thu = @Thu,
                                        Thoiluonghoc = @Thoiluonghoc,
                                        Ngaybatdau = @Ngaybatdau,
                                        Thoigianthi = @Thoigianthi,
                                        Thoiluongthi = @Thoiluongthi,
                                        Socauhoi = @Socauhoi,
                                    	UpdatedAt = SYSDATETIME(),
                                    	UpdatedBy = ''
                                    WHERE MaID = @MaID";
            var queryDelete =           @"DELETE dbo.Khoahoc_Chuyende WHERE KhoahocID = @MaID;"
                                        + @"DELETE dbo.Khoahoc_Giangvien WHERE KhoahocID = @MaID;"
                                        + @"DELETE dbo.Khoahoc_Hocvien WHERE KhoahocID = @MaID;"
                                        + @"DELETE dbo.Lichhoc_Giangvien_Diemdanh WHERE KhoahocID = @MaID;"
                                        + @"DELETE dbo.Lichhoc_Hocvien_Diemdanh WHERE KhoahocID = @MaID;"
                                        + @"DELETE dbo.Lichhoc WHERE KhoahocID = @MaID;";

            var queryCourseTopicInsert = @"INSERT INTO dbo.Khoahoc_Chuyende(MaID, KhoahocID, ChuyendeID, Socauhoi)
                                            VALUES (@MaID, @KhoahocID, @ChuyendeID, @Socauhoi)";
            var queryCourseTeacherInsert = @"INSERT INTO dbo.Khoahoc_Giangvien(MaID, KhoahocID, GiangvienID)
                                            VALUES (@MaID, @KhoahocID, @GiangvienID)";
            var queryCourseStudentInsert = @"INSERT INTO dbo.Khoahoc_Hocvien(MaID, KhoahocID, HocvienID)
                                            VALUES (@MaID, @KhoahocID, @HocvienID, @Diem, @Hieuchinh)";
            var queryCourseScheduleInsert = @"INSERT INTO dbo.Lichhoc (MaID, KhoahocID, Ngaydukien, Ngaythucte, Tugio, Dengio)
                                            VALUES (@MaID, @KhoahocID, @Ngaydukien, @Ngaythucte, @Tugio, @Dengio)";

            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var transaction = conn.BeginTransaction();
                try
                {
                    using (var cmd = new SqlCommand(queryDelete, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@MaID", updateDto.courseUpdate.MaID);

                        if (await cmd.ExecuteNonQueryAsync() < 0)
                        {
                            await transaction.RollbackAsync();
                            return false;
                        }
                    }
                    foreach (var topic in updateDto.courseTopics)
                    {
                        using (var cmd = new SqlCommand(queryCourseTopicInsert, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@MaID", topic.MaID == null ? Guid.NewGuid().ToString()
                                                                                    : topic.MaID);
                            cmd.Parameters.AddWithValue("@KhoahocID", updateDto.courseUpdate.MaID);
                            cmd.Parameters.AddWithValue("@ChuyendeID", topic.ChuyendeID);
                            cmd.Parameters.AddWithValue("@Socauhoi", topic.SoCauhoi);

                            if (await cmd.ExecuteNonQueryAsync() < 0)
                            {
                                await transaction.RollbackAsync();
                                return false;
                            }
                        }
                    }
                    //Step 3: Insert Teachers list
                    foreach (var teacher in updateDto.courseTeachers)
                    {
                        using (var cmd = new SqlCommand(queryCourseTeacherInsert, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@MaID", teacher.MaID == null ? Guid.NewGuid().ToString()
                                                                                    : teacher.MaID);
                            cmd.Parameters.AddWithValue("@KhoahocID", updateDto.courseUpdate.MaID);
                            cmd.Parameters.AddWithValue("@GiangvienID", teacher.GiangvienID);

                            if (await cmd.ExecuteNonQueryAsync() < 0)
                            {
                                await transaction.RollbackAsync();
                                return false;
                            }
                        }
                    }
                    //Step 4: Insert Students list
                    foreach (var student in updateDto.courseStudents)
                    {
                        using (var cmd = new SqlCommand(queryCourseStudentInsert, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@MaID", student.MaID == null ? Guid.NewGuid().ToString()
                                                                                    : student.MaID);
                            cmd.Parameters.AddWithValue("@KhoahocID", updateDto.courseUpdate.MaID);
                            cmd.Parameters.AddWithValue("@HocvienID", student.HocvienID);
                            cmd.Parameters.AddWithValue("@Diem", student.Diem);
                            cmd.Parameters.AddWithValue("@Hieuchinh", student.Hieuchinh);

                            if (await cmd.ExecuteNonQueryAsync() < 0)
                            {
                                await transaction.RollbackAsync();
                                return false;
                            }
                        }
                    }
                    foreach (var schedule in updateDto.courseScheduleUpdates)
                    {
                        using (var cmd = new SqlCommand(queryCourseScheduleInsert, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@MaID", schedule.MaID == null ? Guid.NewGuid().ToString()
                                                                                        : schedule.MaID);
                            cmd.Parameters.AddWithValue("@KhoahocID", updateDto.courseUpdate.MaID);
                            cmd.Parameters.AddWithValue("@Ngaydukien", schedule.Ngaydukien);
                            cmd.Parameters.AddWithValue("@Ngaythucte", schedule.Ngaythucte);
                            cmd.Parameters.AddWithValue("@Tugio", schedule.Tugio);
                            cmd.Parameters.AddWithValue("@Dengio", schedule.Dengio);

                            if (await cmd.ExecuteNonQueryAsync() < 0)
                            {
                                await transaction.RollbackAsync();
                                return false;
                            }
                        }
                    }
                    using (var cmd = new SqlCommand(querycourseUpdate, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Ten", updateDto.courseUpdate.Ten);
                        cmd.Parameters.AddWithValue("@Mota", updateDto.courseUpdate.Mota);
                        cmd.Parameters.AddWithValue("@Diemdat", updateDto.courseUpdate.Diemdat);
                        cmd.Parameters.AddWithValue("@Sobuoihoc", updateDto.courseUpdate.Sobuoihoc);
                        cmd.Parameters.AddWithValue("@Thu", updateDto.courseUpdate.Thu);
                        cmd.Parameters.AddWithValue("@Thoiluonghoc", updateDto.courseUpdate.Thoiluonghoc);
                        cmd.Parameters.AddWithValue("@Ngaybatdau", updateDto.courseUpdate.Ngaybatdau);
                        cmd.Parameters.AddWithValue("@Thoigianthi", updateDto.courseUpdate.Thoigianthi);
                        cmd.Parameters.AddWithValue("@Thoiluongthi", updateDto.courseUpdate.Thoiluongthi);
                        cmd.Parameters.AddWithValue("@Socauhoi", updateDto.courseUpdate.Socauhoi);
                        if (await cmd.ExecuteNonQueryAsync() < 0)
                        {
                            await transaction.RollbackAsync();
                            return false;
                        }
                    }

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex.Message);
                    return false;
                }
            }
            return true;

            throw new NotImplementedException();
        }
    }

}

