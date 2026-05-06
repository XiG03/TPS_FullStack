using TPS_FullStack.Server.Entities;

namespace TPS_FullStack.Server;

public class RefreshToken
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public string refreshToken { get; set; }
    public DateTime ExpiryTime { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime CreatedAt { get; set; }
}
