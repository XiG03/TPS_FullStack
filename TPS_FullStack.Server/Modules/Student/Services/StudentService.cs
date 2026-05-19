namespace TPS_FullStack.Server.Modules.Student
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public async Task<ServiceDefault<studentCourseInfo>> GetStudentCourseInfoAsync(string HocvienID, string KhoahocID)
        {
            var result = await _studentRepository.GetStudentCourseInfoAsync(HocvienID, KhoahocID);
            if (result != null)
            {
                return new ServiceDefault<studentCourseInfo>
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Lấy thông tin khóa học thành công",
                    Data = result
                };
            }
            return new ServiceDefault<studentCourseInfo>
            {
                statusCode = StatusCodes.Status404NotFound,
                Message = "Không tìm thấy thông tin khóa học",
                Data = null
            };
            throw new NotImplementedException();
        }

        public async Task<ServiceDefault<ICollection<studentCourse>>> GetStudentCoursesAsync(string MaID)
        {
            var result = await _studentRepository.GetStudentCoursesAsync(MaID);
            return new ServiceDefault<ICollection<studentCourse>>
            {
                statusCode = StatusCodes.Status200OK,
                Message = "Lấy danh sách khóa học thành công",
                Data = result
            };
        }

        public async Task<ServiceDefault<studentInfo>> GetStudentInfoAsync(string MaID)
        {
            var result = await _studentRepository.GetStudentInfoAsync(MaID);
            if (result == null)
            {
                return new ServiceDefault<studentInfo>
                {
                    statusCode = StatusCodes.Status404NotFound,
                    Message = "Không tìm thấy thông tin sinh viên",
                    Data = null
                };
            }
            return new ServiceDefault<studentInfo>
            {
                statusCode = StatusCodes.Status200OK,
                Message = "Lấy thông tin sinh viên thành công",
                Data = result
            };
        }

        public async Task<ServiceDefault<ICollection<studentSchedule>>> GetStudentScheduleAsync(string MaID)
        {
            var result = await _studentRepository.GetStudentScheduleAsync(MaID);
            if (result == null || result.Count == 0)
            {
                return new ServiceDefault<ICollection<studentSchedule>>
                {
                    statusCode = StatusCodes.Status404NotFound,
                    Message = "Không tìm thấy lịch học",
                    Data = null
                };
            }
            return new ServiceDefault<ICollection<studentSchedule>>
            {
                statusCode = StatusCodes.Status200OK,
                Message = "Lấy lịch học thành công",
                Data = result
            };
        }

        public async Task<ServiceDefault<studentAttendance>> StudentCheckInAsync(studentAttendance attendance)
        {
            var result = await _studentRepository.StudentCheckInAsync(attendance);
            if (result)
            {
                return new ServiceDefault<studentAttendance>
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Điểm danh thành công",
                    Data = attendance
                };
            }
            return new ServiceDefault<studentAttendance>
            {
                statusCode = StatusCodes.Status400BadRequest,
                Message = "Điểm danh thất bại",
                Data = null
            };
        }
    }

}

