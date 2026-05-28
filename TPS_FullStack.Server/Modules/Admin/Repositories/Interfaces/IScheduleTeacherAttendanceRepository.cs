using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public interface IScheduleTeacherAttendanceRepository
    {
        public Task<List<ScheduleTeacherAttendanceModel>> GetAllAsync(SqlConnection conn);
        public Task<ScheduleTeacherAttendanceModel> GetByIDAsync(SqlConnection conn, string? MaID);
        public Task CreateAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string? LichhocID, string? GiangvienID, string? KhoahocID, DateTime? Vaoluc, DateTime? Ngaydiemdanh);
        public Task UpdateAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string? LichhocID, string? GiangvienID, string? KhoahocID, DateTime? Vaoluc, DateTime? Ngaydiemdanh);
        public Task DeleteAsync(SqlConnection conn, SqlTransaction trans, string? MaID);
    }

}

