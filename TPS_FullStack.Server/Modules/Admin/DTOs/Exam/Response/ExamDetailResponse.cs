using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Identity.Client;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class ExamDetailResponse
    {
        public string? MaID { get; set; }
        public string? KhoahocID { get; set; }
        public string? TenKhoahoc { get; set; }
        public string? HocvienID { get; set; }
        public string? TenHocvien { get; set; }
        public DateTime? Batdauthi { get; set; }
        public DateTime? Ketthucthi { get; set; }
        public decimal? Diem {get; set;}
        public List<ExamQuestionDetail>? examQuestionDetails { get; set; }
    }
    public class ExamQuestionDetail
    {
        public string? MaID { get; set; }
        public string? BaithuhoachID { get; set; }
        public string? CauhoiID { get; set; }
        public string? Tencauhoi { get; set; }
        public bool? Dung { get; set; }
        public List<ExamAnswerDetail>? examAnswerDetails { get; set; }
    }
    public class ExamAnswerDetail
    {
        public string? MaID { get; set; }
        public string? BaithuhoachID { get; set; }
        public string? Baithuhoach_CauhoiID { get; set; }
        public string? NdTraloi { get; set; }
        public bool? Dung { get; set; }
        public bool? Chon { get; set; }
    }

}

