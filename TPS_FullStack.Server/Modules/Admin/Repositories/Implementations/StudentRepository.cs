using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using TPS_FullStack.Server.Entities;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class StudentRepository : IStudentRepository
    {
        private readonly UserManager<AppUser> _userManager;

        public StudentRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task CreateAsync(
            SqlConnection conn,
            SqlTransaction trans,
            string? HocvienID,
            string? Hoten,
            DateTime? Ngaysinh,
            string? Gioitinh,
            string? Email,
            string? Diachi,
            string? Dienthoai,
            DateTime? CreatedAt,
            string? CreatedBy,
            DateTime? UpdatedAt,
            string? UpdatedBy,
            DateTime? DeletedAt,
            string? DeletedBy)
        {
            var queryCreate = @"
                INSERT INTO dbo.Hocvien
                (
                    MaID,
                    UserId,
                    Hoten,
                    Ngaysinh,
                    Gioitinh,
                    Email,
                    Diachi,
                    Dienthoai,
                    CreatedAt,
                    CreatedBy,
                    UpdatedAt,
                    UpdatedBy,
                    DeletedAt,
                    DeletedBy,
                    Khongsudung
                )
                VALUES
                (
                    @MaID,
                    @UserId,
                    @Hoten,
                    @Ngaysinh,
                    @Gioitinh,
                    @Email,
                    @Diachi,
                    @Dienthoai,
                    @CreatedAt,
                    @CreatedBy,
                    @UpdatedAt,
                    @UpdatedBy,
                    @DeletedAt,
                    @DeletedBy,
                    0
                )";

            var user = new AppUser
            {
                Id = HocvienID,
                UserName = Email,
                Email = Email,
                PhoneNumber = Dienthoai,
                Kichhoat = false,
            };

            var result = await _userManager.CreateAsync(user, "TPs@123456");

            if (!result.Succeeded)
            {
                throw new Exception(
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            using (var cmd = new SqlCommand(queryCreate, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaID", (object?)HocvienID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UserId", (object?)HocvienID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Hoten", (object?)Hoten ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Ngaysinh", (object?)Ngaysinh ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Gioitinh", (object?)Gioitinh ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", (object?)Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Diachi", (object?)Diachi ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Dienthoai", (object?)Dienthoai ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedAt", (object?)CreatedAt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedBy", (object?)CreatedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UpdatedAt", (object?)UpdatedAt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UpdatedBy", (object?)UpdatedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DeletedAt", (object?)DeletedAt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DeletedBy", (object?)DeletedBy ?? DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAsync(
            SqlConnection conn,
            SqlTransaction trans,
            string HocvienID,
            DateTime? DeletedAt,
            string? DeletedBy)
        {
            var queryDelete = @"
                UPDATE dbo.Hocvien
                SET
                    Khongsudung = 1,
                    DeletedAt = @DeletedAt,
                    DeletedBy = @DeletedBy
                WHERE MaID = @MaID";

            using (var cmd = new SqlCommand(queryDelete, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaID", (object?)HocvienID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DeletedAt", (object?)DeletedAt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DeletedBy", (object?)DeletedBy ?? DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<StudentModel>> GetAllAsync(SqlConnection conn)
        {
            var queryGetAll = @"
                SELECT
                    hv.MaID,
                    hv.Hoten,
                    hv.Email,
                    hv.Dienthoai,
                    hv.Gioitinh,
                    hv.Diachi
                FROM dbo.Hocvien AS hv
                WHERE hv.Khongsudung = 0";

            var list = new List<StudentModel>();

            using (var cmd = new SqlCommand(queryGetAll, conn))
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(new StudentModel
                        {
                            MaID = reader["MaID"]?.ToString(),
                            Hoten = reader["Hoten"]?.ToString(),
                            Email = reader["Email"]?.ToString(),
                            Gioitinh = reader["Gioitinh"]?.ToString(),
                            Diachi = reader["Diachi"]?.ToString(),
                            Dienthoai = reader["Dienthoai"]?.ToString()
                        });
                    }
                }
            }

            return list;
        }

        public async Task<StudentModel?> GetByIDAsync(
            SqlConnection conn,
            string? HocvienID)
        {
            var queryGetByID = @"
                SELECT
                    MaID,
                    Hoten,
                    Ngaysinh,
                    Gioitinh,
                    Email,
                    Diachi,
                    Dienthoai
                FROM dbo.Hocvien
                WHERE MaID = @MaID
                    AND Khongsudung = 0";

            StudentModel? student = null;

            using (var cmd = new SqlCommand(queryGetByID, conn))
            {
                cmd.Parameters.AddWithValue(
                    "@MaID",
                    (object?)HocvienID ?? DBNull.Value);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        student = new StudentModel
                        {
                            MaID = reader["MaID"]?.ToString(),
                            Hoten = reader["Hoten"]?.ToString(),
                            Gioitinh = reader["Gioitinh"]?.ToString(),
                            Email = reader["Email"]?.ToString(),
                            Diachi = reader["Diachi"]?.ToString(),
                            Dienthoai = reader["Dienthoai"]?.ToString()
                        };
                    }
                }
            }

            return student;
        }

        public async Task UpdateAsync(
            SqlConnection conn,
            SqlTransaction trans,
            string? HocvienID,
            string? Hoten,
            DateTime? Ngaysinh,
            string? Gioitinh,
            string? Email,
            string? Diachi,
            string? Dienthoai,
            DateTime? CreatedAt,
            string? CreatedBy,
            DateTime? UpdatedAt,
            string? UpdatedBy,
            DateTime? DeletedAt,
            string? DeletedBy)
        {
            var queryUpdate = @"
                UPDATE dbo.Hocvien
                SET
                    Hoten = @Hoten,
                    Ngaysinh = @Ngaysinh,
                    Gioitinh = @Gioitinh,
                    Email = @Email,
                    Diachi = @Diachi,
                    Dienthoai = @Dienthoai,
                    UpdatedAt = @UpdatedAt,
                    UpdatedBy = @UpdatedBy
                WHERE MaID = @HocvienID";

            using (var cmd = new SqlCommand(queryUpdate, conn, trans))
            {
                cmd.Parameters.AddWithValue("@Hoten", (object?)Hoten ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Ngaysinh", (object?)Ngaysinh ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Gioitinh", (object?)Gioitinh ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", (object?)Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Diachi", (object?)Diachi ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Dienthoai", (object?)Dienthoai ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UpdatedAt", (object?)UpdatedAt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UpdatedBy", (object?)UpdatedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@HocvienID", (object?)HocvienID ?? DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}