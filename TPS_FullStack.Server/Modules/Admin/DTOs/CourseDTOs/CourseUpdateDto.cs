namespace TPS_FullStack.Server.Modules.Admin
{
    public class CourseUpdateDto
    {
        public courseUpdate courseUpdate { get; set; }
        public ICollection<courseTopics> courseTopics { get; set; }
        public ICollection<courseTeachers> courseTeachers { get; set; }
        public ICollection<courseStudentUpdate> courseStudents { get; set; }
        public ICollection<courseScheduleUpdate> courseScheduleUpdates{get; set;}
    }
    public class courseUpdate
    {
        // Information about the course
        public string MaID { get; set; }
        public string Ten { get; set; }
        public string Mota { get; set; }
        public decimal Diemdat { get; set; }
        public string ChungchiID { get; set; }

        // Information about the schedule config
        public decimal Sobuoihoc { get; set; }
        public string Thu { get; set; }
        public decimal Thoiluonghoc { get; set; }
        public DateTime Ngaybatdau { get; set; }

        //Information about the final exam config
        public decimal Thoigianthi { get; set; }
        public decimal Thoiluongthi { get; set; }
        public decimal Socauhoi { get; set; }
    }
    public class courseTopicUpdate
    {
        public string MaID { get; set; }
        public string ChuyendeID { get; set; }
        public decimal SoCauhoi { get; set; }
    }
    public class courseTeacherUpdate
    {
        public string MaID { get; set; }
        public string GiangvienID { get; set; }
    }
    public class courseStudentUpdate
    {
        public string MaID { get; set; }
        public string HocvienID { get; set; }
        public decimal Diem { get; set; }
        public decimal Hieuchinh { get; set; }
    }
    public class courseScheduleUpdate
    {
        public string MaID { get; set; }
        public DateTime Ngaydukien { get; set; }
        public DateTime Ngaythucte { get; set; }
        public DateTime Tugio { get; set; }
        public DateTime Dengio { get; set; }
    }
}

