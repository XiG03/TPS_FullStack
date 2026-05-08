using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Identity.Client;

namespace TPS_FullStack.Server.Entities
{
    public class en_Certification
    {

    }

    public class Chungchi
    {
        [Key]
        [Column(TypeName = "NVARCHAR(50)")]
        public string MaID { get; set; }

        [Column(TypeName = "NVARCHAR(200)")]
        public string Ten { get; set; }

        [Column(TypeName = "NVARCHAR(500)")]
        public string Mota { get; set; }

        [Column(TypeName = "NVARCHAR(500)")]
        public string Donvicap { get; set; }

        [Column(TypeName ="DECIMAL(18,2)")]
        public decimal Thoigiansudung { get; set; } // Format Decimal(18,,2)
        public bool? Khongsudung { get; set; }
        public DateTime? CreatedAt { get; set; }
        [Column(TypeName = "NVARCHAR(450)")]
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [Column(TypeName = "NVARCHAR(450)")]
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        [Column(TypeName = "NVARCHAR(450)")]
        public string? DeletedBy { get; set; }

    }

    public class Chungchi_Hocvien
    {
        [Key]
        [Column(TypeName = "NVARCHAR(50)")]
        public string MaID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string KhoahocID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string ChungchiID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string HocvienID { get; set; }

        [Column(TypeName = "NVARCHAR(200)")]
        public string Chungchi_Ten { get; set; }

        [Column(TypeName = "NVARCHAR(500)")]
        public string Chungchi_Mota { get; set; }

        [Column(TypeName = "NVARCHAR(200)")]
        public string Chungchi_Donvicap { get; set; }

        [Column(TypeName ="DECIMAL(18,2)")]
        public decimal Chungchi_Thoigiansudung { get; set; }
        public DateTime Ngaycap { get; set; }
        public DateTime Ngayhethan { get; set; }
        public bool Khongsudung { get; set;}
    }
}

