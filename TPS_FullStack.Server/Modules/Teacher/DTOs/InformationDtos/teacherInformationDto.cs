using System.Data.SqlTypes;

namespace TPS_FullStack.Server.Modules.Teacher
{
    public class teacherInformationDto
    {

    }
    public class teacherInfo
    {
        public string MaID { get; set; }
        public string Hoten { get; set; }
        public string Gioitinh { get; set; }
        public string Email { get; set; }
        public string Diachi { get; set; }
        public string Dienthoai { get; set; }
    }
    public class teacherTopics
    {
        public string KhoahocID{get; set;}
        public string TenKhoahoc {get; set;}
    }
    public class teacherCourses
    {
        public string KhoahocID {get; set;}
        public string TenKhoahoc {get; set;}
    }
    public class teacherSchedules
    {
        public string LichhocID {get; set;}
        public string KhoahocID{get; set;}
        public string TenKhoahoc {get; set;}
    }
}

