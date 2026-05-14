namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ICourseRepository
    {
        public Task<List<CourseInfo>> CourseGetAllAsync();
        public Task<bool> CourseInsertAsync(CourseCreateDto createDto);
        public Task<bool> CourseUpdateAsync(CourseUpdateDto updateDto);
        public Task<bool> CourseDeleteByIdAsync(string MaId);
        public Task<CourseDetailDto> CourseGetByIdAsync(string MaID);
        public Task<bool> UpdateStudentScoreAsync(updateStudentScoreDto updateDto);
        
    }

}

