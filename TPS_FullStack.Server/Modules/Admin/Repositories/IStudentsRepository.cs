namespace TPS_FullStack.Server.Modules.Admin
{
    public interface IStudentsRepository
    {
        public Task<List<StudentGetAllDto>> StudentGetAllAsync();
        public Task<bool> StudentDeleteAsync(string MaID);
        public Task<StudentDetailDto> GetStudentByIDAsync(string MaID);
        public Task<bool> StudentInsertAsync(StudentCreateDto createDto);
        public Task<bool> StudentUpdateAsync(StudentUpdateDto updateDto);
    }

}

