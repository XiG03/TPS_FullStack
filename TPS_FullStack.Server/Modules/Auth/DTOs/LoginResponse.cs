namespace TPS_FullStack.Server.Modules.Auth
{
    public class LoginResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? TempToken { get; set; }
    }

}

