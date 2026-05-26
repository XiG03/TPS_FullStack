namespace TPS_FullStack.Server.Modules.Admin
{
    public class LabInsert
    {
        public LabSchedule schedule { get; set; }
        public LabInformation lab { get; set; }
    }
    public class LabSchedule
    {
        public string? LichhocID { get; set; }
        public string? KhoahocID { get; set; }
        public DateTime? Ngaydukien { get; set; }
        public DateTime? Batdaudukien { get; set; }
        public DateTime Ketthucdukien { get; set; }
    }
    public class LabInformation
    {
        public string? ThuchanhID { get; set; }
        public string? KhoahocID { get; set; }
        public string? GiangvienID { get; set; }
        public string? ChuyendeID { get; set; }
        public string? Diachi { get; set; }
        public decimal? Soluongtoida { get; set; }
    }
}

