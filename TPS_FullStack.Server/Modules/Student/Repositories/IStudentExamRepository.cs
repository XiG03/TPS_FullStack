using Microsoft.Extensions.Primitives;

namespace TPS_FullStack.Server.Modules.Student
{
    public interface IStudentExamRepository
    {
        public Task<BaithuhoachInfo> GetFinalExamInfoAsync(string HocvienID, string KhoahocID);
        public Task<decimal> SubmitFinalExamAsync(finalExam finalRecord);
    }

}

