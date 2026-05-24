namespace TPS_FullStack.Server.Modules.Admin
{
    public class TopicUpdateRequest
    {
        public string ChuyendeID { get; set; }
        public string Ten { get; set; }
        public string Mota { get; set; }
        public List<TopicDocumentUpdateRequest> Documents { get; set; } = new List<TopicDocumentUpdateRequest>();
        public List<TopicQuestionUpdateRequest> Questions { get; set; } = new List<TopicQuestionUpdateRequest>();
    }

    public class TopicDocumentUpdateRequest
    {
        // Nếu TailieuID null hoặc rỗng -> Insert mới. Nếu có giá trị -> Update
        public string TailieuID { get; set; } 
        public string Tieude { get; set; }
        public DateTime Ngaytao { get; set; }
        public string Loaitailieu { get; set; }
        public decimal Kichthuoc { get; set; }
    }

    public class TopicQuestionUpdateRequest
    {
        // Nếu CauhoiID null hoặc rỗng -> Insert mới. Nếu có giá trị -> Update
        public string CauhoiID { get; set; } 
        public string Ten { get; set; }
        public List<TopicAnswerUpdateRequest> Answers { get; set; } = new List<TopicAnswerUpdateRequest>();
    }

    public class TopicAnswerUpdateRequest
    {
        // Nếu DapanID null hoặc rỗng -> Insert mới. Nếu có giá trị -> Update
        public string DapanID { get; set; } 
        public string Ten { get; set; }
        public bool Dung { get; set; }
    }
}