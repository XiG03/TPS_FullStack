using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class ScheduleGetAllDto
    {
        public string MaID {get; set;}
        public string KhoahocID {get; set;}
        public string TenKhoahoc {get; set;}
        //public string GiangvienID {get; set;}
        //public string TenGiangvien {get; set;}
        public DateTime Ngaydukien { get; set; }
        public DateTime Batdaudukien { get; set; }
        public DateTime Ketthucdukien { get; set; }
        public DateTime Ngaythucte{ get; set; }
        public DateTime Batdauthucte { get; set; }
        public DateTime Ketthucthucte { get; set; }
    }

}

