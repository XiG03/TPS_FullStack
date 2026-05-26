namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ITeachersService
    {
        public Task<List<TeacherGetAllDto>> TeacherGetAllAsync();
        public Task<TeacherCreateDto> CreateTeacherAsync(TeacherCreateDto createDto);
        public Task<TeacherDetailDto> TeacherDetailAsync(string MaID);
        public Task<bool> DeleteTeacherAsync(string MaID);
        public Task<TeacherUpdateDto> UpdateTeacherAsync (TeacherUpdateDto updateDto);
    }

}

