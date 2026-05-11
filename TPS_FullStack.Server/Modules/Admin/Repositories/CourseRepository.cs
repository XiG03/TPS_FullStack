using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using Microsoft.OpenApi.Validations.Rules;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class CourseRepository : ICourseRepository
    {
        private readonly IConfiguration _configuration;
        public CourseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<CourseInfo>> CourseGetAllAsync()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryCourseGetAll = @"SELECT kh.MaID, kh.Ten, kh.Mota
                                    FROM dbo.Khoahoc kh";

            var courses = new List<CourseInfo>();
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(queryCourseGetAll, conn))
                {
                    using(var reader = await cmd.ExecuteReaderAsync())
                    {
                        while(await reader.ReadAsync())
                        {
                            courses.Add(new CourseInfo
                            {
                                MaID = reader["MaID"].ToString(),
                                Ten = reader["Ten"].ToString(),
                                Mota = reader["Mota"].ToString()
                            });
                        }
                    }
                }
            }
            return courses;
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteCourseAsync(string MaId)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryCourseDelete = @"UPDATE dbo.Khoahoc
                                        SET
                                        	DeletedAt = SYSDATETIME(),
                                        	DeletedBy = ''
                                        WHERE MaID = @KhoahocID";


            using(var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(queryCourseDelete, conn))
                {
                    cmd.Parameters.AddWithValue("@KhoahocID", MaId);

                    if(await cmd.ExecuteNonQueryAsync() < 0)
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

