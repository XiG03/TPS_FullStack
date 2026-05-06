using System.ComponentModel.DataAnnotations;
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
        public ICollection<Chuyende_TailieuDtoR> Chuyende_TailieuDtos { get; set; } = new List<Chuyende_TailieuDtoR>();
        public ICollection<Chuyende_Cauhoi_DapanDtoR> Chuyende_Cauhoi_DapanDtos { get; set; } = new List<Chuyende_Cauhoi_DapanDtoR>();

    }
    public class Chuyende_TailieuDtoR
    {
        public string Tieude { get; set; }
        public string Loaitailieu { get; set; }
    }
    public class Chuyende_Cauhoi_DapanDtoR
    {
        public string Cauhoi_Ten { get; set; }
        public string Dapan_Ten { get; set; }
    }

    public class ChuyendeDto_CU
    {
        public string MaID { get; set; }
        public string Ten { get; set; }
        public string Mota { get; set; }
        public ICollection<Chuyende_CauhoiDto> chuyende_CauhoiDtos { get; set; }
        public ICollection<Chuyende_DapanDto> chuyende_DapanDtos { get; set; }
    }

    public class Chuyende_CauhoiDto
    {
        [Required]
        [StringLength(200)]
        public string Ten { get; set; }
        [Range(0, 9999999999999999.99)]
        public decimal Diem { get; set; }
    }
    public class Chuyende_DapanDto
    {
        [Required]
        [StringLength(200)]
        public string Ten { get; set; }

        public bool Dung { get; set; }
    }

}

