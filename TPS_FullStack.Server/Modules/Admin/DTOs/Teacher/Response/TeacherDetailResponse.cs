namespace TPS_FullStack.Server.Modules.Admin
{
    public class TeacherDetailResponse
    {
        public string? MaID { get; set; }
        public string? Hoten { get; set; }
        public string? Gioitinh { get; set; }
        public string? Email { get; set; }
        public string? Diachi { get; set; }
        public string? Dienthoai { get; set; }
        public List<TeacherTopicResponse> Topics { get; set; } = new List<TeacherTopicResponse>();
    }

    public class TeacherTopicResponse
    {
        public string? MaID { get; set; }
        public string? ChuyendeID { get; set; }
        public string? TenChuyende { get; set; }
    }
}