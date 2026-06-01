namespace TPS_FullStack.Server.Modules.JWT
{
    public interface IJwtRepository
    {
        public bool SaveRefreshToken(string UserId, string refreshToken, DateTime ExpiryTime);
        public Task<refreshTokenModel> GetByRefreshTokenAsync(string refreshToken);
    }
}


