using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.IdentityModel.Protocols.Configuration;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class Question
    {
        public string? ChuyendeID { get; set; }
        public string? CauhoiID { get; set; }
        public string? Ten { get; set; }
        public List<QuestionAnswer>? questionAnswers { get; set; }
    }
    public class QuestionAnswer
    {
        public string? CauhoiID { get; set; }
        public string? Ten { get; set; }
        public bool? Dung { get; set; }
    }

    public class ExamQuestion
    {
        public string? MaID { get; set; }
        public string? BaithuhoachID { get; set; }
        public string? CauhoiID { get; set; }
        public string? Tencauhoi { get; set; }
        public string? ChuyendeID { get; set; }
        public string? KhoahocID { get; set; }
        public List<ExamAnswers>? examAnswers { get; set; }
    }
    public class ExamAnswers
    {
        public string? MaID { get; set; }
        public string? BaithuhoachID { get; set; }
        public string? Baithuhoach_CauhoiID { get; set; }
        public string? NdTraloi { get; set; }
        public bool? Dung { get; set; }
        public bool? Chon { get; set; }
    }

}

