namespace TPS_FullStack.Server.Modules.Admin
{
    public class ExamSubmitResquest
    {
        public string? MaID { get; set; }
        public string? KhoahocID { get; set; }
        public string? TenKhoahoc { get; set; }
        public string? HocvienID { get; set; }
        public string? TenHocvien { get; set; }
        public DateTime? Batdauthi { get; set; }
        public DateTime? Ketthucthi { get; set; }
        public decimal? Diem {get; set;}
        public List<ExamAnswerSubmit>? answerSubmits { get; set; }
    }
    public class ExamAnswerSubmit
    {
        public string? MaID { get; set; }
        public string? BaithuhoachID { get; set; }
        public string? Baithuhoach_CauhoiID { get; set; }
        public string? NdTraloi { get; set; }
        public bool? Dung { get; set; }
        public bool? Chon { get; set; }
    }

}