namespace TPS_FullStack.Server.Modules.Admin
{
    public interface IScheduleRepository
    {
        public Task<bool> ScheduleInsertAsync(ScheduleCreateDto createDto);
        public Task<List<ScheduleGetAllDto>> ScheduleGetAllAsync();
        //Sprint 2
        public Task<ScheduleDetailDto> ScheduleGetById(string MaID);
        public Task<bool> ScheduleUpdateAsync(ScheduleUpdateDto updateDto);
        public Task<bool> ScheduleDeleteAsync(string MaID);

        // Giai thich tieng viet: Admin se co the thuc hien diem danh thu cong cho giang vien hoac hoc vien
        public Task<bool> ScheduleTeacherAttandanceAsync(ScheduleTeacherAttendanceDto attendanceDto);
        public Task<bool> ScheduleStudentAttendanceAsync(ScheduleStudentAttendanceDto attendanceDto);
        
    }

}

