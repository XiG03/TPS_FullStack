namespace TPS_FullStack.Server.Modules.Admin
{
    public class TopicDocumentModel
    {  
        public string MaID {get; set;}
        public string ChuyendeID {get; set; }
        public string Tieude {get; set;}
        public DateTime Ngaytao {get; set;}
        public string Loaitailieu {get; set;}
        public decimal Kichthuoc {get; set;}
        public string? Duongdan { get; set; }
        public string? TentepGoc { get; set; }
    }

}

