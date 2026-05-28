namespace TPS_FullStack.Server.Modules.Admin
{
    public interface IStudentService
    {
        public Task<ServiceDefault<List<StudentResponse>>> StudentGetAllAsync();
        public Task<ServiceDefault<StudentDetailResponse>> StudentGetByIDAsync(string? HocvienID);
        public Task<ServiceDefault<StudentCreateResponse>> StudentCreateAsync(StudentCreateRequest createRequest);
        public Task<ServiceDefault<StudentUpdateResponse>> StudentUpdateAsync(StudentUpdateRequest updateRequest);
        public Task<ServiceDefault<bool>> StudentDeleteAsync(string? HocvienID);
        public Task<ServiceDefault<List<ScheduleResponse>>> StudentGetScheduleByIDAsync(string? HocvienID);
        public Task<ServiceDefault<ScheduleDetailResponse>> StudentGetScheduleDetailAsync(string? HocvienID, string? LichhocID);
        public Task<ServiceDefault<ScheduleStudentAttendanceResponse>> StudentCheckinAsync(string? HocvienID, string? LichhocID);
    }

}

