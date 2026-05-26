namespace TPS_FullStack.Server.Modules.Admin
{
    public class StudentsService : IStudentsService
    {
        private readonly IStudentsRepository _studentsRepository;
        public StudentsService(IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;

        }

        public async Task<StudentDetailDto> GetStudentByIDAsync(string MaID)
        {
            var result =await _studentsRepository.GetStudentByIDAsync(MaID);
            if(result == null)
            {
                return null;
            }
            return result;
            throw new NotImplementedException();
        }

        public async Task<bool> StudentDeleteAsync(string MaID)
        {
            var result =await _studentsRepository.StudentDeleteAsync(MaID);
            if (result)
            {
                return true;
            }
            return false;
            throw new NotImplementedException();
        }

        public async Task<List<StudentGetAllDto>> StudentGetAllsAsync()
        {
            var result = await _studentsRepository.StudentGetAllAsync();
            if(result == null)
            {
                return null;
            }
            return result; 
            throw new NotImplementedException();
        }

        public async Task<StudentCreateDto> StudentInsertAsync(StudentCreateDto createDto)
        {
            var result = await _studentsRepository.StudentInsertAsync(createDto);
            if (result)
            {
                return createDto;
            }
            return null;
            throw new NotImplementedException();
        }

        public async Task<StudentUpdateDto> StudentUpdateAsync(StudentUpdateDto updateDto)
        {
            var result = await _studentsRepository.StudentUpdateAsync(updateDto);
            if (result)
            {
                return updateDto;
            }
            return null;
            throw new NotImplementedException();
        }
    }

}

