namespace TPS_FullStack.Server.Modules.Student
{
    public interface IStudentService
    {
        public Task<ServiceDefault<studentInfo>> GetStudentInfoAsync(string MaID);
        public Task<ServiceDefault<ICollection<studentSchedule>>> GetStudentScheduleAsync(string MaID);
        public Task<ServiceDefault<studentAttendance>> StudentCheckInAsync(studentAttendance attendance);
        public Task<ServiceDefault<ICollection<studentCourse>>> GetStudentCoursesAsync(string MaID);
        public Task<ServiceDefault<studentCourseInfo>> GetStudentCourseInfoAsync(string HocvienID, string KhoahocID);


        // Bai thu hoach
        public Task<ServiceDefault<BaithuhoachInfo>> GetFinalExamInfoAsync(string HocvienID, string KhoahocID);
        public Task<ServiceDefault<decimal>> SubmitFinalExamAsync(finalExam finalRecord);
    }

}

