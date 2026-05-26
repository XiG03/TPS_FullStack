namespace TPS_FullStack.Server.Modules.Admin
{
    public class ScheduleUpdateRequest
    {
         public string? LichhocID { get; set; }
        public string? KhoahocID { get; set; }
        public string? ChuyendeID { get; set; }
        public string? GiangvienID { get; set; }
        public DateTime? Ngaydukien { get; set; }
        public DateTime? Batdaudukien { get; set; }
        public DateTime? Kethucdukien { get; set; }
        public DateTime? Ngaythucte { get; set; }
        public DateTime? Batdauthucte { get; set; }
        public DateTime? Kethucthucte { get; set; }
    }

}

