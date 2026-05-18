namespace TPS_FullStack.Server.Modules.Teacher
{
    public interface ITeacherRepository
    {
        public Task<teacherInformationDto> GetTeacherInformationAsync(string teacherId);
        public Task<ICollection<teacherSchedules>> GetTeacherSchedulesAsync(string teacherId);
        public Task<bool> TeacherAttendanceAsync(teacherAttendance attendance);
    }

}

