using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using TPS_FullStack.Server.Entities;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly UserManager<AppUser> _userManager;
        public TeacherRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task CreateAsync(SqlConnection conn, SqlTransaction trans, string? GiangvienID, string? Hoten, DateTime? Ngaysinh, string? Gioitinh, string? Email, string? Diachi, string? Dienthoai, DateTime? CreatedAt, string? CreatedBy, DateTime? UpdatedAt, string? UpdatedBy, DateTime? DeletedAt, string? DeletedBy)
        {
            var queryCreate = @"INSERT INTO dbo.Giangvien(MaID, UserId, Hoten, Ngaysinh, Gioitinh, Email, Diachi, Dienthoai,CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, DeletedAt, DeletedBy,  Khongsudung)
                                VALUES (@MaID, @UserId, @Hoten, @Ngaysinh, @Gioitinh, @Email, @Diachi, @Dienthoai, @CreatedAt, @CreatedBy, @UpdatedAt, @UpdatedBy, @DeletedAt, @DeletedBy, 0)";


            // Tao User manager

            var user = new AppUser
            {
                Id = GiangvienID,
                UserName = Email,
                PhoneNumber = Dienthoai,
                Email = Email,
                Kichhoat = false,
            };

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            using (var cmd = new SqlCommand(queryCreate, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaID", GiangvienID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@UserId", GiangvienID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Hoten", Hoten ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Ngaysinh", Ngaysinh ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Gioitinh", Gioitinh ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", Email ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Diachi", Diachi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Dienthoai", Dienthoai ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedAt", CreatedAt ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@UpdatedAt", UpdatedAt ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DeletedAt", DeletedAt ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DeletedBy", DeletedBy ?? (object)DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }
            // throw new NotImplementedException();
        }

        public async Task DeleteAsync(SqlConnection conn, SqlTransaction trans, string GiangvienID, DateTime? DeletedAt, string? DeletedBy)
        {
            var queryDelete = @"UPDATE dbo.Giangvien
                        SET 
                        	Khongsudung = 1,
                            DeletedAt = @DeletedAt,
                            DeletedBy = @DeletedBy
                        WHERE MaID = @MaID";

            using (var cmd = new SqlCommand(queryDelete, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaID", GiangvienID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DeletedAt", DeletedAt ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DeletedBy", DeletedBy ?? (object)DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }

            // throw new NotImplementedException();
        }

        public async Task<List<TeacherModel>> GetAllAsync(SqlConnection conn)
        {
            var queryGetAll = @"SELECT gv.MaID, gv.Hoten, gv.Email, gv.Dienthoai, gv.Gioitinh, gv.Diachi FROM dbo.Giangvien AS gv
                                WHERE gv.Khongsudung = 0";

            var list = new List<TeacherModel>();
            using (var cmd = new SqlCommand(queryGetAll, conn))
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(new TeacherModel
                        {
                            MaID = reader["MaID"].ToString(),
                            Hoten = reader["Hoten"].ToString(),
                            Email = reader["Email"].ToString(),
                            Gioitinh = reader["Gioitinh"].ToString(),
                            Diachi = reader["Diachi"].ToString(),
                            Dienthoai = reader["Dienthoai"].ToString()
                        });
                    }
                }
                return list;
            }
            // throw new NotImplementedException();
        }

        public async Task<TeacherModel> GetByIDAsync(SqlConnection conn, string? MaID)
        {
            var queryGetByID = @"SELECT MaID, Hoten, Gioitinh, Email, Diachi, Dienthoai	
                                FROM Giangvien
                                WHERE MaID = @MaID AND Khongsudung = 0";
            var teacher = new TeacherModel();
            using (var cmd = new SqlCommand(queryGetByID, conn))
            {
                cmd.Parameters.AddWithValue("@MaID", MaID);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        teacher.MaID = reader["MaID"].ToString();
                        teacher.Hoten = reader["Hoten"].ToString();
                        teacher.Gioitinh = reader["Gioitinh"].ToString();
                        teacher.Email = reader["Email"].ToString();
                        teacher.Diachi = reader["Diachi"].ToString();
                        teacher.Dienthoai = reader["Dienthoai"].ToString();
                    }
                }
            }
            return teacher;
            // throw new NotImplementedException();
        }

        public async Task UpdateAsync(SqlConnection conn, SqlTransaction trans, string? GiangvienID, string? Hoten, DateTime? Ngaysinh, string? Gioitinh, string? Email, string? Diachi, string? Dienthoai,
                                DateTime? CreatedAt, string? CreatedBy, DateTime? UpdatedAt, string? UpdatedBy, DateTime? DeletedAt, string? DeletedBy)
        {
            var queryUpdate = @"UPDATE dbo.Giangvien
                                SET
                                    Hoten = @Hoten, Ngaysinh = @Ngaysinh,
                                    Gioitinh = @Gioitinh, Email = @Email,
                                    Diachi = @Diachi, Dienthoai = @Dienthoai,
                                    UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
                                WHERE MaID = @GiangvienID";
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
                cmd.Parameters.AddWithValue("@GiangvienID", (object?)GiangvienID ?? DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }
            // throw new NotImplementedException();
        }
    }
}

