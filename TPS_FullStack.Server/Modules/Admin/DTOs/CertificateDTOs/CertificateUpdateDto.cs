namespace TPS_FullStack.Server.Modules.Admin
{
    public class CertificateUpdateDto
    {
        public cErtificateInfo certificateInfo { get; set; }
        public ICollection<certificateCourse> certificateCourses { get; set; }
    }

    public class cErtificateInfo
    {
        public string MaID { get; set; }
        public string Ten { get; set; }
        public string Mota { get; set; }
        public string Donvicap { get; set; }
        public decimal Thoigiansudung { get; set; }
    }
    public class certificateCourse
    {
        public string KhoahocID { get; set; }
    }

}

