namespace TPS_FullStack.Server.Modules.Admin
{
    public class ScheduleUpdateDto
    {
        public string MaID {get; set;}
        public DateTime Ngaydukien { get; set; }
        public DateTime Batdaudukien { get; set; }
        public DateTime Ketthucdukien { get; set; }
        public DateTime Ngaythucte { get; set; }
        public DateTime Batdauthucte { get; set; }
        public DateTime Ketthucthucte { get; set; }
    }

}

