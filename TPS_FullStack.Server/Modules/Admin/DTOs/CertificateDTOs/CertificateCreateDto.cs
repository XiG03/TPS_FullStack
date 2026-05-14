namespace TPS_FullStack.Server.Modules.Admin
{
    public class CertificateCreateDto
    {

    }
    public class CertificateInfo
    {
        public string MaID { get; set; }
        public string Ten { get; set; }
        public string Mota { get; set; }
        public string Donvicap { get; set; }
        public decimal Thoigiansudung { get; set; }
    }
    public class CertificateCourses
    {
        public string KhoahocID { get; set; }
    }

}

