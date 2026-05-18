namespace TPS_FullStack.Server.Modules.Admin
{
    public class ScheduleAttendanceDto
    {

    }
    public class ScheduleTeacherAttendanceDto
    {
        public string MaID { get; set; }
        public string GiangvienID { get; set; }
        public string LichhocID { get; set; }
    }
    public class ScheduleStudentAttendanceDto
    {
        public string MaID { get; set; }
        public string HocvienID { get; set; }
        public string LichhocID { get; set; }
    }
}


