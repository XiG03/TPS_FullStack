using System.ComponentModel.DataAnnotations;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class TopicUpdateDto
    {
        public string MaID { get; set; }
        public string Ten { get; set; }
        public string Mota { get; set; }

        public ICollection<DocumentsDto>? DocumentsDto { get; set; }
        public ICollection<QuestionsDto>? QuestionsDtos { get; set; }
    }
    public class DocumentsDto
    {
        public string MaID { get; set; }
        public string Tieude { get; set; }
        public string Loaitailieu { get; set; }
        public decimal Kichthuoc { get; set; }
    }

    public class QuestionsDto
    {
        public string MaID { get; set; }
        public string Ten { get; set; }
        [Range(0, 10)]
        public decimal Diem { get; set; }
        public ICollection<AnswersDto>?AnswersDtos { get; set; }
    }
    public class AnswersDto
    {

        public string MaID { get; set; }
        public string Ten { get; set; }

        public bool Dung { get; set; }
    }
}


