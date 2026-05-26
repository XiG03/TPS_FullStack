using System;
using System.Collections.Generic;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class CourseDetailResponse
    {
        public string? KhoahocID { get; set; }
        public string? Ten { get; set; }
        public string? Mota { get; set; }
        public decimal? Diemdat { get; set; }
        public string? ChungchiID { get; set; }
        public string? Thu { get; set; }
        public decimal? Thoiluonghoc { get; set; }
        public DateTime? Batdaudukien { get; set; }
        public decimal? Sobuoihoc { get; set; }
        public DateTime? Ngaybatdau { get; set; }
        public decimal? Thoiluongthi { get; set; }
        public decimal? Socauhoi { get; set; }

        public List<CourseTeacherResponse> Teachers { get; set; } = new List<CourseTeacherResponse>();
        public List<CourseStudentResponse> Students { get; set; } = new List<CourseStudentResponse>();
        public List<CourseTopicResponse> Topics { get; set; } = new List<CourseTopicResponse>();
    }

    public class CourseTeacherResponse
    {
        public string? MaID { get; set; }
        public string? GiangvienID { get; set; }
    }

    public class CourseStudentResponse
    {
        public string? MaID { get; set; }
        public string? HocvienID { get; set; }
        public decimal? Diem { get; set; }
        public decimal? Dieuchinh { get; set; }
    }

    public class CourseTopicResponse
    {
        public string? MaID { get; set; }
        public string? ChuyendeID { get; set; }
        public decimal? Socauhoi { get; set; }
    }
}
