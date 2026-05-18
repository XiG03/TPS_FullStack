namespace TPS_FullStack.Server.Modules.Admin
{

    public class ScheduleCreateDto
    {
        public string MaID { get; set; }
        public string KhoahocID {get; set;}
        public DateTime Ngaydukien { get; set; }
        public DateTime Batdaudukien { get; set; }
        public DateTime Ketthucdukien { get; set; }
    }
}

