namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ICoursesService
    {
        public Task<List<CourseInfo>> CourseGetAllAsync();
        public Task<CourseCreateDto> CourseInsertAsync(CourseCreateDto createDto);
        public Task<CourseUpdateDto> CourseUpdateAsync(CourseUpdateDto updateDto);
        public Task<bool> CourseDeleteByIdAsync(string MaID);
        public Task<CourseDetailDto> CourseGetByIdAsync(string MaID);
    }

}
