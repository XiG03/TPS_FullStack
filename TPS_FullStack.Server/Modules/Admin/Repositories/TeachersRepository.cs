

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

            //Step 2: Them giang vien moi -- ADO.NET 

            var queryTeacher = @"INSERT INTO dbo.Giangvien(MaID, UserId, Hoten, Ngaysinh, Gioitinh, Email, Diachi, Dienthoai,CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, Khongsudung)
                                VALUES (@MaID, @UserId, @Hoten, @Ngaysinh, @Gioitinh, @Email, @Diachi, @Dienthoai,SYSDATETIME(), '', SYSDATETIME(), '', 0)";

            var queryTeacherTopic = @"INSERT INTO dbo.Chuyende_Giangvien(MaID, ChuyendeID, GiangvienID)
                                    VALUES(@MaID, @ChuyendeID, @GiangvienID)";

            var connectionString = _configuration.GetConnectionString("DefaultConnection");


            using (var conn = new SqlConnection(connectionString))
            {
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

                await conn.OpenAsync();
                var transaction = conn.BeginTransaction();
                try
                {
                    using (var cmd = new SqlCommand(queryTeacher, conn, transaction))
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
                            using (var cmd = new SqlCommand(queryTeacherTopic, conn, transaction))
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
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    var teacher = await _userManager.FindByIdAsync(createDto.MaID);
                    await _userManager.DeleteAsync(teacher);
                    await transaction.RollbackAsync();
                    _logger.LogError(ex.Message);
                    return false;
                }
            }
            return true;
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
                using (var conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaID", MaID);

                        if (await cmd.ExecuteNonQueryAsync() <= 0)
                        {
                            return false;
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

        public async Task<TeacherDetailDto> TeacherDetailAsync(string MaID)
        {
            var teacherInfo = @"SELECT MaID, HoTen, Gioitinh, Email, Diachi, Dienthoai	
                                FROM Giangvien
                                WHERE MaID = @MaID AND Khongsudung = 0";

            var teacherTopics = @"SELECT cd.MaID, cd.Ten
                                FROM Chuyende cd 
                                JOIN Chuyende_Giangvien cd_gv ON cd.MaID = cd_gv.ChuyendeID
                                WHERE cd_gv.GiangvienID = @MaID";

            var teacherCourses = @"SELECT kh.MaID, kh.Ten
                                    FROM Khoahoc kh
                                    --JOIN Khoahoc_DmTrangthai kh_tt ON kh.MaID = kh_tt.KhoahocID
                                    JOIN Khoahoc_Giangvien kh_gv ON kh.MaID = kh_gv.KhoahocID
                                    WHERE kh_gv.GiangvienID = @GiangvienID AND kh.Khongsudung = 0";

            var teacherStudents = @"SELECT hv.MaID AS HocvienID, hv.Hoten AS HocvienTen
                                    FROM Hocvien hv
                                    JOIN Khoahoc_Hocvien kh_hv ON hv.MaID = kh_hv.HocvienID
                                    JOIN Khoahoc kh ON kh_hv.KhoahocID = kh.MaID
                                    JOIN Khoahoc_Giangvien kh_gv ON kh.MaID = kh_gv.KhoahocID
                                    WHERE kh_gv.GiangvienID = @GiangvienID";
            var teacherDetail = new TeacherDetailDto
            {
                teacherInfo = new TeacherInfoDto(),
                teacherTopics = new List<TopicsDto>(),
                teacherCourses = new List<CoursesDto>(),
                teacherStudents = new List<StudentsDto>()

            };

            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var transaction = conn.BeginTransaction();
                try
                {
                    //Step 1: Load thong tin chi tiet cua giang vien dua theo id
                    using (var cmd = new SqlCommand(teacherInfo, conn, transaction))
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
                    using (var cmd = new SqlCommand(teacherTopics, conn, transaction))
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

                    // Step 3: Load danh sach khoa hoc ma giang vien dam nhan
                    using (var cmd = new SqlCommand(teacherCourses, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@GiangvienID", MaID);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                teacherDetail.teacherCourses.Add(new CoursesDto
                                {
                                    MaID = reader["MaID"].ToString(),
                                    Ten = reader["Ten"].ToString()
                                });
                            }
                        }
                    }

                    // Step 4: Load danh sach hoc vien dang hoc voi giang vien 
                    using (var cmd = new SqlCommand(teacherStudents, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@GiangvienID", MaID);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                teacherDetail.teacherStudents.Add(new StudentsDto
                                {
                                    MaID = reader["HocvienID"].ToString(),
                                    Ten = reader["HocvienTen"].ToString()
                                });
                            }
                        }
                    }
                    await transaction.CommitAsync();
                    return teacherDetail;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex.Message);
                    return null;
                }
            }
            throw new NotImplementedException();
        }

        public async Task<List<TeacherGetAllDto>> TeacherGetAllAsync()
        {
            var query = @"SELECT gv.MaID, gv.Hoten, gv.Email, gv.Dienthoai
                        FROM dbo.Giangvien AS gv
                        WHERE gv.Khongsudung = 0";
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var list = new List<TeacherGetAllDto>();
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
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

        public async Task<bool> UpdateTeacherAsync(TeacherUpdateDto updateDto) // hien tai dang chi thay doi chuyen de cho giang vien, ve sau can them thi vao day de them
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            var queryDeleteTeacherTopic = @"DELETE 
                                            FROM dbo.Chuyende_Giangvien
                                            WHERE GiangvienID = @GiangvienID";

            var queryInsertTeacherTopic = @"INSERT INTO dbo.Chuyende_Giangvien (MaID, GiangvienID, ChuyendeID)
                                            VALUES (@MaID, @GiangvienID, @ChuyendeID)";

            var queryUpdateTeacher = @"UPDATE dbo.Giangvien
                                        SET
                                        	UpdatedAt = SYSDATETIME(),
                                        	UpdatedBy = @GiangvienID
                                        WHERE MaID = @GiangvienID";


            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var transaction = conn.BeginTransaction();
                try
                {
                    #region // Delete teacher topic when have TeacherId
                    using (var cmd = new SqlCommand(queryDeleteTeacherTopic, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@GiangvienID", updateDto.MaID);

                        await cmd.ExecuteNonQueryAsync();
                    }
                    #endregion
                    #region // Insert teacher topic
                    foreach (var teachertopic in updateDto.teacherTopics)
                    {
                        using (var cmd = new SqlCommand(queryInsertTeacherTopic, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@MaID", teachertopic.MaID == null ? Guid.NewGuid().ToString()
                                                                                            : teachertopic.MaID);
                            cmd.Parameters.AddWithValue("@GiangvienID", updateDto.MaID);
                            cmd.Parameters.AddWithValue("@ChuyendeID", teachertopic.ChuyendeID);

                            if (await cmd.ExecuteNonQueryAsync() < 0)
                            {
                                await transaction.RollbackAsync();
                                return false;
                            }
                        }
                    }
                    #endregion
                    #region // Update field updatedat in teacher table
                    using (var cmd = new SqlCommand(queryUpdateTeacher, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@GiangvienID", updateDto.MaID);
                        if (await cmd.ExecuteNonQueryAsync() < 0)
                        {
                            await transaction.RollbackAsync();
                            return false;
                        }
                    }
                    #endregion
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex.Message);
                }
            }
            return true;
            throw new NotImplementedException();
        }
    }

}
