using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Identity.Client;

namespace TPS_FullStack.Server.Modules.Student
{
    public class studentCourseInfo
    {
        public courseInfo courseInfo { get; set; }
        public ICollection<topicInfo> topicInfos { get; set; }
        public ICollection<topicDocInfo> topicDocInfos { get; set; }
        public ICollection<courseExamInfo> courseExamInfo { get; set; }
    }

    public class courseInfo
    {
        public string MaID { get; set; }
        public string TenKhoaHoc { get; set; }
        public string Mota { get; set; }
        public string Diemdat { get; set; }
    }
    public class topicInfo
    {
        public string ChuyendeID { get; set; }
        public string TenChuyende { get; set; }
        public string Mota { get; set; }
        public decimal Socauhoi{get; set;}
    }
    public class topicDocInfo
    {
        public string TailieuID { get; set; }
        public string Tentailieu { get; set; }
    }
    public class courseExamInfo
    {
        public decimal Thoiluongthi { get; set; }
        public decimal Socauhoi { get; set; }

    }

}

