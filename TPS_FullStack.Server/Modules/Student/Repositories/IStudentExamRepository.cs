using Microsoft.Extensions.Primitives;

namespace TPS_FullStack.Server.Modules.Student
{
    public interface IStudentExamRepository
    {
        public Task<courseQuestions> GetFinalExamInfo(string KhoahocID);
        public Task<bool> SubmitFinalExam(finalExam finalRecord);
    }

}

