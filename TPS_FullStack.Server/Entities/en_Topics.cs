using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TPS_FullStack.Server.Entities
{
    public class Topics
    {

    }

    public class Chuyende
    {
        [Key]
        [Column(TypeName = "NVARCHAR(50)")]
        public string MaID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string Ten { get; set; }

        [Column(TypeName = "NVARCHAR(500)")]
        public string Mota { get; set; }
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

    public class Chuyende_Tailieu
    {
        [Key]
        [Column(TypeName = "NVARCHAR(50)")]
        public string MaID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string ChuyendeID { get; set; }

        [Column(TypeName = "NVARCHAR(200)")]
        public string Tieude { get; set; }
        public DateTime Ngaytao { get; set; }
        public string Loaitailieu { get; set; }

        [Column(TypeName = "DECIMAL(18,2)")]
        public decimal Kichthuoc { get; set; }
        public bool? Khongsudung { get; set; }
    }

    public class Chuyende_Cauhoi
    {
        [Key]
        [Column(TypeName = "NVARCHAR(50)")]
        public string MaID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string ChuyendeId { get; set; }

        [Column(TypeName = "NVARCHAR(200)")]
        public string Ten { get; set; }

        [Column(TypeName = "DECIMAL(18,2)")]
        public decimal Diem { get; set; } // Format  Decimal (18,2)

    }
    public class Chuyende_Dapan
    {
        [Key]
        [Column(TypeName = "NVARCHAR(50)")]
        public string MaId { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string Chuyende_CauhoiID { get; set; }

        [Column(TypeName = "NVARCHAR(200)")]
        public string Ten { get; set; }
        public bool Dung { get; set; }
    }

    // Chuyende_Giaovien
}

