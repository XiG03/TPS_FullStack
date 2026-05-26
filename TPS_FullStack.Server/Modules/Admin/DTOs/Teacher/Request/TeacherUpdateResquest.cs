namespace TPS_FullStack.Server.Modules.Admin
{
    public class TeacherUpdateResquest
    {
        public string? GiangvienID { get; set; }
        public string? Hoten { get; set; }
        public DateTime? Ngaysinh { get; set; }
        public string? Gioitinh { get; set; }
        public string? Email { get; set; }
        public string? Diachi { get; set; }
        public string? Dienthoai { get; set; }
        public List<TeacherTopicUpdateRequest>? Topics { get; set; }

    }
    public class TeacherTopicUpdateRequest
    {
        public string? MaID { get; set; }
        public string? ChuyendeID { get; set; }
    }
}


