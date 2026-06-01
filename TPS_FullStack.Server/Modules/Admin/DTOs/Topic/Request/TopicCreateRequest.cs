namespace TPS_FullStack.Server.Modules.Admin
{
    public class TopicCreateRequest
    {
        public string? ChuyendeID { get; set; }
        public string Ten { get; set; }
        public string? Mota { get; set; }
        public List<TopicDocumentCreateRequest> Documents { get; set; } = new();
        public List<TopicQuestionCreateRequest> Questions { get; set; } = new();
    }
    public class TopicDocumentCreateRequest
    {
        public string? TailieuID { get; set; }
        public string Tieude { get; set; }
        public DateTime Ngaytao { get; set; }
        public string Loaitailieu { get; set; }
        public decimal Kichthuoc { get; set; }
        public IFormFile? File { get; set; }
    }
    public class TopicQuestionCreateRequest
    {
        public string? CauhoiID { get; set; }
        public string Ten { get; set; }
        public List<TopicAnswerCreateRequest> Answers { get; set; } = new();
    }
    public class TopicAnswerCreateRequest
    {
        public string? DapanID { get; set; }
        public string Ten { get; set; }
        public bool Dung { get; set; }
    }

}

