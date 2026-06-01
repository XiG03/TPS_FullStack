namespace TPS_FullStack.Server.Modules.Admin
{
    public class CourseCreateRequest
    {
        public string? KhoahocID { get; set; }
        public string? Ten { get; set; }
        public string? Mota { get; set; }
        public decimal? Diemdat { get; set; }
        public string? ChungchiID { get; set; }
        public string? Thu { get; set; }
        public decimal? Thoiluonghoc { get; set; }
        public DateTime Batdaudukien {get; set;}
        public decimal? Sobuoihoc { get; set; }
        public DateTime? Ngaybatdau { get; set; }
        public decimal? Thoiluongthi { get; set; }
        public decimal? Socauhoi { get; set; }
        public List<CourseTeacherCreateRequest> Teachers { get; set; }
        public List<CourseStudentCreateRequest> Students { get; set; }
        public List<CourseTopicCreateRequest> Topics { get; set; }
    }

    public class CourseTeacherCreateRequest
    {
        public string? MaID { get; set; }
        public string? KhoahocID { get; set; }
        public string? GiangvienID { get; set; }
    }
    public class CourseTopicCreateRequest
    {
        public string? MaID { get; set; }
        public string? KhoahocID { get; set; }
        public string? ChuyendeID { get; set; }
        public decimal? Socauhoi { get; set; }
    }
    public class CourseStudentCreateRequest
    {
        public string? MaID { get; set; }
        public string? KhoahocID { get; set; }
        public string? HocvienID { get; set; }
        public decimal? Diem { get; set; }
        public decimal? Dieuchinh { get; set; }
    }

}

