using System.Runtime.InteropServices;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class CoursesService : ICoursesService
    {
        private readonly ICourseRepository _courseRepository;
        public CoursesService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }
        public async Task<bool> CourseDeleteByIdAsync(string MaID)
        {
            var result = await _courseRepository.CourseDeleteByIdAsync(MaID);
            if (!result)
            {
                return false;
            }
            return true;
            throw new NotImplementedException();
        }

        public async Task<List<CourseInfo>> CourseGetAllAsync()
        {
            var result = await _courseRepository.CourseGetAllAsync();
            if (result == null)
            {
                return null;
            }
            return result;
            throw new NotImplementedException();
        }

        public async Task<CourseDetailDto> CourseGetByIdAsync(string MaID)
        {
            var result = await _courseRepository.CourseGetByIdAsync(MaID);
            if(result == null)
            {
                return null;
            }
            return result;
            throw new NotImplementedException();
        }

        public async Task<CourseCreateDto> CourseInsertAsync(CourseCreateDto createDto)
        {
            var result = await _courseRepository.CourseInsertAsync(createDto);
            if (!result)
            {
                return null;
            }
            return createDto;
            throw new NotImplementedException();
        }

        public async Task<CourseUpdateDto> CourseUpdateAsync(CourseUpdateDto updateDto)
        {
            var result =await _courseRepository.CourseUpdateAsync(updateDto);
            if (!result)
            {
                return null;
            }
            return updateDto;
            throw new NotImplementedException();
        }
    }

}

