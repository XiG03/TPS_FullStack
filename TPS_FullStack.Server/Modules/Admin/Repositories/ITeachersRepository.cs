namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ITeachersRepository
    {
        public Task<List<TeacherGetAllDto>> TeacherGetAllAsync();
        public Task<TeacherDetailDto> TeacherDetailAsync(string MaID);
        public Task<bool> CreateTeacherAsync(TeacherCreateDto createDto);
        public Task<bool> DeleteTeacherAsync(string MaID);
        public Task<bool> UpdateTeacherAsync(TeacherUpdateDto updateDto);
    }

}

