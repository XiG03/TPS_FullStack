namespace TPS_FullStack.Server.Modules.Student
{
    public class BaithuhoachInfo
    {
        public string MaID {get; set;}
        public string KhoahocID {get; set;}
        public string HocvienID {get; set;}
        public DateTime Batdauthi {get; set;}
        public DateTime Ketthucthi {get; set;}
        public decimal Diem {get; set;}
        public decimal Thoigianlambai {get; set;}
        public List<BaithuhoachCauhoiInfo> Danhsachcauhoi {get; set;}
    }
    public class BaithuhoachCauhoiInfo
    {
        public string MaID {get; set;}
        public string BaithuhoachID {get; set;}
        public string KhoahocID {get; set;}
        public string ChuyendeID{get; set ;}
        public string CauhoiID{get; set;}
        public string Tencauhoi {get; set;}
        public List<BaithuhoachCauhoiDapanInfo> Dapan {get; set;}
    }

    public class BaithuhoachCauhoiDapanInfo
    {
        public string MaID {get; set;}
        public string CauhoiID{get; set;}
        public string DapanID {get; set;}
        public string Ten {get; set;}
        public bool Dung {get; set;}
    }

}

