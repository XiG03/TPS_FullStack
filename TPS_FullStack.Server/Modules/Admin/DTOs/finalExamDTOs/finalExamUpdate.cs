namespace TPS_FullStack.Server.Modules.Admin
{
    public class finalExamUpdate
    {
        public string MaID { get; set; }
        public string KhoahocID { get; set; }
        public string HocvienID { get; set; }
        public decimal Thoigianlambai { get; set; }
    }
    public class finalExamQuestionUpdate
    {
        public string MaID { get; set; }
        public string BaithuhoachID { get; set; }
        public string KhoahocID { get; set; }
        public string ChuyendeID { get; set; }
        public string CauhoiID { get; set; }
        public string Tencauhoi { get; set; }
        public string TraloiID { get; set; }
        public string Noidungtraloi { get; set; }
        public string DapanID { get; set; }
        public string Noidungdapan { get; set; }
        public bool Dung { get; set; }
    }
}