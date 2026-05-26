namespace TPS_FullStack.Server.Modules.Admin
{
    public class StudentDetailResponse
    {
        public string? HocvienID { get; set; }
        public string? Hoten { get; set; }
        public DateTime? Ngaysinh { get; set; } // Sẽ bị null nếu StudentModel bên Repo chưa map Ngaysinh
        public string? Gioitinh { get; set; }
        public string? Email { get; set; }
        public string? Diachi { get; set; }
        public string? Dienthoai { get; set; }
        public List<StudentCertificateDetailResponse> Certificates { get; set; } = new List<StudentCertificateDetailResponse>();
    }

    public class StudentCertificateDetailResponse
    {
        public string? MaID { get; set; }
        public string? ChungchiID { get; set; }
        public string? TenChungchi { get; set; }
        public string? Mota { get; set; }
        public string? Donvicap { get; set; }
        public DateTime? Ngaycap { get; set; }
        public DateTime? Ngayhethan { get; set; }
    }

}

