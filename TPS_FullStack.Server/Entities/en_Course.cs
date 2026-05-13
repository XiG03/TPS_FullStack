using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TPS_FullStack.Server.Entities
{

    public class en_Course
    {

    }

    public class Khoahoc
    {
        [Key]
        [Column(TypeName = "NVARCHAR(50)")]
        public string MaID { get; set; }

        [Column(TypeName = "NVARCHAR(200)")]
        public string? Ten { get; set; }

        [Column(TypeName = "NVARCHAR(500)")]
        public string? Mota { get; set; }

        [Column(TypeName ="DECIMAL(18,2)")]
        public decimal? Diemdat { get; set; } // Set Decimal(18,2), he 100
        
        [Column(TypeName = "NVARCHAR(50)")]
        public string? ChungchiID { get; set; }
        public bool? Khongsudung { get; set; }
        [Column(TypeName ="DATETIME")]
        public DateTime? CreatedAt { get; set; }
        [Column(TypeName = "NVARCHAR(450)")]
        public string? CreatedBy { get; set; }
        [Column(TypeName ="DATETIME")]
        public DateTime? UpdatedAt { get; set; }
        [Column(TypeName = "NVARCHAR(450)")]
        public string? UpdatedBy { get; set; }
        [Column(TypeName ="DATETIME")]
        public DateTime? DeletedAt { get; set; }
        [Column(TypeName = "NVARCHAR(450)")]
        public string? DeletedBy { get; set; }
        [Column(TypeName = "NVARCHAR(100)")]
        public string? Thu { get; set; }

        [Column(TypeName ="DECIMAL(4,1)")]
        public decimal? Thoigianhoc { get; set; } // Format Decimal(4,1)
        
        [Column(TypeName ="DECIMAL(4,1)")]
        public decimal? Sobuoihoc { get; set; } // Format Decimal(18,0)
    }

    public class Khoahoc_Hocvien
    {
        [Key]
        [Column(TypeName = "NVARCHAR(50)")]
        public string MaID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string? KhoahocID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string? HocvienID { get; set; }

        [Column(TypeName ="DECIMAL(18,2)")]
        public decimal? Diem { get; set; }

        [Column(TypeName ="DECIMAL(18,2)")]
        public decimal? Dieuchinh { get; set; }
    }

    public class Khoahoc_Giangvien
    {
        [Key]
        [Column(TypeName = "NVARCHAR(50)")]
        public string MaID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string? KhoahocID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string? GiangvienID { get; set; }
    }

    public class Khoahoc_Chuyende
    {
        [Key]
        [Column(TypeName = "NVARCHAR(50)")]
        public string? MaID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string? KhoahocID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string? ChuyendeID { get; set; }
        [Column(TypeName ="DECIMAL(18,2)")]
        public decimal? slCauhoi{get; set;}
    }
    public class Khoahoc_dmTrangthai
    {
        [Key]
        [Column(TypeName = "NVARCHAR(50)")]
        public string MaID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string? KhoahocID { get; set; }

        [Column(TypeName = "NVARCHAR(200)")]
        public string? Ten { get; set; }
    }
}

