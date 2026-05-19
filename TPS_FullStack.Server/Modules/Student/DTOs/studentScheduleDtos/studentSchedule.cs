namespace TPS_FullStack.Server.Modules.Student
{
    public class studentSchedule
    {
        public string LichhocID { get; set; }
        public string KhoahocID { get; set; }
        public string TenKhoahoc { get; set; }
        public string ChuyendeID { get; set; }
        public string TenChuyende { get; set; }
        public DateTime Ngaydukien { get; set; }
        public DateTime Batdaudukien { get; set; }
        public DateTime Ketthucdukien { get; set; }
        public DateTime Ngaythucte { get; set; }
        public DateTime Batdautheothucte { get; set; }
        public DateTime Ketthuctheothucte { get; set; }
        public string Trangthai { get; set; } // Chua diem danh/ Diem danh
    }
}


