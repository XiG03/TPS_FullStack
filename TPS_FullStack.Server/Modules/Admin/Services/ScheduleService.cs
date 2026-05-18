using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }
        public async Task<ServiceDefault<bool>> ScheduleDeleteAsync(string MaID)
        {
            var result = await _scheduleRepository.ScheduleDeleteAsync(MaID);
            if (result)
            {
                return new ServiceDefault<bool>
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Complete",
                    Data = true
                };
            }
            if (!result)
            {
                return new ServiceDefault<bool>
                {
                    statusCode = StatusCodes.Status500InternalServerError,
                    Message = "Server can't delete",
                    Data = false
                };
            }
            throw new NotImplementedException();
        }

        public async Task<ServiceDefault<List<ScheduleGetAllDto>>> ScheduleGetAllAsync()
        {
            var result = await _scheduleRepository.ScheduleGetAllAsync();
            if (result != null)
            {
                return new ServiceDefault<List<ScheduleGetAllDto>>
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Find Schedule list",
                    Data = result
                };
            }
            throw new NotImplementedException();
        }

        public async Task<ServiceDefault<ScheduleCreateDto>> ScheduleInsertAsync(ScheduleCreateDto createDto)
        {
            var result = await _scheduleRepository.ScheduleInsertAsync(createDto);
            if (result)
            {
                return new ServiceDefault<ScheduleCreateDto>
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Complete",
                    Data = createDto
                };
            }
            if (!result)
            {
                return new ServiceDefault<ScheduleCreateDto>
                {
                    statusCode = StatusCodes.Status500InternalServerError,
                    Message = "Server can't",
                    Data = null
                };
            }
            throw new NotImplementedException();
        }

        public async Task<ServiceDefault<ScheduleStudentAttendanceDto>> ScheduleStudentAttendanceAsync(ScheduleStudentAttendanceDto attendanceDto)
        {
            var result = await _scheduleRepository.ScheduleStudentAttendanceAsync(attendanceDto);
            if (result)
            {
                return new ServiceDefault<ScheduleStudentAttendanceDto>
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Complete",
                    Data = attendanceDto
                };
            }
            if (!result)
            {
                return new ServiceDefault<ScheduleStudentAttendanceDto>
                {
                    statusCode = StatusCodes.Status500InternalServerError,
                    Message = "Fail",
                    Data = null
                };
            }
            throw new NotImplementedException();
        }

        public async Task<ServiceDefault<ScheduleTeacherAttendanceDto>> ScheduleTeacherAttendanceAsync(ScheduleTeacherAttendanceDto attendanceDto)
        {
            var result = await _scheduleRepository.ScheduleTeacherAttandanceAsync(attendanceDto);
            if (result)
            {
                return new ServiceDefault<ScheduleTeacherAttendanceDto>
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Complete",
                    Data = attendanceDto
                };
            }
            if (!result)
            {
                return new ServiceDefault<ScheduleTeacherAttendanceDto>
                {
                    statusCode = StatusCodes.Status500InternalServerError,
                    Message = "Fail",
                    Data = null
                };
            }
            throw new NotImplementedException();
        }

        public async Task<ServiceDefault<ScheduleUpdateDto>> ScheduleUpdateAsync(ScheduleUpdateDto updateDto)
        {
            var result = await _scheduleRepository.ScheduleUpdateAsync(updateDto);
            if (result)
            {
                return new ServiceDefault<ScheduleUpdateDto>
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Complete",
                    Data = updateDto
                };
            }
            if (!result)
            {
                return new ServiceDefault<ScheduleUpdateDto>
                {
                    statusCode = StatusCodes.Status500InternalServerError,
                    Message = "Fail",
                    Data = null
                };
            }
            throw new NotImplementedException();
        }
    }

}

