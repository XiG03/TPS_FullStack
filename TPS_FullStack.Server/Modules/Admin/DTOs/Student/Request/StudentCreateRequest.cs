namespace TPS_FullStack.Server.Modules.Admin
{
    public class StudentCreateRequest
    {
        public string? HocvienID { get; set; }
        public string? Hoten { get; set; }
        public DateTime? Ngaysinh { get; set; }
        public string? Gioitinh { get; set; }
        public string? Email { get; set; }
        public string? Diachi { get; set; }
        public string? Dienthoai { get; set; }
    }

}
