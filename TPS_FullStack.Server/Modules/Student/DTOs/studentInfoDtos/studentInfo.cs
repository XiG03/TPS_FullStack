using Microsoft.Identity.Client;

namespace TPS_FullStack.Server.Modules.Student
{
    public class studentInfo
    {
        public studentInfoDto studentInfoDto { get; set; }
        public List<studentCertificateDto> studentCertificateDtos { get; set; }
        public List<studentCourseDto> studentCourseDtos { get; set; }
    }
    public class studentInfoDto
    {
        public string MaID { get; set; }
        public string studentName { get; set; }
        public DateTime Ngaysinh { get; set; }
        public string Gioitinh { get; set; }
        public string Diachi { get; set; }
        public string Dienthoai { get; set; }
    }
    public class studentCertificateDto
    {
        public string ChungchiID { get; set; }
        public string Tenchungchi { get; set; }
        public string Mota { get; set; }
        public string Donvicap { get; set; }
        public DateTime Ngaycap { get; set; }
        public DateTime Ngayhethan { get; set; }
    }
    public class studentCourseDto
    {
        public string MaID { get; set; }
        public string TenKhoaHoc { get; set; }
    }


}

