namespace TPS_FullStack.Server.Modules.Admin
{
    public class CertificateDetailDto
    {
        public certificateInfo certificateInfo { get; set; }
        public ICollection<certificateCourses> certificateCourses { get; set; }
        public ICollection<certificateStudents> certificateStudents { get; set; }
    }

    public class certificateInfo
    {
        public string MaID { get; set; }
        public string Ten { get; set; }
        public string Mota { get; set; }
        public decimal Thoigiansudung { get; set; }
        public string Donvicap { get; set; }
    }
    public class certificateCourses
    {
        public string KhoahocID { get; set; }
        public string Ten { get; set; }
    }
    public class certificateStudents
    {
        public string MaID { get; set; }
        public string ChungchiID { get; set; }
        public string HocvienID { get; set; }
        public string HocvienTen { get; set; }
        public DateTime Ngaycap {get; set;}
        public DateTime Ngayhethan {get; set;}
    }

}

