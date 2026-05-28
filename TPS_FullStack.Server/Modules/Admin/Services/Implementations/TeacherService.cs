using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Identity.Client;
using Microsoft.OpenApi.Services;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class TeacherService : ITeacherService
    {
        private readonly IConfiguration _configuration;
        private readonly ITeacherRepository _teacherRepository;
        private readonly ITopicTeacherRepository _topicTeacherRepository;
        private readonly ISchedulesRepository _scheduleRepository;
        private readonly IScheduleTeacherAttendanceRepository _attendanceRepository;

        public TeacherService(IConfiguration configuration, ITeacherRepository teacherRepository, ITopicTeacherRepository topicTeacherRepository, ISchedulesRepository scheduleRepository,
                            IScheduleTeacherAttendanceRepository attendanceRepository)
        {
            _configuration = configuration;
            _teacherRepository = teacherRepository;
            _topicTeacherRepository = topicTeacherRepository;
            _scheduleRepository = scheduleRepository;
            _attendanceRepository = attendanceRepository;
        }


        public async Task<ServiceDefault<TeacherCreateResponse>> TeacherCreateAsync(TeacherCreateResquest createResquest)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        // Nếu GiangvienID rỗng thì tạo mới Guid
                        string gvId = string.IsNullOrEmpty(createResquest.GiangvienID) ? Guid.NewGuid().ToString() : createResquest.GiangvienID;

                        // Tạo giảng viên
                        await _teacherRepository.CreateAsync(conn, trans, gvId, createResquest.Hoten, createResquest.Ngaysinh,
                            createResquest.Gioitinh, createResquest.Email, createResquest.Diachi, createResquest.Dienthoai,
                            DateTime.UtcNow, "Admin", null, null, null, null);

                        // Thêm danh sách chuyên đề phụ trách
                        if (createResquest.Topics != null)
                        {
                            foreach (var topic in createResquest.Topics)
                            {
                                string mappingId = Guid.NewGuid().ToString();
                                await _topicTeacherRepository.CreateAsync(conn, trans, mappingId, gvId, topic.ChuyendeID);
                            }
                        }

                        await trans.CommitAsync();

                        return new ServiceDefault<TeacherCreateResponse>
                        {
                            statusCode = StatusCodes.Status200OK,
                            Message = "Tạo giảng viên thành công",
                            Data = new TeacherCreateResponse { GiangvienID = gvId }
                        };
                    }
                    catch (Exception ex)
                    {
                        await trans.RollbackAsync();
                        return new ServiceDefault<TeacherCreateResponse>
                        {
                            statusCode = StatusCodes.Status500InternalServerError,
                            Message = "Tạo giảng viên không thành công: " + ex.Message,
                            Data = null
                        };
                    }
                }
            }
        }

        public async Task<ServiceDefault<bool>> TeacherDeleteAsync(string? MaID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        // Xóa mềm Giảng viên (theo thiết kế repo là set Khongsudung = 1)
                        await _teacherRepository.DeleteAsync(conn, trans, MaID, DateTime.UtcNow, "Admin");
                        await trans.CommitAsync();

                        return new ServiceDefault<bool>
                        {
                            statusCode = StatusCodes.Status200OK,
                            Message = "Xóa giảng viên thành công",
                            Data = true
                        };
                    }
                    catch (Exception ex)
                    {
                        await trans.RollbackAsync();
                        return new ServiceDefault<bool>
                        {
                            statusCode = StatusCodes.Status500InternalServerError,
                            Message = "Xóa giảng viên không thành công: " + ex.Message,
                            Data = false
                        };
                    }
                }
            }
        }

        public async Task<ServiceDefault<List<TeacherResponse>>> TeacherGetAllAsync()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var listGv = await _teacherRepository.GetAllAsync(conn);

                var responseList = new List<TeacherResponse>();
                foreach (var gv in listGv)
                {
                    var res = new TeacherResponse();
                    res.MaID = gv.MaID;
                    res.Hoten = gv.Hoten;
                    res.Gioitinh = gv.Gioitinh;
                    res.Email = gv.Email;
                    res.Diachi = gv.Diachi;
                    res.Dienthoai = gv.Dienthoai;

                    responseList.Add(res);
                }

                return new ServiceDefault<List<TeacherResponse>>
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Lấy danh sách thành công",
                    Data = responseList
                };
            }
        }

        public async Task<ServiceDefault<TeacherDetailResponse>> TeacherGetByIDAsync(string? MaID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var gv = await _teacherRepository.GetByIDAsync(conn, MaID);

                // Check nếu không tồn tại hoặc rỗng
                if (gv == null || string.IsNullOrEmpty(gv.MaID))
                {
                    return new ServiceDefault<TeacherDetailResponse>
                    {
                        statusCode = StatusCodes.Status404NotFound,
                        Message = "Không tìm thấy giảng viên",
                        Data = null
                    };
                }

                var detail = new TeacherDetailResponse
                {
                    MaID = gv.MaID,
                    Hoten = gv.Hoten,
                    Gioitinh = gv.Gioitinh,
                    Email = gv.Email,
                    Diachi = gv.Diachi,
                    Dienthoai = gv.Dienthoai,
                    Topics = new List<TeacherTopicResponse>()
                };

                // Lấy danh sách chuyên đề phụ trách
                var topics = await _topicTeacherRepository.GetByTeacherIDAsync(conn, MaID);
                foreach (var t in topics)
                {
                    var tr = new TeacherTopicResponse();
                    tr.MaID = t.MaID;
                    tr.ChuyendeID = t.ChuyendeID;
                    tr.TenChuyende = t.TenChuyende;
                    detail.Topics.Add(tr);
                }

                return new ServiceDefault<TeacherDetailResponse>
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Lấy thông tin thành công",
                    Data = detail
                };
            }
        }

        public async Task<ServiceDefault<List<ScheduleResponse>>> TeacherGetScheduleByIDAsync(string? GiangvienID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var result = new List<ScheduleResponse>();
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var list = await _scheduleRepository.GetAllAsync(conn);
                foreach (var schedule in list)
                {
                    if (schedule.GiangvienID == GiangvienID)
                    {
                        result.Add(new ScheduleResponse
                        {
                            LichhocID = schedule.LichhocID,
                            KhoahocID = schedule.KhoahocID,
                            TenKhoahoc = schedule.TenKhoahoc,
                            ChuyendeID = schedule.ChuyendeID,
                            TenChuyende = schedule.TenChuyende,
                            GiangvienID = schedule.TenGiangvien,
                            Ngaydukien = schedule.Ngaydukien,
                            Batdaudukien = schedule.Batdaudukien,
                            Ketthucdukien = schedule.Ketthucdukien,
                            Ngaythucte = schedule.Ngaythucte,
                            Batdauthucte = schedule.Batdauthucte,
                            Ketthucthucte = schedule.Ketthucthucte,
                        });
                    }
                }

                return new ServiceDefault<List<ScheduleResponse>>
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Tim thanh cong",
                    Data = result
                };
            }
            throw new NotImplementedException();
        }

        public async Task<ServiceDefault<ScheduleDetailResponse>> TeacherGetScheduleDetailAsync(string? GiangvienID, string? LichhocID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var schedule = await _scheduleRepository.GetByIDAsync(conn, LichhocID);

                if (schedule != null)
                {
                    return new ServiceDefault<ScheduleDetailResponse>
                    {
                        statusCode = StatusCodes.Status200OK,
                        Message = "Tim thanh cong",
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
                            Ketthucthucte = schedule.Ketthucthucte,
                        }
                    };
                }
            }
            throw new NotImplementedException();
        }

        public async Task<ServiceDefault<ScheduleTeacherAttendanceResponse>> TeacherCheckinAsync(string? GiangvienID, string? LichhocID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        // Buoc 1: them vao bang diem danh
                        var attendance = Guid.NewGuid().ToString();
                        await _attendanceRepository.CreateAsync(conn, trans, attendance, LichhocID, GiangvienID, null, DateTime.UtcNow, DateTime.UtcNow);

                        // Buoc 2: update bang Lichhoc
                        await _scheduleRepository.UpdateAsync(conn, trans, LichhocID, null, null, GiangvienID, null, null, null, DateTime.UtcNow, DateTime.UtcNow, null);
                        await trans.CommitAsync();

                        return new ServiceDefault<ScheduleTeacherAttendanceResponse>
                        {
                            statusCode = StatusCodes.Status500InternalServerError,
                            Message = "Check in thanh cong",
                            Data = null,
                        };
                    }
                    catch (Exception ex)
                    {
                        return new ServiceDefault<ScheduleTeacherAttendanceResponse>
                        {
                            statusCode = StatusCodes.Status500InternalServerError,
                            Message = "Check in khong thanh cong" + ex.Message,
                            Data = null,
                        };
                    }

                }
            }
            throw new NotImplementedException();
        }

        public async Task<ServiceDefault<TeacherUpdateResponse>> TeacherUpdateAsync(TeacherUpdateResquest updateResquest)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                // 1. Kiểm tra tồn tại
                var existGv = await _teacherRepository.GetByIDAsync(conn, updateResquest.GiangvienID);
                if (existGv == null || string.IsNullOrEmpty(existGv.MaID))
                {
                    return new ServiceDefault<TeacherUpdateResponse>
                    {
                        statusCode = StatusCodes.Status404NotFound,
                        Message = "Không tìm thấy giảng viên",
                        Data = null
                    };
                }

                var existingMappings = await _topicTeacherRepository.GetByTeacherIDAsync(conn, updateResquest.GiangvienID);

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        // 2. Cập nhật thông tin cơ bản
                        await _teacherRepository.UpdateAsync(conn, trans, updateResquest.GiangvienID, updateResquest.Hoten, updateResquest.Ngaysinh,
                            updateResquest.Gioitinh, updateResquest.Email, updateResquest.Diachi, updateResquest.Dienthoai,
                            null, null, DateTime.UtcNow, "Admin", null, null);

                        // 3. Xử lý map Chuyên đề (Topic) bằng Diffing (Không LINQ)
                        // Xóa các Topic mapping cũ bị lược bỏ
                        foreach (var oldMap in existingMappings)
                        {
                            bool isFound = false;
                            if (updateResquest.Topics != null)
                            {
                                foreach (var newMap in updateResquest.Topics)
                                {
                                    if (newMap.MaID == oldMap.MaID)
                                    {
                                        isFound = true;
                                        break;
                                    }
                                }
                            }

                            if (!isFound)
                            {
                                await _topicTeacherRepository.DeleteAsync(conn, trans, oldMap.MaID);
                            }
                        }

                        // Thêm mới hoặc Cập nhật các Topic mapping
                        if (updateResquest.Topics != null)
                        {
                            foreach (var newMap in updateResquest.Topics)
                            {
                                if (string.IsNullOrEmpty(newMap.MaID)) // Thêm mới
                                {
                                    string newMapId = Guid.NewGuid().ToString();
                                    await _topicTeacherRepository.CreateAsync(conn, trans, newMapId, updateResquest.GiangvienID, newMap.ChuyendeID);
                                }
                                else // Update
                                {
                                    await _topicTeacherRepository.UpdateAsync(conn, trans, newMap.MaID, updateResquest.GiangvienID, newMap.ChuyendeID);
                                }
                            }
                        }

                        await trans.CommitAsync();

                        return new ServiceDefault<TeacherUpdateResponse>
                        {
                            statusCode = StatusCodes.Status200OK,
                            Message = "Cập nhật thành công",
                            Data = new TeacherUpdateResponse { GiangvienID = updateResquest.GiangvienID }
                        };
                    }
                    catch (Exception ex)
                    {
                        await trans.RollbackAsync();
                        return new ServiceDefault<TeacherUpdateResponse>
                        {
                            statusCode = StatusCodes.Status500InternalServerError,
                            Message = "Cập nhật không thành công: " + ex.Message,
                            Data = null
                        };
                    }
                }
            }
        }

        public async Task<ServiceDefault<ScheduleTeacherAttendanceResponse>> TeacherCheckoutAsync(string? GiangvienID, string? LichhocID)
        {

            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        // Buoc 1: them vao bang diem danh
                        var attendance = Guid.NewGuid().ToString();
                        await _attendanceRepository.CreateAsync(conn, trans, attendance, LichhocID, GiangvienID, null, DateTime.UtcNow, DateTime.UtcNow);

                        // Buoc 2: update bang Lichhoc
                        await _scheduleRepository.UpdateAsync(conn, trans, LichhocID, null, null, GiangvienID, null, null, null, null, null, DateTime.UtcNow);
                        await trans.CommitAsync();

                        return new ServiceDefault<ScheduleTeacherAttendanceResponse>
                        {
                            statusCode = StatusCodes.Status500InternalServerError,
                            Message = "Check out thanh cong",
                            Data = null,
                        };
                    }
                    catch (Exception ex)
                    {
                        return new ServiceDefault<ScheduleTeacherAttendanceResponse>
                        {
                            statusCode = StatusCodes.Status500InternalServerError,
                            Message = "Check out khong thanh cong" + ex.Message,
                            Data = null,
                        };
                    }

                }
            }
            throw new NotImplementedException();
        }
    }
}
