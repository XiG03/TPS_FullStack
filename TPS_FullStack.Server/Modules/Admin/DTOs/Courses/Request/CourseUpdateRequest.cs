namespace TPS_FullStack.Server.Modules.Admin
{
    public class CourseUpdateRequest
    {
        public string? KhoahocID { get; set; }
        public string? Ten { get; set; }
        public string? Mota { get; set; }
        public decimal? Diemdat { get; set; }
        public string? ChungchiID { get; set; }
        public string? Thu { get; set; }
        public decimal? Thoiluonghoc { get; set; }
        public DateTime Batdaudukien { get; set; }
        public decimal? Sobuoihoc { get; set; }
        public DateTime? Ngaybatdau { get; set; }
        public decimal? Thoiluongthi { get; set; }
        public decimal? Socauhoi { get; set; }
        public List<CourseTeacherUpdateRequest> Teachers { get; set; }
        public List<CourseStudentUpdateRequest> Students { get; set; }
        public List<CourseTopicUpdateRequest> Topics { get; set; }
    }

    public class CourseTeacherUpdateRequest
    {
        public string? MaID { get; set; }
        public string? KhoahocID { get; set; }
        public string? GiangvienID { get; set; }
    }
    public class CourseTopicUpdateRequest
    {
        public string? MaID { get; set; }
        public string? KhoahocID { get; set; }
        public string? ChuyendeID { get; set; }
        public decimal? Socauhoi { get; set; }
    }
    public class CourseStudentUpdateRequest
    {
        public string? MaID { get; set; }
        public string KhoahocID { get; set; }
        public string HocvienID { get; set; }
        public decimal? Diem { get; set; }
        public decimal? Dieuchinh { get; set; }
    }

}

