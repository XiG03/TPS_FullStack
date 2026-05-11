namespace TPS_FullStack.Server.Modules.Admin
{
    public class CourseUpdateDto
    {
        public courseUpdate courseUpdate { get; set; }
        public ICollection<courseTopics> courseTopics { get; set; }
        public ICollection<courseTeachers> courseTeachers { get; set; }
        public ICollection<courseStudents> courseStudents { get; set; }
    }
    public class courseUpdate
    {
        public string MaID { get; set; }
        public string Ten { get; set; }
        public string Mota { get; set; }
        public decimal Diemdat { get; set; }
        public string ChungchiID { get; set; }
        public string LichhocID { get; set; }
    }
    public class courseTopicUpdate
    {
        public string MaID { get; set; }
        public string Ten { get; set; }
    }
    public class courseTeacherUpdate
    {
        public string MaID { get; set; }
        public string Ten { get; set; }
    }
    public class courseStudentUpdate
    {
        public string MaID { get; set; }
        public string Ten { get; set; }
    }
}

