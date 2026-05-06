using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TPS_FullStack.Server.Entities
{
    public class en_Schedule
    {

    }

    public class Lichhoc
    {
        [Key]
        [Column(TypeName = "NVARCHAR(50)")]
        public string MaID { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        public string Thu { get; set; }

        [Column(TypeName ="DECIMAL(4,1)")]
        public decimal Thoigianhoc { get; set; } // Format Decimal(4,1)
        
        [Column(TypeName ="DECIMAL(4,1)")]
        public decimal Sobuoihoc { get; set; } // Format Decimal(18,0)
    }
    public class Lichhoc_Ct
    {
        [Key]
        [Column(TypeName = "NVARCHAR(50)")]
        public string MaID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string LichhocID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string GiangvienID { get; set; }
        public DateTime Ngay { get; set; }
        public DateTime Tugio { get; set; }
        public DateTime Dengio { get; set; }

        [Column(TypeName ="DECIMAL(4,1)")]
        public decimal Lichhoc_Thoigianhoc { get; set; } // Format Decimal(4,1)

    }
    public class Lichhoc_Ct_Diemdanh
    {
        [Key]
        [Column(TypeName = "NVARCHAR(50)")]
        public string MaID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string Lichhoc_CtID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string HocvienID { get; set; }
    }
}

