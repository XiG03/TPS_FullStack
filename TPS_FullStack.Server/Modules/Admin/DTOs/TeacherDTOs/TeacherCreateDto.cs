using System.Data.SqlTypes;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class TeacherCreateDto
    {
        public string MaID { get; set; }
        public string Hoten { get; set; }
        public string Gioitinh { get; set; }
        public string Email { get; set; }
        public string Diachi { get; set; }
        public string Dienthoai { get; set; }
        public DateTime Ngaysinh { get; set; }
        // public string Diachi {get; set;}
        public ICollection<TeachertopicsDto> topics { get; set; }

    }
    public class TeachertopicsDto
    {
        public string MaID { get; set; }
        public bool Picked { get; set; } // Bien de tao dung de kiem tra co them vao dto hay khong
    }
    // public class CoursesDto
    // {
    //     public string MaID { get; set; }
    //     public string Ten { get; set; }
    // }

}

