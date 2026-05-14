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

        [Column(TypeName = "NVARCHAR(50)")]
        public string? GiangvienID { get; set; }
        [Column(TypeName ="DATETIME")]
        public DateTime? Ngay { get; set; }
        [Column(TypeName ="DATETIME")]
        public DateTime? Batdaudukien { get; set; }
        [Column(TypeName ="DATETIME")]
        public DateTime? Ketthucdukien { get; set; }
    }
    public class Lichhoc_Hocvien_Diemdanh
    {
        [Key]
        [Column(TypeName = "NVARCHAR(50)")]
        public string MaID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string? LichhocID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string? HocvienID { get; set; }
    }
    public class Lichhoc_Giangvien_Diemdanh
    {
        [Key]
        [Column(TypeName = "NVARCHAR(50)")]
        public string MaID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string? LichhocID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string? GiangvienID { get; set; }
    }
}

