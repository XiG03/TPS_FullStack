namespace TPS_FullStack.Server.Modules.Admin
{
    public class TopicDetailResponse
    {
        public string ChuyendeID { get; set; }

        public string Ten { get; set; }

        public string? Mota { get; set; }

        public List<TopicDocumentDetailResponse> Documents { get; set; } = new();

        public List<TopicQuestionDetailResponse> Questions { get; set; } = new();
    }
    public class TopicDocumentDetailResponse
    {
        public string TailieuID { get; set; }

        public string Tieude { get; set; }

        public DateTime? Ngaytao { get; set; }

        public string? Loaitailieu { get; set; }

        public decimal? Kichthuoc { get; set; }
    }
    public class TopicQuestionDetailResponse
    {
        public string CauhoiID { get; set; }

        public string Ten { get; set; }

        public List<TopicAnswerDetailResponse> Answers { get; set; } = new();
    }
    public class TopicAnswerDetailResponse
    {
        public string DapanID { get; set; }

        public string Ten { get; set; }

        public bool Dung { get; set; }
    }
}

