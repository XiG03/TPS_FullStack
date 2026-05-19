namespace TPS_FullStack.Server.Modules.Admin
{
    public interface IFinalExamServices
    {
        public Task<ServiceDefault<List<finalExams>>> GenerateFinalExamAsync (List<string> HocvienIDs, string KhoahocID, decimal Socauhoi);
    }
}
