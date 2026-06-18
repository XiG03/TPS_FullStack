namespace TPS_FullStack.Server.Helpers
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string? toEmail, string? subject, string? message);

        public Task<string> RenderAsync(string? templateName, Dictionary<string, string>? values);
    }

}

