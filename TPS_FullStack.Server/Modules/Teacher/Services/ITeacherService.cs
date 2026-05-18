namespace TPS_FullStack.Server.Modules.Teacher
{
    public interface ITeacherService
    {
        public Task<ServiceDefault<teacherInformationDto>> GetTeacherInformationAsync(string teacherId);
        public Task<ServiceDefault<ICollection<teacherSchedules>>> GetTeacherSchedulesAsync(string teacherId);
        public Task<ServiceDefault<teacherAttendance>> TeacherAttendanceAsync(teacherAttendance attendance);
    }

}

