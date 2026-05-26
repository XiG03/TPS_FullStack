using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.IdentityModel.Protocols.Configuration;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class Question
    {
        public string? ChuyendeID { get; set; }
        public string? CauhoiID { get; set; }
        public string? Ten { get; set; }
        public List<QuestionAnswer> questionAnswers { get; set; }
    }
    public class QuestionAnswer
    {
        public string? CauhoiID { get; set; }
        public string? Ten { get; set; }
        public bool? Dung { get; set; }
    }

}

