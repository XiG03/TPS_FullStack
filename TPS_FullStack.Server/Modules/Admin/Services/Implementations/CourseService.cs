using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class CourseService : ICourseService
    {
        private readonly IConfiguration _configuration;
        private readonly ICoursesRepository _coursesRepository;
        private readonly ICourseTopicRepository _courseTopicRepository;
        private readonly ICourseTeacherRepository _courseTeacherRepository;
        private readonly ICourseStudentRepository _courseStudentRepository;
        private readonly ILogger<CourseService> _logger;

        public CourseService(IConfiguration configuration, ICoursesRepository coursesRepository,
                             ICourseTopicRepository courseTopicRepository, ICourseTeacherRepository courseTeacherRepository,
                             ICourseStudentRepository courseStudentRepository, ILogger<CourseService> logger)
        {
            _configuration = configuration;
            _coursesRepository = coursesRepository;
            _courseTopicRepository = courseTopicRepository;
            _courseTeacherRepository = courseTeacherRepository;
            _courseStudentRepository = courseStudentRepository;
            _logger = logger;
        }

        // Logic đệ quy tính toán danh sách lịch học dựa vào file code cũ
        private List<ScheduleModel> CreateSchedules(List<ScheduleModel> schedules, List<int> studyDays, DateTime ngayHienTai, decimal soBuoiHoc, int count, string khoahocID)
        {
            if (count >= soBuoiHoc)
            {
                return schedules;
            }

            schedules.Add(new ScheduleModel
            {
                LichhocID = Guid.NewGuid().ToString(),
                KhoahocID = khoahocID,
                Ngaydukien = ngayHienTai,
            });

            int loca = count % studyDays.Count;
            DateTime ngayTiepTheo = ngayHienTai.AddDays(loca == studyDays.Count - 1
                                                            ? studyDays[0] - studyDays[loca] + 7
                                                            : studyDays[loca + 1] - studyDays[loca]);

            return CreateSchedules(schedules, studyDays, ngayTiepTheo, soBuoiHoc, count + 1, khoahocID);
        }

        public async Task<ServiceDefault<CourseCreateResponse>> CreateCourseAsync(CourseCreateRequest createRequest)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        string courseId = string.IsNullOrEmpty(createRequest.KhoahocID) ? Guid.NewGuid().ToString() : createRequest.KhoahocID;

                        // Gọi tới Repository truyền các parameter riêng lẻ
                        await _coursesRepository.CreateAsync(
                            conn, trans,
                            courseId, createRequest.Ten, createRequest.Mota, createRequest.Diemdat ?? 0,
                            createRequest.ChungchiID, createRequest.Thu, createRequest.Thoiluonghoc ?? 0,
                            createRequest.Sobuoihoc ?? 0, createRequest.Thoiluongthi ?? 0, createRequest.Ngaybatdau,
                            createRequest.Socauhoi ?? 0, DateTime.UtcNow, "Admin", null, null, null, null
                        );

                        // Lưu Topics, Teachers, Students
                        if (createRequest.Topics != null)
                            foreach (var topic in createRequest.Topics)
                                await _courseTopicRepository.CreateAsync(conn, trans, Guid.NewGuid().ToString(), courseId, topic.ChuyendeID, topic.Socauhoi);

                        if (createRequest.Teachers != null)
                            foreach (var teacher in createRequest.Teachers)
                                await _courseTeacherRepository.CreateAsync(conn, trans, Guid.NewGuid().ToString(), courseId, teacher.GiangvienID);

                        if (createRequest.Students != null)
                            foreach (var student in createRequest.Students)
                                await _courseStudentRepository.CreateAsync(conn, trans, Guid.NewGuid().ToString(), courseId, student.HocvienID, student.Diem, student.Dieuchinh);

                        // Logic tính và lưu lịch học (Schedule) kế thừa từ code cũ
                        if (!string.IsNullOrEmpty(createRequest.Thu) && createRequest.Ngaybatdau.HasValue && createRequest.Sobuoihoc.HasValue)
                        {
                            var studyDays = new List<int>();
                            foreach (var item in createRequest.Thu.Split("#"))
                            {
                                if (int.TryParse(item, out int number)) studyDays.Add(number);
                            }

                            if (studyDays.Count > 0)
                            {
                                var schedules = new List<ScheduleModel>();
                                schedules = CreateSchedules(schedules, studyDays, createRequest.Ngaybatdau.Value, createRequest.Sobuoihoc.Value, 0, courseId);

                                foreach (var sch in schedules)
                                {
                                    var querySchedule = @"INSERT INTO dbo.Lichhoc (MaID, KhoahocID, Ngaydukien) VALUES (@MaID, @KhoahocID, @Ngaydukien)";
                                    using (var cmd = new SqlCommand(querySchedule, conn, trans))
                                    {
                                        cmd.Parameters.AddWithValue("@MaID", sch.LichhocID);
                                        cmd.Parameters.AddWithValue("@KhoahocID", sch.KhoahocID);
                                        cmd.Parameters.AddWithValue("@Ngaydukien", sch.Ngaydukien ?? (object)DBNull.Value);
                                        await cmd.ExecuteNonQueryAsync();
                                    }
                                }
                            }
                        }

                        await trans.CommitAsync();

                        return new ServiceDefault<CourseCreateResponse>
                        {
                            statusCode = StatusCodes.Status200OK,
                            Message = "Tạo khóa học thành công",
                            Data = new CourseCreateResponse { KhoahocID = courseId }
                        };
                    }
                    catch (Exception ex)
                    {
                        await trans.RollbackAsync();
                        _logger.LogError(ex.Message);
                        return new ServiceDefault<CourseCreateResponse>
                        {
                            statusCode = StatusCodes.Status500InternalServerError,
                            Message = "Tạo khóa học không thành công: " + ex.Message,
                            Data = null
                        };
                    }
                }
            }
        }

        public async Task<ServiceDefault<CourseUpdateResponse>> UpdateCourseAsync(CourseUpdateRequest updateRequest)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var existCourse = await _coursesRepository.GetByIDAsync(conn, updateRequest.KhoahocID, null);
                if (existCourse == null)
                {
                    return new ServiceDefault<CourseUpdateResponse> { statusCode = StatusCodes.Status404NotFound, Message = "Không tìm thấy khóa học", Data = null };
                }

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        // Gọi tới Repository truyền các parameter riêng lẻ
                        await _coursesRepository.UpdateAsync(
                            conn, trans,
                            updateRequest.KhoahocID, updateRequest.Ten, updateRequest.Mota, updateRequest.Diemdat ?? 0,
                            updateRequest.ChungchiID, updateRequest.Thu, updateRequest.Thoiluonghoc ?? 0,
                            updateRequest.Sobuoihoc ?? 0, updateRequest.Thoiluongthi ?? 0, updateRequest.Ngaybatdau,
                            updateRequest.Socauhoi ?? 0, null, null, DateTime.UtcNow, "Admin", null, null
                        );

                        // --- Cập nhật Mappings (Diffing) ---
                        // Topics
                        var allTopics = await _courseTopicRepository.GetAllAsync(conn);
                        var oldTopics = new List<CourseTopicModel>();
                        foreach (var t in allTopics) if (t.KhoahocID == updateRequest.KhoahocID) oldTopics.Add(t);
                        foreach (var oldT in oldTopics)
                        {
                            bool isFound = false;
                            if (updateRequest.Topics != null) foreach (var newT in updateRequest.Topics) if (newT.MaID == oldT.MaID) { isFound = true; break; }
                            if (!isFound) await _courseTopicRepository.DeleteAsync(conn, trans, oldT.MaID);
                        }
                        if (updateRequest.Topics != null)
                        {
                            foreach (var newT in updateRequest.Topics)
                            {
                                if (string.IsNullOrEmpty(newT.MaID)) await _courseTopicRepository.CreateAsync(conn, trans, Guid.NewGuid().ToString(), updateRequest.KhoahocID, newT.ChuyendeID, newT.Socauhoi);
                                else await _courseTopicRepository.UpdateAsync(conn, trans, newT.MaID, updateRequest.KhoahocID, newT.ChuyendeID, newT.Socauhoi);
                            }
                        }

                        // Teachers
                        var allTeachers = await _courseTeacherRepository.GetAllAsync(conn);
                        var oldTeachers = new List<CourseTeacherModel>();
                        foreach (var t in allTeachers) if (t.KhoahocID == updateRequest.KhoahocID) oldTeachers.Add(t);
                        foreach (var oldT in oldTeachers)
                        {
                            bool isFound = false;
                            if (updateRequest.Teachers != null) foreach (var newT in updateRequest.Teachers) if (newT.MaID == oldT.MaID) { isFound = true; break; }
                            if (!isFound) await _courseTeacherRepository.DeleteAsync(conn, trans, oldT.MaID);
                        }
                        if (updateRequest.Teachers != null)
                        {
                            foreach (var newT in updateRequest.Teachers)
                            {
                                if (string.IsNullOrEmpty(newT.MaID)) await _courseTeacherRepository.CreateAsync(conn, trans, Guid.NewGuid().ToString(), updateRequest.KhoahocID, newT.GiangvienID);
                                else await _courseTeacherRepository.UpdateAsync(conn, trans, newT.MaID, updateRequest.KhoahocID, newT.GiangvienID);
                            }
                        }

                        // Students
                        var allStudents = await _courseStudentRepository.GetAllAsync(conn);
                        var oldStudents = new List<CourseStudentModel>();
                        foreach (var s in allStudents) if (s.KhoahocID == updateRequest.KhoahocID) oldStudents.Add(s);
                        foreach (var oldS in oldStudents)
                        {
                            bool isFound = false;
                            if (updateRequest.Students != null) foreach (var newS in updateRequest.Students) if (newS.MaID == oldS.MaID) { isFound = true; break; }
                            if (!isFound) await _courseStudentRepository.DeleteAsync(conn, trans, oldS.MaID);
                        }
                        if (updateRequest.Students != null)
                        {
                            foreach (var newS in updateRequest.Students)
                            {
                                if (string.IsNullOrEmpty(newS.MaID)) await _courseStudentRepository.CreateAsync(conn, trans, Guid.NewGuid().ToString(), updateRequest.KhoahocID, newS.HocvienID, newS.Diem, newS.Dieuchinh);
                                else await _courseStudentRepository.UpdateAsync(conn, trans, newS.MaID, updateRequest.KhoahocID, newS.HocvienID, newS.Diem, newS.Dieuchinh);
                            }
                        }

                        // --- Tính lại Lịch Học (Giống logic Repository cũ xóa đi tạo lại) ---
                        var queryDeleteSchedules = "DELETE FROM dbo.Lichhoc WHERE KhoahocID = @KhoahocID";
                        using (var cmdDel = new SqlCommand(queryDeleteSchedules, conn, trans))
                        {
                            cmdDel.Parameters.AddWithValue("@KhoahocID", updateRequest.KhoahocID);
                            await cmdDel.ExecuteNonQueryAsync();
                        }

                        if (!string.IsNullOrEmpty(updateRequest.Thu) && updateRequest.Ngaybatdau.HasValue && updateRequest.Sobuoihoc.HasValue)
                        {
                            var studyDays = new List<int>();
                            foreach (var item in updateRequest.Thu.Split("#"))
                            {
                                if (int.TryParse(item, out int number)) studyDays.Add(number);
                            }

                            if (studyDays.Count > 0)
                            {
                                var schedules = new List<ScheduleModel>();
                                schedules = CreateSchedules(schedules, studyDays, updateRequest.Ngaybatdau.Value, updateRequest.Sobuoihoc.Value, 0, updateRequest.KhoahocID);

                                foreach (var sch in schedules)
                                {
                                    var querySchedule = @"INSERT INTO dbo.Lichhoc (MaID, KhoahocID, Ngaydukien) VALUES (@MaID, @KhoahocID, @Ngaydukien)";
                                    using (var cmd = new SqlCommand(querySchedule, conn, trans))
                                    {
                                        cmd.Parameters.AddWithValue("@MaID", sch.LichhocID);
                                        cmd.Parameters.AddWithValue("@KhoahocID", sch.KhoahocID);
                                        cmd.Parameters.AddWithValue("@Ngaydukien", sch.Ngaydukien ?? (object)DBNull.Value);
                                        await cmd.ExecuteNonQueryAsync();
                                    }
                                }
                            }
                        }

                        await trans.CommitAsync();

                        return new ServiceDefault<CourseUpdateResponse>
                        {
                            statusCode = StatusCodes.Status200OK,
                            Message = "Cập nhật thành công",
                            Data = new CourseUpdateResponse { KhoahocID = updateRequest.KhoahocID }
                        };
                    }
                    catch (Exception ex)
                    {
                        await trans.RollbackAsync();
                        _logger.LogError(ex.Message);
                        return new ServiceDefault<CourseUpdateResponse> { statusCode = StatusCodes.Status500InternalServerError, Message = "Lỗi: " + ex.Message, Data = null };
                    }
                }
            }
        }

        public async Task<ServiceDefault<List<CourseResponse>>> GetAllCoursesAsync()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var courses = await _coursesRepository.GetAllAsync(conn);
                var result = new List<CourseResponse>();

                foreach (var c in courses)
                {
                    result.Add(new CourseResponse { KhoahocID = c.KhoahocID, Ten = c.Ten, Mota = c.Mota });
                }

                return new ServiceDefault<List<CourseResponse>> { statusCode = StatusCodes.Status200OK, Message = "Thành công", Data = result };
            }
        }

        public async Task<ServiceDefault<CourseDetailResponse>> GetCourseDetailAsync(string? KhoahocID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var course = await _coursesRepository.GetByIDAsync(conn, KhoahocID, null);
                if (course == null) return new ServiceDefault<CourseDetailResponse> { statusCode = StatusCodes.Status404NotFound, Message = "Không tìm thấy", Data = null };

                var detail = new CourseDetailResponse
                {
                    KhoahocID = course.KhoahocID,
                    Ten = course.Ten,
                    Mota = course.Mota,
                    Diemdat = course.Diemdat,
                    ChungchiID = course.ChungchiID,
                    Thu = course.Thu,
                    Thoiluonghoc = course.Thoiluonghoc,
                    Sobuoihoc = course.Sobuoihoc,
                    Thoiluongthi = course.Thoiluongthi,
                    Ngaybatdau = course.Ngaybatdau,
                    Socauhoi = course.Socauhoi
                };

                var topics = await _courseTopicRepository.GetAllAsync(conn);
                foreach (var t in topics) if (t.KhoahocID == KhoahocID) detail.Topics.Add(new CourseTopicResponse { MaID = t.MaID, ChuyendeID = t.ChuyendeID, Socauhoi = t.Socauhoi });

                var teachers = await _courseTeacherRepository.GetAllAsync(conn);
                foreach (var t in teachers) if (t.KhoahocID == KhoahocID) detail.Teachers.Add(new CourseTeacherResponse { MaID = t.MaID, GiangvienID = t.GiangvienID });

                var students = await _courseStudentRepository.GetAllAsync(conn);
                foreach (var s in students) if (s.KhoahocID == KhoahocID) detail.Students.Add(new CourseStudentResponse { MaID = s.MaID, HocvienID = s.HocvienID, Diem = s.Diem, Dieuchinh = s.Dieuchinh });

                return new ServiceDefault<CourseDetailResponse> { statusCode = StatusCodes.Status200OK, Message = "Thành công", Data = detail };
            }
        }

        public async Task<ServiceDefault<bool>> DeleteCourseAsync(string? KhoahocID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        await _coursesRepository.DeleteAsync(conn, trans, KhoahocID, DateTime.UtcNow, "Admin");
                        await trans.CommitAsync();
                        return new ServiceDefault<bool> { statusCode = StatusCodes.Status200OK, Message = "Xóa thành công", Data = true };
                    }
                    catch (Exception ex)
                    {
                        await trans.RollbackAsync();
                        _logger.LogError(ex.Message);
                        return new ServiceDefault<bool> { statusCode = StatusCodes.Status500InternalServerError, Message = "Lỗi: " + ex.Message, Data = false };
                    }
                }
            }
        }
    }
}