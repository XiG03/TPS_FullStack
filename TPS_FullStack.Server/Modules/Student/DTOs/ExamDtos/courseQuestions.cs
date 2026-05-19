namespace TPS_FullStack.Server.Modules.Student
{
    public class courseQuestions
    {
        public string ChuyendeID{get; set ;}
        public string CauhoiID{get; set;}
        public string Ten {get; set;}
        public decimal Diem{get; set;}
        public List<courseQuestionAnswer> Dapan {get; set;}
    }

    public class courseQuestionAnswer
    {
        public string CauhoiID{get; set;}
        public string DapanID {get; set;}
        public string Ten {get; set;}
        public bool Dung {get; set;}
    }

}

