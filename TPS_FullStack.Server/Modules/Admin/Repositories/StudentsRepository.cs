using System.Net.Http.Headers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using TPS_FullStack.Server.Entities;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class StudentsRepository : IStudentsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        public StudentsRepository(IConfiguration configuration,
                                UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<StudentDetailDto> GetStudentByIDAsync(string MaID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryGetStudentDetail = @"SELECT hv.MaID, hv.Hoten, hv.Email, hv.Dienthoai, hv.Diachi, hv.Gioitinh
                                            FROM dbo.Hocvien hv
                                            WHERE hv.MaID = @MaID";
            var studentDetail = new StudentDetailDto();
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(queryGetStudentDetail, conn))
                {
                    cmd.Parameters.AddWithValue("@MaID", MaID);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            studentDetail.MaID = reader["MaID"].ToString();
                            studentDetail.Hoten = reader["Hoten"].ToString();
                            studentDetail.Email = reader["Email"].ToString();
                            studentDetail.Dienthoai = reader["Dienthoai"].ToString();
                        }
                    }
                }
            }
            return studentDetail;
            throw new NotImplementedException();
        }

        public async Task<bool> StudentDeleteAsync(string MaID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryStudentDelete = @" UPDATE dbo.Hocvien
                                        SET
                                        	DeletedAt = SYSDATETIME(),
                                        	DeletedBy = ''
                                        WHERE MaID = @MaID";

            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(queryStudentDelete, conn))
                {
                    cmd.Parameters.AddWithValue("@MaId", MaID);

                    if (await cmd.ExecuteNonQueryAsync() < 0)
                    {
                        return false;
                    }
                }
            }
            return true;
            throw new NotImplementedException();
        }

        public async Task<List<StudentGetAllDto>> StudentGetAllAsync()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryGetAll = @"SELECT hv.MaID, hv.Hoten, hv.Email, hv.Dienthoai
                            FROM dbo.Hocvien hv ";
            var students = new List<StudentGetAllDto>();
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(queryGetAll, conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            students.Add(new StudentGetAllDto
                            {
                                MaID = reader["MaID"].ToString(),
                                Hoten = reader["Hoten"].ToString(),
                                Email = reader["Email"].ToString(),
                                Dienthoai = reader["Dienthoai"].ToString()
                            });
                        }
                    }
                }
            }
            return students;
            throw new NotImplementedException();
        }

        public async Task<bool> StudentInsertAsync(StudentCreateDto createDto)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryInsert = @"INSERT INTO dbo.Hocvien(MaID, UserId, Hoten, Email, Dienthoai, Ngaysinh, Gioitinh, Diachi, CreatedAt, CreatedBy)
                                VALUES (@MaID, @MaID,  @Hoten, @Email, @Dienthoai, @Ngaysinh, @Gioitinh, @Diachi, SYSDATETIME(), '')";
            createDto.MaID = Guid.NewGuid().ToString();


            using (var conn = new SqlConnection(connectionString))
            {
                var user = new AppUser
                {
                    Id = createDto.MaID,
                    UserName = createDto.Email,
                    PhoneNumber = createDto.Dienthoai,
                    Email = createDto.Email,
                    Kichhoat = false
                };
                var result = await _userManager.CreateAsync(user);

                if (!result.Succeeded)
                {
                    return false;
                }
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(queryInsert, conn))
                {
                    cmd.Parameters.AddWithValue("@MaID", createDto.MaID);
                    cmd.Parameters.AddWithValue("@Hoten", createDto.Hoten);
                    cmd.Parameters.AddWithValue("@Email", createDto.Email);
                    cmd.Parameters.AddWithValue("@Dienthoai", createDto.Dienthoai);
                    cmd.Parameters.AddWithValue("@Ngaysinh", createDto.Ngaysinh);
                    cmd.Parameters.AddWithValue("@Gioitinh", createDto.Gioitinh);
                    cmd.Parameters.AddWithValue("@Diachi", createDto.Diachi);
                    if (await cmd.ExecuteNonQueryAsync() < 0)
                    {
                        var student =await _userManager.FindByIdAsync(createDto.MaID);
                        await _userManager.DeleteAsync(student);
                        return false;
                    }
                }
            }
            return true;
            throw new NotImplementedException();
        }

        public async Task<bool> StudentUpdateAsync(StudentUpdateDto updateDto)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryStudentUpdate = @"UPDATE dbo.Hocvien
                                        SET
                                        	Hoten = @Hoten,
                                        	Email = @Email,
                                        	Dienthoai = @Dienthoai,
                                        	UpdatedAt = SYSDATETIME(),
                                        	UpdatedBy = ''
                                        WHERE MaID = @MaID";

            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(queryStudentUpdate, conn))
                {
                    cmd.Parameters.AddWithValue("@Hoten", updateDto.Hoten);
                    cmd.Parameters.AddWithValue("@Email", updateDto.Email);
                    cmd.Parameters.AddWithValue("@Dienthoai", updateDto.Dienthoai);
                    cmd.Parameters.AddWithValue("@MaID", updateDto.MaID);

                    if (await cmd.ExecuteNonQueryAsync() < 0)
                    {
                        return false;
                    }
                }
            }
            return true;
            throw new NotImplementedException();
        }
    }

}

