namespace TPS_FullStack.Server.Modules.Student
{
    public interface IStudentRepository
    {
        public Task<studentInfo> GetStudentInfoAsync(string MaID);
        public Task<ICollection<studentSchedule>> GetStudentScheduleAsync(string MaID);
        public Task<bool> StudentCheckInAsync(studentAttendance attendance);
        public Task<ICollection<studentCourse>> GetStudentCoursesAsync(string MaID);
        public Task<studentCourseInfo> GetStudentCourseInfoAsync(string HocvienID, string KhoahocID);
    }

}

