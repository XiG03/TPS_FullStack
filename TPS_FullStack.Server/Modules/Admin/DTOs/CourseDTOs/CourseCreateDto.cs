namespace TPS_FullStack.Server.Modules.Admin
{
    public class CourseCreateDto
    {
        public CourseDto courseDto { get; set; }
        public ICollection<CourseTopicDto> courseTopicDtos { get; set; }
        public ICollection<CourseTeacherDto> courseTeacherDtos { get; set; }
        public ICollection<CourseStudentDto> courseStudentDtos { get; set; }
    }
    public class CourseDto
    {
        public string MaID { get; set; }
        public string Ten { get; set; }
        public string Mota { get; set; }
        public decimal Diemdat { get; set; }
        public string ChungchiID { get; set; }
        public string LichhocID { get; set; }
    }
    public class CourseTopicDto
    {
        public string MaID { get; set; }
        public string Ten { get; set; }
    }
    public class CourseTeacherDto
    {
        public string MaID { get; set; }
        public string Ten { get; set; }
    }
    public class CourseStudentDto
    {
        public string MaID { get; set; }
        public string Ten { get; set; }
    }
}

