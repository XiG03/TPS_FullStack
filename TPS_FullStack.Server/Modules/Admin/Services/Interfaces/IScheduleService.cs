namespace TPS_FullStack.Server.Modules.Admin
{
    public interface IScheduleService
    {
        public Task<ServiceDefault<ScheduleCreateDto>> ScheduleInsertAsync(ScheduleCreateDto createDto);
        public Task<ServiceDefault<List<ScheduleGetAllDto>>> ScheduleGetAllAsync ();
        public Task<ServiceDefault<ScheduleUpdateDto>> ScheduleUpdateAsync(ScheduleUpdateDto updateDto);
        public Task<ServiceDefault<bool>> ScheduleDeleteAsync(string MaID);
        public Task<ServiceDefault<ScheduleTeacherAttendanceDto>> ScheduleTeacherAttendanceAsync(ScheduleTeacherAttendanceDto attendanceDto);
        public Task<ServiceDefault<ScheduleStudentAttendanceDto>> ScheduleStudentAttendanceAsync(ScheduleStudentAttendanceDto attendanceDto);
    }

}

