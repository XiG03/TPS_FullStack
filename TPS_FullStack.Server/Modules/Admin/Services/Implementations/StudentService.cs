using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class StudentService : IStudentService
    {
        private readonly IConfiguration _configuration;
        private readonly IStudentRepository _studentRepository;
        private readonly ICertificateStudentRepository _certificateStudentRepository;
        private readonly IScheduleStudentAttendanceRepository _scheduleStudentRepository;
        private readonly ISchedulesRepository _scheduleRepository;
        private readonly ILogger<StudentService> _logger;

        public StudentService(IConfiguration configuration, IStudentRepository studentRepository,
                              ICertificateStudentRepository certificateStudentRepository, ILogger<StudentService> logger, IScheduleStudentAttendanceRepository scheduleStudentAttendanceRepository,
                                ISchedulesRepository scheduleRepository)
        {
            _configuration = configuration;
            _studentRepository = studentRepository;
            _certificateStudentRepository = certificateStudentRepository;
            _logger = logger;
            _scheduleStudentRepository = scheduleStudentAttendanceRepository;
            _scheduleRepository = scheduleRepository;
        }

        public async Task<ServiceDefault<ScheduleStudentAttendanceResponse>> StudentCheckinAsync(string? HocvienID, string? LichhocID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        string MaID = Guid.NewGuid().ToString();
                        await _scheduleStudentRepository.CreateAsync(conn, trans, MaID, LichhocID, HocvienID, null, DateTime.UtcNow, DateTime.UtcNow);

                        await trans.CommitAsync();
                        return new ServiceDefault<ScheduleStudentAttendanceResponse>
                        {
                            statusCode = StatusCodes.Status200OK,
                            Message = "Check in thanh cong",
                            Data = null
                        };

                    }
                    catch (Exception ex)
                    {
                        return new ServiceDefault<ScheduleStudentAttendanceResponse>
                        {
                            statusCode = StatusCodes.Status500InternalServerError,
                            Message = "Server chua diem danh",
                            Data = null
                        };
                    }
                }

            }
            throw new NotImplementedException();
        }

        public async Task<ServiceDefault<StudentCreateResponse>> StudentCreateAsync(StudentCreateRequest createRequest)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        string id = string.IsNullOrEmpty(createRequest.HocvienID) ? Guid.NewGuid().ToString() : createRequest.HocvienID;

                        await _studentRepository.CreateAsync(
                            conn, trans, id, createRequest.Hoten, createRequest.Ngaysinh, createRequest.Gioitinh,
                            createRequest.Email, createRequest.Diachi, createRequest.Dienthoai,
                            DateTime.UtcNow, "Admin", null, null, null, null
                        );

                        await trans.CommitAsync();

                        return new ServiceDefault<StudentCreateResponse>
                        {
                            statusCode = StatusCodes.Status200OK,
                            Message = "Tạo học viên thành công",
                            Data = new StudentCreateResponse { HocvienID = id }
                        };
                    }
                    catch (Exception ex)
                    {
                        await trans.RollbackAsync();
                        _logger.LogError(ex, "Lỗi khi tạo học viên mới");
                        return new ServiceDefault<StudentCreateResponse>
                        {
                            statusCode = StatusCodes.Status500InternalServerError,
                            Message = "Tạo học viên không thành công: " + ex.Message,
                            Data = null
                        };
                    }
                }
            }
        }

        public async Task<ServiceDefault<bool>> StudentDeleteAsync(string? HocvienID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        // Xóa mềm: truyền DeletedAt và DeletedBy
                        await _studentRepository.DeleteAsync(conn, trans, HocvienID, DateTime.UtcNow, "Admin");
                        await trans.CommitAsync();

                        return new ServiceDefault<bool>
                        {
                            statusCode = StatusCodes.Status200OK,
                            Message = "Xóa học viên thành công",
                            Data = true
                        };
                    }
                    catch (Exception ex)
                    {
                        await trans.RollbackAsync();
                        _logger.LogError(ex, "Lỗi khi xóa học viên");
                        return new ServiceDefault<bool>
                        {
                            statusCode = StatusCodes.Status500InternalServerError,
                            Message = "Xóa học viên không thành công: " + ex.Message,
                            Data = false
                        };
                    }
                }
            }
        }

        public async Task<ServiceDefault<List<StudentResponse>>> StudentGetAllAsync()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var students = await _studentRepository.GetAllAsync(conn);
                var result = new List<StudentResponse>();

                // Dùng vòng lặp thuần, không LINQ
                foreach (var s in students)
                {
                    result.Add(new StudentResponse
                    {
                        HocvienID = s.MaID,
                        Hoten = s.Hoten,
                        Gioitinh = s.Gioitinh,
                        Email = s.Email,
                        Diachi = s.Diachi,
                        Dienthoai = s.Dienthoai
                    });
                }

                return new ServiceDefault<List<StudentResponse>>
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Lấy danh sách học viên thành công",
                    Data = result
                };
            }
        }

        public async Task<ServiceDefault<StudentDetailResponse>> StudentGetByIDAsync(string? HocvienID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                var student = await _studentRepository.GetByIDAsync(conn, HocvienID);
                if (student == null)
                {
                    return new ServiceDefault<StudentDetailResponse>
                    {
                        statusCode = StatusCodes.Status404NotFound,
                        Message = "Không tìm thấy học viên",
                        Data = null
                    };
                }

                var detail = new StudentDetailResponse
                {
                    HocvienID = student.MaID,
                    Hoten = student.Hoten,
                    Gioitinh = student.Gioitinh,
                    Email = student.Email,
                    Diachi = student.Diachi,
                    Dienthoai = student.Dienthoai,
                    Certificates = new List<StudentCertificateDetailResponse>()
                };

                // Lấy danh sách Chứng chỉ của học viên
                var certs = await _certificateStudentRepository.GetByStudentIDAsync(conn, HocvienID);
                foreach (var c in certs)
                {
                    detail.Certificates.Add(new StudentCertificateDetailResponse
                    {
                        MaID = c.MaID,
                        ChungchiID = c.ChungchiID,
                        TenChungchi = c.TenChungchi,
                        Mota = c.Mota,
                        Donvicap = c.Donvicap,
                        Ngaycap = c.Ngaycap,
                        Ngayhethan = c.Ngayhethan
                    });
                }

                return new ServiceDefault<StudentDetailResponse>
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Lấy thông tin chi tiết học viên thành công",
                    Data = detail
                };
            }
        }

        public async Task<ServiceDefault<List<ScheduleResponse>>> StudentGetScheduleByIDAsync(string? HocvienID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var result = new List<ScheduleResponse>();
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                var schedules = await _scheduleRepository.GetByStudentIDAsync(conn, HocvienID);
                foreach (var schedule in schedules)
                {
                    result.Add(new ScheduleResponse
                    {
                        LichhocID = schedule.LichhocID,
                        KhoahocID = schedule.KhoahocID,
                        TenKhoahoc = schedule.TenKhoahoc,
                        ChuyendeID = schedule.ChuyendeID,
                        TenChuyende = schedule.TenChuyende,
                        GiangvienID = schedule.GiangvienID,
                        TenGiangvien = schedule.TenGiangvien,
                        Ngaydukien = schedule.Ngaydukien,
                        Batdaudukien = schedule.Batdaudukien,
                        Ketthucdukien = schedule.Ketthucdukien,
                        Ngaythucte = schedule.Ngaythucte,
                        Batdauthucte = schedule.Batdauthucte,
                        Ketthucthucte = schedule.Ketthucthucte,
                        Trangthai = schedule.Trangthai
                    });
                }
            }
            return new ServiceDefault<List<ScheduleResponse>>
            {
                statusCode = StatusCodes.Status200OK,
                Message = "Tim thay lich hoc",
                Data = result
            };
            throw new NotImplementedException();
        }

        public async Task<ServiceDefault<ScheduleDetailResponse>> StudentGetScheduleDetailAsync(string? HocvienID, string? LichhocID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                var schedule = await _scheduleRepository.GetByIDAsync(conn, LichhocID);
                if (schedule == null)
                {
                    return new ServiceDefault<ScheduleDetailResponse>
                    {
                        statusCode = StatusCodes.Status200OK,
                        Message = "Khong co lich chi tiet",
                        Data = null
                    };
                }
                return new ServiceDefault<ScheduleDetailResponse>
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Thanh cong",
                    Data = new ScheduleDetailResponse
                    {
                        LichhocID = schedule.LichhocID,
                        KhoahocID = schedule.KhoahocID,
                        TenKhoahoc = schedule.TenKhoahoc,
                        ChuyendeID = schedule.ChuyendeID,
                        TenChuyende = schedule.TenChuyende,
                        GiangvienID = schedule.GiangvienID,
                        TenGiangvien = schedule.TenGiangvien,
                        Ngaydukien = schedule.Ngaydukien,
                        Batdaudukien = schedule.Batdaudukien,
                        Ketthucdukien = schedule.Ketthucdukien,
                        Ngaythucte = schedule.Ngaythucte,
                        Batdauthucte = schedule.Batdauthucte,
                        Ketthucthucte = schedule.Ketthucthucte
                    }
                };
            }
            throw new NotImplementedException();
        }

        public async Task<ServiceDefault<StudentUpdateResponse>> StudentUpdateAsync(StudentUpdateRequest updateRequest)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                // 1. Kiểm tra tồn tại
                var existStudent = await _studentRepository.GetByIDAsync(conn, updateRequest.HocvienID);
                if (existStudent == null)
                {
                    return new ServiceDefault<StudentUpdateResponse>
                    {
                        statusCode = StatusCodes.Status404NotFound,
                        Message = "Không tìm thấy học viên để cập nhật",
                        Data = null
                    };
                }

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        // 2. Cập nhật thông tin
                        await _studentRepository.UpdateAsync(
                            conn, trans, updateRequest.HocvienID, updateRequest.Hoten, updateRequest.Ngaysinh, updateRequest.Gioitinh,
                            updateRequest.Email, updateRequest.Diachi, updateRequest.Dienthoai,
                            null, null, DateTime.UtcNow, "Admin", null, null
                        );

                        await trans.CommitAsync();

                        return new ServiceDefault<StudentUpdateResponse>
                        {
                            statusCode = StatusCodes.Status200OK,
                            Message = "Cập nhật học viên thành công",
                            Data = new StudentUpdateResponse { HocvienID = updateRequest.HocvienID }
                        };
                    }
                    catch (Exception ex)
                    {
                        await trans.RollbackAsync();
                        _logger.LogError(ex, "Lỗi khi cập nhật học viên");
                        return new ServiceDefault<StudentUpdateResponse>
                        {
                            statusCode = StatusCodes.Status500InternalServerError,
                            Message = "Cập nhật học viên không thành công: " + ex.Message,
                            Data = null
                        };
                    }
                }
            }
        }
    }
}