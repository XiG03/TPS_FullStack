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
    }

}

