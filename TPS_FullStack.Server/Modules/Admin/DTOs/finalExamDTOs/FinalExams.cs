namespace TPS_FullStack.Server.Modules.Admin
{
    public class FinalExams
    {
        
    }

    public class finalExams
    {
        public string MaID {get; set;}
        public string KhoahocID {get; set;}
        public string HocvienID {get; set;}
        public decimal Thoigianlambai {get; set;}
        public List<finalExamQuestions> Danhsachcauhoi {get; set;}
    }
    public class finalExamQuestions
    {
        public string MaID {get; set;}
        public string BaithuhoachID {get; set;}
        public string KhoahocID {get; set;}
        public string ChuyendeID {get; set;}
        public string CauhoiID {get; set;}
        public string Tencauhoi {get; set;}
        public string DapanID {get; set;}
        public string Noidungdapan {get; set;}
    }
}
