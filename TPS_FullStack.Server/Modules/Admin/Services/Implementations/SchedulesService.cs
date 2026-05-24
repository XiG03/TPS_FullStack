using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class SchedulesService : ISchedulesService
    {
        private readonly IConfiguration _configuration;
        private readonly ISchedulesRepository _schedulesRepository;
        private readonly ILogger<SchedulesService> _logger;

        public SchedulesService(IConfiguration configuration, ISchedulesRepository schedulesRepository, ILogger<SchedulesService> logger)
        {
            _configuration = configuration;
            _schedulesRepository = schedulesRepository;
            _logger = logger;
        }

        public async Task<ServiceDefault<ScheduleCreateResponse>> ScheduleCreateAsync(ScheduleCreateRequest createRequest)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        string lichhocId = string.IsNullOrEmpty(createRequest.LichhocID) ? Guid.NewGuid().ToString() : createRequest.LichhocID;

                        await _schedulesRepository.CreateAsync(
                            conn, trans, lichhocId, createRequest.KhoahocID, createRequest.ChuyendeID, createRequest.GiangvienID,
                            createRequest.Ngaydukien, createRequest.Batdaudukien, createRequest.Ketthucdukien,
                            createRequest.Ngaythucte, createRequest.Batdauthucte, createRequest.Ketthucthucte
                        );

                        await trans.CommitAsync();

                        return new ServiceDefault<ScheduleCreateResponse>
                        {
                            statusCode = StatusCodes.Status200OK,
                            Message = "Tạo lịch học thành công",
                            Data = new ScheduleCreateResponse { LichhocID = lichhocId }
                        };
                    }
                    catch (Exception ex)
                    {
                        await trans.RollbackAsync();
                        _logger.LogError(ex.Message);
                        return new ServiceDefault<ScheduleCreateResponse>
                        {
                            statusCode = StatusCodes.Status500InternalServerError,
                            Message = "Tạo lịch học không thành công: " + ex.Message,
                            Data = null
                        };
                    }
                }
            }
        }

        public async Task<ServiceDefault<bool>> ScheduleDeleteAsync(string? MaID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        await _schedulesRepository.DeleteAsync(conn, trans, MaID);
                        await trans.CommitAsync();

                        return new ServiceDefault<bool>
                        {
                            statusCode = StatusCodes.Status200OK,
                            Message = "Xóa lịch học thành công",
                            Data = true
                        };
                    }
                    catch (Exception ex)
                    {
                        await trans.RollbackAsync();
                        _logger.LogError(ex.Message);
                        return new ServiceDefault<bool>
                        {
                            statusCode = StatusCodes.Status500InternalServerError,
                            Message = "Xóa lịch học không thành công: " + ex.Message,
                            Data = false
                        };
                    }
                }
            }
        }

        public async Task<ServiceDefault<List<ScheduleResponse>>> ScheduleGetAllAsync()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var list = await _schedulesRepository.GetAllAsync(conn);
                var result = new List<ScheduleResponse>();

                foreach (var item in list)
                {
                    var res = new ScheduleResponse();
                    res.LichhocID = item.LichhocID;
                    res.KhoahocID = item.KhoahocID;
                    res.ChuyendeID = item.ChuyendeID;
                    res.GiangvienID = item.GiangvienID;
                    res.Ngaydukien = item.Ngaydukien;
                    res.Batdaudukien = item.Batdaudukien;
                    res.Ketthucdukien = item.Ketthucdukien;
                    res.Ngaythucte = item.Ngaythucte;
                    res.Batdauthucte = item.Batdauthucte;
                    res.Ketthucthucte = item.Ketthucthucte;

                    result.Add(res);
                }

                return new ServiceDefault<List<ScheduleResponse>>
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Lấy danh sách thành công",
                    Data = result
                };
            }
        }

        public async Task<ServiceDefault<ScheduleDetailResponse>> ScheduleGetByIDAsync(string? MaID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var item = await _schedulesRepository.GetByIDAsync(conn, MaID);

                if (item == null)
                {
                    return new ServiceDefault<ScheduleDetailResponse>
                    {
                        statusCode = StatusCodes.Status404NotFound,
                        Message = "Không tìm thấy lịch học",
                        Data = null
                    };
                }

                var detail = new ScheduleDetailResponse();
                detail.LichhocID = item.LichhocID;
                detail.KhoahocID = item.KhoahocID;
                detail.ChuyendeID = item.ChuyendeID;
                detail.GiangvienID = item.GiangvienID;
                detail.Ngaydukien = item.Ngaydukien;
                detail.Batdaudukien = item.Batdaudukien;
                detail.Ketthucdukien = item.Ketthucdukien;
                detail.Ngaythucte = item.Ngaythucte;
                detail.Batdauthucte = item.Batdauthucte;
                detail.Ketthucthucte = item.Ketthucthucte;

                return new ServiceDefault<ScheduleDetailResponse>
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Lấy thông tin thành công",
                    Data = detail
                };
            }
        }

        public async Task<ServiceDefault<ScheduleUpdateResponse>> ScheduleUpdateAsync(ScheduleUpdateRequest updateRequest)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                var existSchedule = await _schedulesRepository.GetByIDAsync(conn, updateRequest.LichhocID);
                if (existSchedule == null)
                {
                    return new ServiceDefault<ScheduleUpdateResponse>
                    {
                        statusCode = StatusCodes.Status404NotFound,
                        Message = "Không tìm thấy lịch học để cập nhật",
                        Data = null
                    };
                }

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        await _schedulesRepository.UpdateAsync(
                            conn, trans, updateRequest.LichhocID, updateRequest.KhoahocID, updateRequest.ChuyendeID, updateRequest.GiangvienID,
                            updateRequest.Ngaydukien, updateRequest.Batdaudukien, updateRequest.Kethucdukien,
                            updateRequest.Ngaythucte, updateRequest.Batdauthucte, updateRequest.Kethucthucte
                        );

                        await trans.CommitAsync();

                        return new ServiceDefault<ScheduleUpdateResponse>
                        {
                            statusCode = StatusCodes.Status200OK,
                            Message = "Cập nhật lịch học thành công",
                            Data = new ScheduleUpdateResponse { LichhocID = updateRequest.LichhocID }
                        };
                    }
                    catch (Exception ex)
                    {
                        await trans.RollbackAsync();
                        _logger.LogError(ex.Message);
                        return new ServiceDefault<ScheduleUpdateResponse>
                        {
                            statusCode = StatusCodes.Status500InternalServerError,
                            Message = "Cập nhật lịch học không thành công: " + ex.Message,
                            Data = null
                        };
                    }
                }
            }
        }
    }
}