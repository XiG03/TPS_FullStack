namespace TPS_FullStack.Server.Modules.Admin
{
    public class ExamUpdateRequest
    {
        public string? MaID { get; set; }
        public string? KhoahocID { get; set; }
        public string? TenKhoahoc { get; set; }
        public string? HocvienID { get; set; }
        public string? TenHocvien { get; set; }
        public DateTime? Batdauthi { get; set; }
        public DateTime? Ketthucthi { get; set; }
        public List<ExamQuestionUpdateRequest>? Cauhois { get; set; }
        
    }

    public class ExamQuestionUpdateRequest
    {
        public string? MaID {get; set;}
        public string? CauhoiID { get; set; }
        public string Tencauhoi {get; set;}
        public bool? Dung {get; set;}
        public string? ChuyendeID {get; set;}
        public string? KhoahocID {get; set;}
        public List<ExamAnswerUpdateRequest>? Tralois {get; set;}
    }
    public class ExamAnswerUpdateRequest
    {
        public string? MaID {get; set; }
        public string? BaithuhoachID {get; set;}
        public string? Baithuhoach_CauhoiID {get; set;}
        public string? NdTraloi {get; set;}
        public bool? Dung {get; set;}
        public bool? Chon {get; set;}
    }
}

