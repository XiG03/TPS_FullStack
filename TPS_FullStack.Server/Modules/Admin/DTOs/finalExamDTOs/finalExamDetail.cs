namespace TPS_FullStack.Server.Modules.Admin
{

    public class finalExamDetail
    {
        public string MaID {get; set;}
        public string KhoahocID {get; set;}
        public string HocvienID {get; set;}
        public string Tenhocvien {get; set;}
        public decimal Thoigianlambai {get; set;}
        public DateTime Batdauthi {get; set;}
        public DateTime Ketthucthi {get; set;}
        public decimal Diem {get; set;}
        public List<finalExamQuestionDetail> Danhsachcauhoi {get; set;}
    }
    public class finalExamQuestionDetail
    {
        public string MaID {get; set;}
        public string BaithuhoachID {get; set;}
        public string KhoahocID {get; set;}
        public string ChuyendeID {get; set;}
        public string CauhoiID {get; set;}
        public string Tencauhoi {get; set;}
        public string TraloiID {get; set;}
        public string Noidungtraloi {get; set;}
        public string DapanID {get; set;}
        public string Noidungdapan {get; set;}
        public bool Dung {get; set;}
    }
}