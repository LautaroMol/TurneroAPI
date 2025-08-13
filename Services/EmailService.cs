using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using TurneroAPI.Settings;

namespace TurneroAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly GmailSettings _settings;

        public EmailService(IOptions<GmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var client = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort)
            {
                Credentials = new NetworkCredential(_settings.SenderEmail, _settings.Password),
                EnableSsl = _settings.EnableSsl
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_settings.SenderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
        }
    }
}
