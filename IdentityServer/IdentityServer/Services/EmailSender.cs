using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace IdentityServer.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;

        public EmailSender(IConfiguration configuration)
        {
            _smtpServer = configuration["Smtp:Server"];
            _smtpPort = int.Parse(configuration["Smtp:Port"]);
            _smtpUser = configuration["Smtp:User"];
            _smtpPass = configuration["Smtp:Pass"];
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("Arooj", _smtpUser));
            mimeMessage.To.Add(new MailboxAddress(email, email));
            mimeMessage.Subject = subject;
            mimeMessage.Body = new TextPart("html")
            {
                Text = message
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpServer, _smtpPort, true);
            await client.AuthenticateAsync(_smtpUser, _smtpPass);
            await client.SendAsync(mimeMessage);
            await client.DisconnectAsync(true);
        }
    }
}
