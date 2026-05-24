namespace TPS_FullStack.Server.Modules.Admin
{
    public class TeacherCreateResquest
    {
        public string? GiangvienID {get; set; }
        public string? Hoten {get; set;}
        public DateTime? Ngaysinh {get; set;}
        public string? Gioitinh {get; set;}
        public string? Email {get; set;}
        public string? Diachi {get; set;}
        public string? Dienthoai {get; set;}
        public List<TeacherTopicCreateRequest>? Topics {get; set;}

    }
    public class TeacherTopicCreateRequest
    {
        public string? MaID {get; set;}
        public string? ChuyendeID {get; set;}
    }

}

