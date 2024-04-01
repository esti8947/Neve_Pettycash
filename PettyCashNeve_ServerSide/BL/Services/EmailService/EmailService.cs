using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace BL.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly System.Net.Mail.SmtpClient _smtpClient;
        private readonly string _senderEmail;

        private readonly string _smtpUsername;
        private readonly string _smtpPassword;

        public EmailService(IConfiguration configuration)
        {
            _smtpClient = new System.Net.Mail.SmtpClient();

            // Read sender email configuration
            _senderEmail = configuration["EmailSettings:SenderEmail"];
            if (!IsValidEmail(_senderEmail))
            {
                throw new ArgumentException("Invalid sender email address configuration.");
            }

            // Read SMTP server configuration
            string smtpServer = configuration["EmailSettings:SmtpServer"];
            if (string.IsNullOrEmpty(smtpServer))
            {
                throw new ArgumentException("SMTP server configuration is missing or empty.");
            }

            // Read SMTP port configuration
            string smtpPortStr = configuration["EmailSettings:Port"];
            if (!int.TryParse(smtpPortStr, out int smtpPort))
            {
                throw new ArgumentException("Invalid SMTP port configuration.");
            }

            // Read SMTP credentials
            string smtpUsername = configuration["EmailSettings:Username"];
            string smtpPassword = configuration["EmailSettings:Password"];
            _smtpUsername = configuration["EmailSettings:Username"];
            _smtpPassword = configuration["EmailSettings:Password"];

            // Configure SMTP client
            _smtpClient.Host = smtpServer;
            _smtpClient.Port = smtpPort;
            _smtpClient.UseDefaultCredentials = false;
            _smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
            _smtpClient.EnableSsl = true;
        }

        public void Send(string to, string subject, string html, string from = null)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? _senderEmail));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_smtpClient.Host, _smtpClient.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_smtpUsername, _smtpPassword);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(_senderEmail);
                mailMessage.To.Add(email);
                mailMessage.Subject = subject;
                mailMessage.Body = message;
                mailMessage.IsBodyHtml = true;

                await _smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately

                throw new ApplicationException("Failed to send email. See inner exception for details.", ex);
            }
        }

        // Helper method to validate email address format
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
