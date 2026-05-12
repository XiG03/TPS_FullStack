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
            var queryCourseInfo = @"SELECT kh.MaID, kh.Ten, kh.Mota
                                    FROM dbo.Khoahoc kh
                                    WHERE kh.MaID = @MaID AND kh.Khongsudung = 0";
            var queryCourseTeachers = @"SELECT gv.MaID, gv.Hoten
                                        FROM dbo.Giangvien gv
                                        JOIN Khoahoc_Giangvien kh_gv ON kh_gv.GiangvienID = gv.MaID
                                        WHERE kh_gv.KhoahocID = @MaID";
            var queryCourseTopics = @"SELECT cd.MaID, cd.Ten
                                        FROM dbo.Chuyende cd
                                        JOIN Khoahoc_Chuyende kh_cd ON kh_cd.ChuyendeID = cd.MaID
                                        WHERE kh_cd.KhoahocID = @MaID";
            var queryCourseStudents = @"SELECT hv.MaID, hv.Hoten
                                        FROM dbo.Hocvien hv
                                        JOIN  Khoahoc_Hocvien kh_hv ON kh_hv.HocvienID = hv.MaID
                                        WHERE kh_hv.KhoahocID = @MaID";
            var courseDetail = new CourseDetailDto
            {
                courseInfo = new courseInfo(),
                courseTopics = new List<courseTopics>(),
                courseTeachers = new List<courseTeachers>(),
                courseStudents = new List<courseStudents>(),
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
                                Ten = reader["Ten"].ToString()
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
                                Ten = reader["Ten"].ToString()
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
                                Ten = reader["Ten"].ToString()
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

        public async Task<bool> CourseInsertAsync(CourseCreateDto createDto)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryCourseInsert = @"INSERT INTO dbo.Khoahoc(MaID, Ten, Mota, Diemdat, ChungchiID, LichhocID, Khongsudung, CreatedAt, CreatedBy)
                                    VALUES (@MaID, @Ten, @Mota, @Diemdat,@ChungchiID, @LichhocID, 0, SYSDATETIME(), '')";
            var queryCourseTopicInsert = @"INSERT INTO dbo.Khoahoc_Chuyende(MaID, KhoahocID, ChuyendeID)
                                            VALUES (@MaID, @KhoahocID, @ChuyendeID)";
            var queryCourseTeacherInsert = @"INSERT INTO dbo.Khoahoc_Giangvien(MaID, KhoahocID, GiangvienID)
                                            VALUES (@MaID, @KhoahocID, @GiangvienID)";
            var queryCourseStudentInsert = @"INSERT INTO dbo.Khoahoc_Hocvien(MaID, KhoahocID, HocvienID)
                                            VALUES (@MaID, @KhoahocID, @HocvienID)";


            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var transaction = conn.BeginTransaction();

                try
                {
                    //Step 1: Insert course
                    using (var cmd = new SqlCommand(queryCourseInsert, conn, transaction))
                    {
                        createDto.courseDto.MaID = Guid.NewGuid().ToString();
                        cmd.Parameters.AddWithValue("@MaID", createDto.courseDto.MaID == null ? Guid.NewGuid().ToString()
                                                                                            : createDto.courseDto.MaID);
                        cmd.Parameters.AddWithValue("@Ten", createDto.courseDto.Ten);
                        cmd.Parameters.AddWithValue("@Mota", createDto.courseDto.Mota);
                        cmd.Parameters.AddWithValue("@Diemdat", createDto.courseDto.Diemdat);
                        cmd.Parameters.AddWithValue("@ChungchiID", createDto.courseDto.ChungchiID);
                        cmd.Parameters.AddWithValue("@LichhocID", createDto.courseDto.LichhocID);
                        if (await cmd.ExecuteNonQueryAsync() < 0)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }

                    //Step 2: Insert Topics list
                    foreach (var topic in createDto.courseTopicDtos)
                    {
                        using (var cmd = new SqlCommand(queryCourseTopicInsert, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@MaID", topic.MaID == null ? Guid.NewGuid().ToString()
                                                                                    : topic.MaID);
                            cmd.Parameters.AddWithValue("@KhoahocID", createDto.courseDto.MaID);
                            cmd.Parameters.AddWithValue("@ChuyendeID", topic.MaID);

                            if (await cmd.ExecuteNonQueryAsync() < 0)
                            {
                                transaction.Rollback();
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
                            cmd.Parameters.AddWithValue("@GiangvienID", teacher.MaID);

                            if (await cmd.ExecuteNonQueryAsync() < 0)
                            {
                                transaction.Rollback();
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
                            cmd.Parameters.AddWithValue("@HocvienID", student.MaID);

                            if (await cmd.ExecuteNonQueryAsync() < 0)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {

                    transaction.Rollback();
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
                                    	UpdatedAt = SYSDATETIME(),
                                    	UpdatedBy = ''
                                    WHERE MaID = @MaID";
            var querycourseTopicDelete = @"DELETE dbo.Khoahoc_Chuyende WHERE KhoahocID = @MaID";
            var querycourseTeacherDelete = @"DELETE dbo.Khoahoc_Giangvien WHERE KhoahocID = @MaID";
            var querycourseStudentDelete = @"DELETE dbo.Khoahoc_Hocvien WHERE KhoahocID = @MaID";

            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var transaction = conn.BeginTransaction();
                try
                {
                    using (var cmd = new SqlCommand(querycourseTopicDelete, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@MaID", updateDto.courseUpdate.MaID);

                        if (await cmd.ExecuteNonQueryAsync() < 0)
                        {
                            await transaction.RollbackAsync();
                            return false;
                        }
                    }
                    using (var cmd = new SqlCommand(querycourseTeacherDelete, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@MaID", updateDto.courseUpdate.MaID);

                        if (await cmd.ExecuteNonQueryAsync() < 0)
                        {
                            await transaction.RollbackAsync();
                            return false;
                        }
                    }
                    using (var cmd = new SqlCommand(querycourseStudentDelete, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@MaID", updateDto.courseUpdate.MaID);

                        if (await cmd.ExecuteNonQueryAsync() < 0)
                        {
                            await transaction.RollbackAsync();
                            return false;
                        }
                    }

                    if (await courserelationInsert(conn, transaction, updateDto))
                    {
                        return false;
                    }
                    using (var cmd = new SqlCommand(querycourseUpdate, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Ten", updateDto.courseUpdate.Ten);
                        cmd.Parameters.AddWithValue("@Mota", updateDto.courseUpdate.Mota);
                        cmd.Parameters.AddWithValue("@Diemdat", updateDto.courseUpdate.Diemdat);
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
        private async Task<bool> courserelationInsert(SqlConnection conn, SqlTransaction transaction, CourseUpdateDto dto)
        {
            var queryCourseTopicInsert = @"INSERT INTO dbo.Khoahoc_Chuyende(MaID, KhoahocID, ChuyendeID)
                                            VALUES (@MaID, @KhoahocID, @ChuyendeID)";
            var queryCourseTeacherInsert = @"INSERT INTO dbo.Khoahoc_Giangvien(MaID, KhoahocID, GiangvienID)
                                            VALUES (@MaID, @KhoahocID, @GiangvienID)";
            var queryCourseStudentInsert = @"INSERT INTO dbo.Khoahoc_Hocvien(MaID, KhoahocID, HocvienID)
                                            VALUES (@MaID, @KhoahocID, @HocvienID)";
            try
            {
                //Step 1: Insert course

                //Step 2: Insert Topics list
                foreach (var topic in dto.courseTopics)
                {
                    using (var cmd = new SqlCommand(queryCourseTopicInsert, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@MaID", topic.MaID == null ? Guid.NewGuid().ToString()
                                                                                : topic.MaID);
                        cmd.Parameters.AddWithValue("@KhoahocID", dto.courseUpdate.MaID);
                        cmd.Parameters.AddWithValue("@ChuyendeID", topic.MaID);

                        if (await cmd.ExecuteNonQueryAsync() < 0)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                }
                //Step 3: Insert Teachers list
                foreach (var teacher in dto.courseTeachers)
                {
                    using (var cmd = new SqlCommand(queryCourseTeacherInsert, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@MaID", teacher.MaID == null ? Guid.NewGuid().ToString()
                                                                                : teacher.MaID);
                        cmd.Parameters.AddWithValue("@KhoahocID", dto.courseUpdate.MaID);
                        cmd.Parameters.AddWithValue("@GiangvienID", teacher.MaID);

                        if (await cmd.ExecuteNonQueryAsync() < 0)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                }
                //Step 4: Insert Students list
                foreach (var student in dto.courseStudents)
                {
                    using (var cmd = new SqlCommand(queryCourseStudentInsert, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@MaID", student.MaID == null ? Guid.NewGuid().ToString()
                                                                                : student.MaID);
                        cmd.Parameters.AddWithValue("@KhoahocID", dto.courseUpdate.MaID);
                        cmd.Parameters.AddWithValue("@HocvienID", student.MaID);

                        if (await cmd.ExecuteNonQueryAsync() < 0)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                }
                transaction.Commit();
            }
            catch (Exception ex)
            {

                transaction.Rollback();
                _logger.LogError(ex.Message);
                return false;
            }
            return true;
        }
    }

}

