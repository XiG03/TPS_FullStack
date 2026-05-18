namespace TPS_FullStack.Server.Modules.Teacher
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;
        public TeacherService(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }
        public async Task<ServiceDefault<teacherInformationDto>> GetTeacherInformationAsync(string teacherId)
        {
            var result = await _teacherRepository.GetTeacherInformationAsync(teacherId);
            if(result == null)
            {
                return new ServiceDefault<teacherInformationDto>
                {
                    statusCode = StatusCodes.Status404NotFound,
                    Message = "Teacher not found",
                    Data = null
                };
            }
            return new ServiceDefault<teacherInformationDto>
            {
                statusCode = StatusCodes.Status200OK,
                Message = "Teacher found",
                Data = result
            };
        }

        public async Task<ServiceDefault<ICollection<teacherSchedules>>> GetTeacherSchedulesAsync(string teacherId)
        {
            var result = await _teacherRepository.GetTeacherSchedulesAsync(teacherId);
            if(result == null)
            {
                return new ServiceDefault<ICollection<teacherSchedules>>
                {
                    statusCode = StatusCodes.Status404NotFound,
                    Message = "Teacher schedules not found",
                    Data = null
                };
            }
            return new ServiceDefault<ICollection<teacherSchedules>>
            {
                statusCode = StatusCodes.Status200OK,
                Message = "Teacher schedules found",
                Data = result
            };
        }

        public async Task<ServiceDefault<teacherAttendance>> TeacherAttendanceAsync(teacherAttendance attendance)
        {
            var result = await _teacherRepository.TeacherAttendanceAsync(attendance);
            if(result == null)
            {
                return new ServiceDefault<teacherAttendance>
                {
                    statusCode = StatusCodes.Status404NotFound,
                    Message = "Teacher attendance not found",
                    Data = null
                };
            }
            return new ServiceDefault<teacherAttendance>
            {
                statusCode = StatusCodes.Status200OK,
                Message = "Teacher attendance found",
                Data = attendance
            };
        }
    }
}


