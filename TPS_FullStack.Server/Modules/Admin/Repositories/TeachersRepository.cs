

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.JSInterop.Infrastructure;
using TPS_FullStack.Server.Entities;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class TeachersRepository : ITeachersRepository
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<AppUser> _logger;
        public TeachersRepository(IConfiguration configuration, UserManager<AppUser> userManager,
                                ILogger<AppUser> logger)
        {
            _configuration = configuration;
            _userManager = userManager;
            _logger = logger;
        }


        // Tao bang AspNetUser (chua kich hoat) + Bang giang vien + Chuyen de giang vien
        //      EF Core                             ADO.NET             ADO.NET
        public async Task<bool> CreateTeacherAsync(TeacherCreateDto createDto)
        {
            createDto.MaID = Guid.NewGuid().ToString();
            //Step 1: Tao AspNetUser moi // chua verify
            var user = new AppUser
            {
                // Dang ep cho Id cua AspNetUser la MaID cua giang vien // du kien se lam cho hoc vien
                Id = createDto.MaID,
                UserName = createDto.Email,
                PhoneNumber = createDto.Dienthoai,
                Email = createDto.Email,
                Kichhoat = false,
            };

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                return false;
            }
            //Step 2: Them giang vien moi -- ADO.NET 

            var queryTeacher = @"INSERT INTO dbo.Giangvien(MaID, UserId, Hoten, Ngaysinh, Gioitinh, Email, Diachi, Dienthoai,CreatedAt, CreatedBy, UpdatedAt, UpdatedBy)
                                VALUES (@MaID, @UserId, @Hoten, @Ngaysinh, @Gioitinh, @Email, @Diachi, @Dienthoai,SYSDATETIME(), '', SYSDATETIME(), '')";

            var queryTeacherTopic = @"INSERT INTO dbo.Chuyende_Giangvien(MaID, ChuyendeID, GiangvienID)
                                    VALUES(@MaID, @ChuyendeID, @GiangvienID)";

            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (var transaction = conn.BeginTransaction())
                    {
                        using (var cmd = new SqlCommand(queryTeacher, conn))
                        {
                            cmd.Parameters.AddWithValue("@MaID", createDto.MaID);
                            cmd.Parameters.AddWithValue("@UserId", createDto.MaID);
                            cmd.Parameters.AddWithValue("@Hoten", createDto.Hoten);
                            cmd.Parameters.AddWithValue("@Ngaysinh", createDto.Ngaysinh);
                            cmd.Parameters.AddWithValue("@Gioitinh", createDto.Gioitinh);
                            cmd.Parameters.AddWithValue("@Email", createDto.Email);
                            cmd.Parameters.AddWithValue("@Diachi", createDto.Diachi);
                            cmd.Parameters.AddWithValue("@Dienthoai", createDto.Dienthoai);

                            if (await cmd.ExecuteNonQueryAsync() <= 0)
                            {
                                return false;
                            }
                        }
                        if (createDto.topics != null)
                        {
                            var GiangvienID = createDto.MaID;
                            foreach (var topic in createDto.topics)
                            {
                                using (var cmd = new SqlCommand(queryTeacherTopic, conn))
                                {
                                    cmd.Parameters.AddWithValue("@MaID", Guid.NewGuid().ToString());
                                    cmd.Parameters.AddWithValue("@ChuyendeID", topic.MaID);
                                    cmd.Parameters.AddWithValue("@GiangvienID", GiangvienID);

                                    if (await cmd.ExecuteNonQueryAsync() <= 0)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }


            throw new NotImplementedException();
        }

        public async Task<bool> DeleteTeacherAsync(string MaID)
        {
            var query = @"UPDATE dbo.Giangvien
                        SET 
                        	Khongsudung = 1
                        WHERE MaID = @MaID";

            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            try
            {
                using(var conn = new SqlConnection(connectionString))
                {
                    conn.OpenAsync();
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaID", MaID);

                        if(await cmd.ExecuteNonQueryAsync() <= 0)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
            throw new NotImplementedException();
        }

        public async Task<TeacherDetailDto> TeacherDetailAsync(string MaID)
        {
            var teacherInfo = @"SELECT MaID, HoTen, Gioitinh, Email, Diachi, Dienthoai	
                                FROM Giangvien
                                WHERE MaID = @MaID AND Khongsudung = 0";

            var teacherTopics = @"SELECT cd.MaID, cd.Ten
                                FROM Chuyende cd 
                                JOIN Chuyende_Giangvien cd_gv ON cd.MaID = cd_gv.ChuyendeID
                                WHERE cd_gv.MaID = @MaID";

            var teacherCourses = @"SELECT kh.MaID, kh.Ten
                                    FROM Khoahoc kh
                                    --JOIN Khoahoc_DmTrangthai kh_tt ON kh.MaID = kh_tt.KhoahocID
                                    JOIN Khoahoc_Giangvien kh_gv ON kh.MaID = kh_gv.KhoahocID
                                    WHERE kh_gv.GiangvienID = '' AND kh.Khongsudung = 0";
            
            var teacherStudents = @"";
            var teacherDetail = new TeacherDetailDto();

            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                conn.OpenAsync();
                //Step 1: Load thong tin chi tiet cua giang vien dua theo id
                using (var cmd = new SqlCommand(teacherInfo, conn))
                {
                    cmd.Parameters.AddWithValue("@MaID", MaID);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            teacherDetail.teacherInfo.MaID = reader["MaID"].ToString();
                            teacherDetail.teacherInfo.Hoten = reader["Hoten"].ToString();
                            teacherDetail.teacherInfo.Gioitinh = reader["Gioitinh"].ToString();
                            teacherDetail.teacherInfo.Email = reader["Email"].ToString();
                            teacherDetail.teacherInfo.Diachi = reader["Diachi"].ToString();
                            teacherDetail.teacherInfo.Dienthoai = reader["Dienthoai"].ToString();
                        }
                    }
                }

                //Step 2: Load danh sach topic ma giang vien dam nhan
                using (var cmd = new SqlCommand(teacherTopics, conn))
                {
                    cmd.Parameters.AddWithValue("@MaID", MaID);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            teacherDetail.teacherTopics.Add(new TopicsDto
                            {
                                MaID = reader["MaID"].ToString(),
                                Ten = reader["Ten"].ToString()
                            });

                        }
                    }
                }

                //Step 3: Load danh sach khoa hoc

            }
            throw new NotImplementedException();
        }

        public async Task<List<TeacherGetAllDto>> TeacherGetAllAsync()
        {
            var query = @"SELECT gv.MaID, gv.Hoten, gv.Email, gv.Dienthoai
                        FROM dbo.Giangvien AS gv
                        WHERE gv.Khongsudung == 0";
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var list = new List<TeacherGetAllDto>();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.OpenAsync();
                using (var cmd = new SqlCommand(query, conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new TeacherGetAllDto
                            {
                                MaID = reader["MaID"].ToString(),
                                Hoten = reader["Hoten"].ToString(),
                                Email = reader["Email"].ToString(),
                                Dienthoai = reader["Dienthoai"].ToString()
                            });
                        }
                    }

                }
                if (list == null)
                {
                    return null;
                }
            }
            return list;
            throw new NotImplementedException();
        }
    }

}
