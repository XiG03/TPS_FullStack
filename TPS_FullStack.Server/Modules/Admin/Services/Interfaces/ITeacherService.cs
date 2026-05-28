using Microsoft.Identity.Client;

namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ITeacherService
    {
        public Task<ServiceDefault<TeacherCreateResponse>> TeacherCreateAsync(TeacherCreateResquest createResquest);
        public Task<ServiceDefault<TeacherUpdateResponse>> TeacherUpdateAsync (TeacherUpdateResquest updateResquest);
        public Task<ServiceDefault<List<TeacherResponse>>> TeacherGetAllAsync();
        public Task<ServiceDefault<TeacherDetailResponse> >TeacherGetByIDAsync(string? MaID);
        public Task<ServiceDefault<bool>> TeacherDeleteAsync(string? MaID);
        public Task<ServiceDefault<List<ScheduleResponse>>> TeacherGetScheduleByIDAsync(string? GiangvienID);
        public Task<ServiceDefault<ScheduleDetailResponse>> TeacherGetScheduleDetailAsync(string? GiangvienID, string? LichhocID);
        public Task<ServiceDefault<ScheduleTeacherAttendanceResponse>> TeacherCheckinAsync(string? GiangvienID, string? LichhocID);
        public Task<ServiceDefault<ScheduleTeacherAttendanceResponse>> TeacherCheckoutAsync(string? GiangvienID, string? LichhocID);
        
    }

}

