using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace TPS_FullStack.Server.Helpers
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        public EmailService(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public async Task<string> RenderAsync(string? templateName, Dictionary<string, string>? values)
        {
            var templatePath = Path.Combine(
     _env.ContentRootPath, "Helpers", "EmailTemplates", $"{templateName}.html");

            var html = await File.ReadAllTextAsync(templatePath);
            foreach (var item in values)
            {
                html = html.Replace(
                 $"{{{{{item.Key}}}}}",
                 item.Value);
            }

            var layoutPath = Path.Combine( _env.ContentRootPath, "Helpers", "EmailTemplates", "Layout.html");

            var layout =
                await File.ReadAllTextAsync(layoutPath);

            return layout.Replace(
                "{{Content}}",
                html);

        }

        public async Task SendEmailAsync(string? toEmail, string? subject, string? message)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_config["EmailSettings:SenderName"], _config["EmailSettings:SenderEmail"]));
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = subject;


                email.Body = new TextPart("html")
                {
                    Text = message
                };

                using var smtp = new SmtpClient();

                await smtp.ConnectAsync(_config["EmailSettings:MailServer"], int.Parse(_config["EmailSettings:MailPort"]), MailKit.Security.SecureSocketOptions.StartTls);


                await smtp.AuthenticateAsync(_config["EmailSettings:SenderEmail"], _config["EmailSettings:Password"]);


                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {

            }


        }
    }

}

