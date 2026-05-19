using System.Text.Json.Serialization;

namespace TPS_FullStack.Server.Modules.Student
{
    public class finalExam
    {
        public finalExamInfo examInfo {get; set;}
        public ICollection<examQues> examQues {get; set;}
        public ICollection<examQuesAns> examQuesAns {get; set;}
    }
    public class finalExamInfo
    {
        public string BaithiID{get; set ;}
        public string HocvienID{get; set;}
        public string KhoahocID{get; set;}
        public string Tenbaithi{get; set;}
        public decimal Thoigianlambai{get; set;}
        public decimal Diem{get; set;}
        public decimal Socauhoi{get; set;}
    }
    public class examQues
    {
        public string MaID  {get; set;}
        public string CauhoiID{get; set;}
        public string Ten {get; set;}
        public decimal Diem {get; set;}
    }
    public class examQuesAns
    {
        public string MaID {get; set;}
        public string CauhoiID{get; set;}
        [JsonIgnore]
        public string DapanID{get; set;}
        public string TraloiID {get; set;}
        [JsonIgnore]
        public bool Dung {get; set;}
    }
}

