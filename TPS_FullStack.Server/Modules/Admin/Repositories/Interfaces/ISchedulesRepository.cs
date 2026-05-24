using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ISchedulesRepository
    {
        public Task<List<ScheduleModel>> GetAllAsync(SqlConnection conn);
        public Task<ScheduleModel> GetByIDAsync(SqlConnection conn, string? LichhocID);
        public Task CreateAsync(SqlConnection conn, SqlTransaction trans, string? LichhocID, string? KhoahocID, string? ChuyendeID, string? GiangvienID,
                                DateTime? Ngaydukien, DateTime? Batdaudukien, DateTime? Ketthucdukien, DateTime? Ngaythucte, DateTime? Batdauthucte, DateTime? Ketthucthucte);
        public Task UpdateAsync (SqlConnection conn, SqlTransaction trans, string? LichhocID, string? KhoahocID, string? ChuyendeID, string? GiangvienID,
                                DateTime? Ngaydukien, DateTime? Batdaudukien, DateTime? Ketthucdukien, DateTime? Ngaythucte, DateTime? Batdauthucte, DateTime? Ketthucthucte);
        public Task DeleteAsync (SqlConnection conn, SqlTransaction trans, string? LichhocID);
    }

}

