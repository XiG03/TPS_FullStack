namespace TPS_FullStack.Server.Modules.Admin
{
    public interface IFinalExamRepository
    {
        public Task<List<examQuestions>> GetExamQuestions(string KhoahocID);
        public Task<bool> FinalExamInsertAsync(List<finalExams> finalExamQuestionsList);


        public Task<List<finalExamList>> GetFinalExamListsAsync();
        public Task<List<finalExamList>> GetFinalExamListsAsync(string KhoahocID);
        public Task<finalExamDetail> GetFinalExamDetailAsync(string MaID);
        public Task<bool> UpdateFinalExamScoreAsync(finalExamUpdate finalExamUpdate);
    }

}

