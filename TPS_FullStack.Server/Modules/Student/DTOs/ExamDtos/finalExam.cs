using System.Text.Json.Serialization;

namespace TPS_FullStack.Server.Modules.Student
{
    public class finalExam
    {
        public string MaID {get; set;}
        public string HocvienID {get; set;}
        public string KhoahocID {get; set;}
        public DateTime Batdauthi {get; set;}
        public DateTime Ketthucthi {get; set;}
        public decimal Diem {get; set;}
        public decimal Thoigianlambai {get; set;}
        public List<finalExamQuesUpdate> Danhsachcauhoi {get; set;}
    }
    public class finalExamQuesUpdate
    {
        public string BaithuhoachID {get; set;}
        public string CauhoiID {get; set;}
        public string DapanID {get; set;}
        public string Noidungtraloi {get; set;}
    }
   
}

