using Microsoft.Identity.Client;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class TeacherDetailDto
    {
        public TeacherInfoDto teacherInfo { get; set; }
        public ICollection<TopicsDto> teacherTopics { get; set; }
        public ICollection<CoursesDto> teacherCourses { get; set; }
        public ICollection<StudentsDto> teacherStudents { get; set; }
    }
    public class TeacherInfoDto
    {
        public string MaID { get; set; }
        public string Hoten { get; set; }
        public string Gioitinh { get; set; }
        public string Email { get; set; }
        public string Diachi { get; set; }
        public string Dienthoai { get; set; }

    }
    public class TopicsDto
    {
        public string MaID { get; set; }
        public string Ten { get; set; }
    }
    public class CoursesDto
    {
        public string MaID { get; set; }
        public string Ten { get; set; }
    }

    public class StudentsDto
    {
        public string MaID { get; set; }
        public string Ten { get; set; }
    }

}

