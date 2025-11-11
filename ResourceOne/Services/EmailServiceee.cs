using Humanizer;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using ResourceOne.Models;


namespace ResourceOne.Services
{
    public class EmailServiceee : IEmailServiceee
    {
        private readonly SmtpSettingss _settings;
        public EmailServiceee(IOptions<SmtpSettingss> settings)
        {
            _settings = settings.Value;
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            email.Body = new TextPart("html") { Text = body };

            using (var smtp = new SmtpClient())
            {
                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await smtp.ConnectAsync(_settings.Server, _settings.Port, MailKit.Security.SecureSocketOptions.Auto);
                await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
        }
    }
}
