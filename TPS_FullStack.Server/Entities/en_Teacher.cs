using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TPS_FullStack.Server.Entities
{
    public class en_Teacher
    {

    }

    public class Giangvien
    {
        [Key]
        [Column(TypeName = "NVARCHAR(50)")]
        public string MaID { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }

        [Column(TypeName = "NVARCHAR(200)")]
        public string? Hoten { get; set; }
        [Column(TypeName ="DATETIME")]
        public DateTime? Ngaysinh { get; set; }

        [Column(TypeName = "NVARCHAR(10)")]
        public string? Gioitinh { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string? Email { get; set; }

        [Column(TypeName = "NVARCHAR(200)")]
        public string? Diachi { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string? Dienthoai { get; set; }
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
        public bool? Khongsudung { get; set; }
    }

    // public class Teacher_Certificate
    // {
    //     public string MaID {get; set;}
    //     public string GiangvienID {get; set;}
    //     public string Ten {get; set;}
    //     public string 
    // }

}

