namespace TPS_FullStack.Server.Modules.JWT
{
    public class refreshTokenModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string refreshToken { get; set; }
        public DateTime ExpiryTime { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}

