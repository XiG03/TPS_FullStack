namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ICourseRepository
    {
        public Task<List<CourseInfo>> CourseGetAllAsync();
        public Task<bool> DeleteCourseAsync(string MaId);
    }

}

