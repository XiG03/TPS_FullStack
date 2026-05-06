using TPS_FullStack.Server.Entities;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class TopicDtos
    {
    }

    public class ChuyendeDto
    {
        public string MaID { get; set; }
        public string Ten { get; set; }
        public string Mota { get; set; }
    }

    public class Chuyende_ChitietDto
    {
        public string MaID { get; set; }
        public string Ten { get; set; }
        public string Mota { get; set; }
        public ICollection<Chuyende_TailieuDto> Chuyende_TailieuDtos {get; set;} = new List<Chuyende_TailieuDto>();
        public ICollection<Chuyende_Cauhoi_DapanDto> Chuyende_Cauhoi_DapanDtos {get; set;} = new List<Chuyende_Cauhoi_DapanDto>();

    }
    public class Chuyende_TailieuDto
    {
        public string Tieude { get; set; }
        public string Loaitailieu { get; set; }
    }
    public class Chuyende_Cauhoi_DapanDto
    {
        public string Cauhoi_Ten {get; set;}
        public string Dapan_Ten {get; set;}
    }

}

