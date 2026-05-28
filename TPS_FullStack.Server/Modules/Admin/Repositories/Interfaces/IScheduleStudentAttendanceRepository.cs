using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public interface IScheduleStudentAttendanceRepository
    {
        public Task<List<ScheduleStudentAttendanceModel>> GetAllAsync(SqlConnection conn);
        public Task<ScheduleStudentAttendanceModel> GetByIDAsync(SqlConnection conn, string? MaID);
        public Task CreateAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string? LichhocID, string? HocvienID, string? KhoahocID, DateTime? Vaoluc, DateTime? Ngaydiemdanh);
        public Task UpdateAsync(SqlConnection conn, SqlTransaction trans, string? MaID, string? LichhocID, string? HocvienID, string? KhoahocID, DateTime? Vaoluc, DateTime? Ngaydiemdanh);
        public Task DeleteAsync(SqlConnection conn, SqlTransaction trans, string? MaID);
    }

}

