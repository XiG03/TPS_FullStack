namespace TPS_FullStack.Server.Modules.Auth
{
    public class LoginDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public bool StayedSignedin { get; set; }
    }

}

