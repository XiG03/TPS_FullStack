using Microsoft.Identity.Client;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class TeacherUpdateDto
    {
        public string MaID { get; set; }
        public ICollection<TeacherTopic> teacherTopics { get; set; }
    }
    public class TeacherTopic
    {
        public string MaID { get; set; }
        public string GiangvienID { get; set; }
        public string ChuyendeID { get; set; }
    }

}

