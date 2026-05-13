namespace TPS_FullStack.Server.Modules.Admin
{
    public class StudentDetailDto
    {
        public StudentInfo studentInfo {get; set;}
        public List<StudentCertificate> studentCertificate{get; set;}
    }
    public class StudentInfo
    {
        public string MaID { get; set; }
        public string Hoten { get; set; }
        public string Email { get; set; }
        public string Dienthoai { get; set; }
    }
    public class StudentCertificate
    {
        public string MaID{get; set;}
        public string Tenchungchi {get; set;}
        public DateTime Ngaycap {get; set;}
        public DateTime Ngayhethan {get; set;}
    }

}

