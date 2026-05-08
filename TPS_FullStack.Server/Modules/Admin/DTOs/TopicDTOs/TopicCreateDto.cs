using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TPS_FullStack.Server.Entities;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class TopicCreateDto
    {
    }

    

    public class Chuyende_ChitietDto
    {
        
        public Guid MaID { get; set; }
        public string Ten { get; set; }
        public string Mota { get; set; }
        public ICollection<Chuyende_TailieuDto>? Chuyende_TailieuDtos { get; set; } = new List<Chuyende_TailieuDto>();
        public ICollection<Chuyende_CauhoiDto>? Chuyende_CauhoiDtos { get; set; }
    }
    public class Chuyende_TailieuDto
    {
        
        public Guid MaID { get; set; }
        
        public Guid ChuyendeID { get; set; }
        public string Tieude { get; set; }
        public string Loaitailieu { get; set; }
        public decimal Kichthuoc { get; set; }
    }

    public class Chuyende_CauhoiDto
    {
        
        public Guid MaID { get; set; }
        
        public Guid ChuyendeID { get; set; }
        [Required]
        [StringLength(200)]
        public string Ten { get; set; }
        [Range(0, 1)]
        public decimal Diem { get; set; }
        public ICollection<Chuyende_DapanDto>? Chuyende_DapanDtos { get; set; }
    }
    public class Chuyende_DapanDto
    {
        
        public Guid MaID { get; set; }
        
        public Guid Chuyende_CauhoiID { get; set; }
        [Required]
        [StringLength(200)]
        public string Ten { get; set; }

        public bool Dung { get; set; }
    }

    // public class ChuyendeDto
    // {
    //     public Guid MaID { get; set; }
    //     public string Ten { get; set; }
    //     public string Mota { get; set; }
    // }

}

