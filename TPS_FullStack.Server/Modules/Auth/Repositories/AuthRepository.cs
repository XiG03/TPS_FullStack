using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using TPS_FullStack.Server.Entities;

namespace TPS_FullStack.Server.Modules.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthRepository(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<AppUser?> GetAccountAsync(string Username)
        {
            var user = await _userManager.FindByNameAsync(Username);
            return user;
            // if(user == null)
            // {
            //     return new ServiceDefault<AppUser>
            //     {
            //         Success = false,
            //         status = Status.NotFound
            //     };
            // }
            // if(!await _userManager.CheckPasswordAsync(user, password))
            // {
            //     return new ServiceDefault<AppUser>
            //     {
            //         Success = false,
            //         status = Status.Invalid
            //     };
            // }

            // if(await _userManager.CheckPasswordAsync(user, password))
            // {
            //     return new ServiceDefault<AppUser>
            //     {
            //         Success = true,
            //         Data = user
            //     };
            // }
            throw new NotImplementedException();
        }

        public async Task<AppUser?> GetByUserNameAsync(string? Username, string? Password)
        {
            var user = await _userManager.FindByNameAsync(Username);
            if (user == null)
            {
                return null;
            }
            var isValidPassword = await _userManager.CheckPasswordAsync(user, Password);
            if (user.Kichhoat == false & isValidPassword)
            {
                return null;
            }
            throw new NotImplementedException();
        }

        public async Task<string?> GetRoleByIdAsync(string? UserId)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryCheckTeacher = @"SELECT TOP 1 * FROM dbo.Giangvien WHERE MaID = @UserId";
            var queryCheckStudent = @"SELECT TOP 1 * FROM dbo.Hocvien WHERE MaID = @UserId";

            // kiem tra xem ID co trong giang vien khong
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                // kiem tra ID co trong giang vien hay khong?
                using (var cmd = new SqlCommand(queryCheckTeacher, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", UserId);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return "Giangvien";
                        }
                    }
                }

                using (var cmd = new SqlCommand(queryCheckStudent, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", UserId);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return "Hocvien";
                        }
                    }
                }

                return "Admin";
            }
            throw new NotImplementedException();
        }
    }

}

