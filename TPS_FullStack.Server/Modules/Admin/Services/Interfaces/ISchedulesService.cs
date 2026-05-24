namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ISchedulesService
    {
        public Task<ServiceDefault<List<ScheduleResponse>>> ScheduleGetAllAsync();
        public Task<ServiceDefault<ScheduleDetailResponse>> ScheduleGetByIDAsync(string? MaID);
        public Task<ServiceDefault<ScheduleCreateResponse>> ScheduleCreateAsync(ScheduleCreateRequest createRequest);
        public Task<ServiceDefault<ScheduleUpdateResponse>> ScheduleUpdateAsync(ScheduleUpdateRequest updateRequest);
        public Task<ServiceDefault<bool>> ScheduleDeleteAsync(string? MaID);
    }

}

