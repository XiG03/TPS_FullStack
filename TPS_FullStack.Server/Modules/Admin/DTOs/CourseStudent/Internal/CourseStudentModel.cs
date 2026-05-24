using Microsoft.Identity.Client;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class CourseStudentModel
    {
        public string MaID { get; set; }
        public string KhoahocID { get; set; }
        public string HocvienID { get; set; }
        public decimal? Diem { get; set; }
        public decimal? Dieuchinh { get; set; }

    }

}

