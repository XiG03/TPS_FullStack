namespace TPS_FullStack.Server.Modules.Admin
{
    public class CourseDetailDto
    {
        public courseInfo courseInfo { get; set; }
        public ICollection<courseTopics> courseTopics { get; set; }
        public ICollection<courseTeachers> courseTeachers { get; set; }
        public ICollection<courseStudents> courseStudents { get; set; }
    }

    public class courseInfo
    {
        public string MaID { get; set; }
        public string Ten { get; set; }
        public string Mota { get; set; }
        public decimal Diemdat { get; set; }
    }
    public class courseTopics
    {
        public string MaID { get; set; }
        public string Ten { get; set; }
    }
    public class courseTeachers
    {
        public string MaID { get; set; }
        public string Ten { get; set; }
    }
    public class courseStudents
    {
        public string MaID { get; set; }
        public string Ten { get; set; }
    }

}

