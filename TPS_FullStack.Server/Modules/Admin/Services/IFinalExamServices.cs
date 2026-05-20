namespace TPS_FullStack.Server.Modules.Admin
{
    public interface IFinalExamServices
    {
        public Task<ServiceDefault<List<finalExams>>> GenerateFinalExamAsync (List<string> HocvienIDs, string KhoahocID, decimal Socauhoi);
        public Task<ServiceDefault<List<finalExamList>>> GetFinalExamListsAsync();
        public Task<ServiceDefault<List<finalExamList>>> GetFinalExamListsAsync(string KhoahocID);
        public Task<ServiceDefault<finalExamDetail>> GetFinalExamDetailAsync(string MaID);
        public Task<ServiceDefault<bool>> UpdateFinalExamScoreAsync(finalExamUpdate finalExamUpdate);
    }
}
