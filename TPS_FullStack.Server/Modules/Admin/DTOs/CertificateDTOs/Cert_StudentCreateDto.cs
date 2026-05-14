using System.Text.Json.Serialization;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class Cert_StudentCreateDto
    {
        public string MaID { get; set; }
        public string ChungchiID { get; set; }
        public string HocvienID { get; set; }
        public string Ten { get; set; }
        public string Mota { get; set; }
        public string Donvicap { get; set; }
        [JsonIgnore]
        public string Ngaycap { get; set; }
        public decimal Thoigiansudung { get; set; }
        [JsonIgnore]
        public string Ngayhethan { get; set; }
        [JsonIgnore]
        public bool? Khongsudung { get; set; }
    }

}

