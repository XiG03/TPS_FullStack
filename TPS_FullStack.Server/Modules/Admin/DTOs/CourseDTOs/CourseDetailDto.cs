using System.ComponentModel;
using Microsoft.Identity.Client;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class CourseDetailDto
    {
        public courseInfo courseInfo { get; set; }
        public ICollection<courseTopics> courseTopics { get; set; }
        public ICollection<courseTeachers> courseTeachers { get; set; }
        public ICollection<courseStudents> courseStudents { get; set; }
        public ICollection<courseSchedules> courseSchedules { get; set; }
    }

    public class courseInfo
    {
        // Information about the course
        public string MaID { get; set; }
        public string Ten { get; set; }
        public string Mota { get; set; }
        public decimal Diemdat { get; set; }

        // Information about the schedule config
        public decimal Sobuoihoc { get; set; }
        public string Thu { get; set; }
        public decimal Thoiluonghoc { get; set; }
        public DateTime Ngaybatdau { get; set; }

        // Information about the final exam config
        public decimal Thoigianthi { get; set; }
        public decimal Thoiluongthi { get; set; }
        public decimal Socauhoi { get; set; }

    }
    public class courseTopics
    {
        public string MaID { get; set; }
        public string ChuyendeID { get; set; }
        public string Ten { get; set; }
        public decimal SoCauhoi { get; set; }
    }
    public class courseTeachers
    {
        public string MaID { get; set; }
        public string GiangvienID { get; set; }
        public string Hoten { get; set; }
    }
    public class courseStudents
    {
        public string MaID { get; set; }
        public string HocvienID { get; set; }
        public string Hoten { get; set; }
    }
    public class courseSchedules
    {
        public string MaID { get; set; }
        public DateTime Ngaydukien { get; set; }
        public DateTime Ngaythucte { get; set; }
        public DateTime Tugio { get; set; }
        public DateTime Dengio { get; set; }
    }

    public class courseTeacherAttendance
    {
        public string MaID { get; set; }
        public string LichhocID { get; set; }
        public string GiangvienID { get; set; }
        public string KhoahocID { get; set; }
    }
    public class courseStudentAttendance
    {
        public string MaID { get; set; }
        public string LichhocID { get; set; }
        public string HocvienID { get; set; }
        public string KhoahocID { get; set; }
    }

}

