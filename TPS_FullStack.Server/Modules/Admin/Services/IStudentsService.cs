namespace TPS_FullStack.Server.Modules.Admin
{
    public interface IStudentsService
    {
        public Task<List<StudentGetAllDto>> StudentGetAllsAsync();
        public Task<StudentDetailDto> GetStudentByIDAsync(string MaID);
        public Task<StudentCreateDto> StudentInsertAsync(StudentCreateDto createDto);
        public Task<StudentUpdateDto> StudentUpdateAsync(StudentUpdateDto updateDto);
        public Task<bool> StudentDeleteAsync(string MaID);
    }

}

