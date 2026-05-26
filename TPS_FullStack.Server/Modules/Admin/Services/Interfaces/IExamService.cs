namespace TPS_FullStack.Server.Modules.Admin
{
    public interface IExamService
    {
        public Task<ServiceDefault<List<ExamResponse>>> ExamGetAllAsync();
        public Task<ServiceDefault<ExamDetailResponse>> ExamGetDetailAsync(string? MaID);
        public Task<ServiceDefault<ExamCreateResponse>> ExamCreateAsync(ExamCreateRequest createRequest);
        public Task<ServiceDefault<ExamUpdateResponse>> ExamUpdateAsync(ExamUpdateRequest updateRequest);
        public Task<ServiceDefault<bool>> ExamDeleteAsync(string? MaID);
    }

}

