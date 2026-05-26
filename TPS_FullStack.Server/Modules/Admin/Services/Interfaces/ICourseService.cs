namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ICourseService
    {
        public Task<ServiceDefault<List<CourseResponse>>> GetAllCoursesAsync();
        public Task<ServiceDefault<CourseDetailResponse>> GetCourseDetailAsync(string? KhoahocID);
        public Task<ServiceDefault<CourseCreateResponse>> CreateCourseAsync(CourseCreateRequest createRequest);
        public Task<ServiceDefault<CourseUpdateResponse>> UpdateCourseAsync(CourseUpdateRequest updateRequest);
        public Task<ServiceDefault<bool>> DeleteCourseAsync(string? KhoahocID);
    }

}

