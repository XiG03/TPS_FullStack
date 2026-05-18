namespace TPS_FullStack.Server.Modules.Teacher
{
    public class teacherSchedule
    {
        public string LichhocID {get; set;}
        public string KhoahocID { get; set; }
        public string TenKhoahoc { get; set; }
        public string ChuyendeID {get; set;}
        public string TenChuyende { get; set; }
        public DateTime Ngaydukien { get; set; }
        public DateTime Batdaudukien { get; set; }
        public DateTime Ketthucdukien { get; set; }
        public DateTime Ngaythucte { get; set; }
        public DateTime Batdautheothucte { get; set; }
        public DateTime Ketthuctheothucte { get; set; }
    }
}

