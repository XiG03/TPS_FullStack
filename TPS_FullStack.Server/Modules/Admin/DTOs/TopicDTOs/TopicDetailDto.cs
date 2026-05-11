using System.Diagnostics.Eventing.Reader;
using System.Text.Json.Serialization;
using Microsoft.Identity.Client;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class TopicDetailDto
    {

        public string MaID { get; set; }
        public string Ten { get; set; }
        public string Mota { get; set; }
        public ICollection<DocumentDto> Documents { get; set; }
        public ICollection<QuestionDto> Questions { get; set; }
        public ICollection<TeacherDto> Teachers { get; set; }


    }

    public class DocumentDto
    {

        public string MaID { get; set; }
        public string Tieude { get; set; }
        public string Loaitailieu { get; set; }
        public decimal Kichthuoc { get; set; }
    }
    public class QuestionDto
    {

        public string MaID { get; set; }
        public string Ten { get; set; }
        public decimal Diem { get; set; }
        public ICollection<AnswerDto> Answers { get; set; }
    }
    public class AnswerDto
    {

        public string MaID { get; set; }
        public string CauhoiID { get; set; }
        public string Ten { get; set; }
        public bool Dung { get; set; }
    }
    public class TeacherDto
    {

        public string MaID { get; set; }
        public string Hoten { get; set; }
    }
}


