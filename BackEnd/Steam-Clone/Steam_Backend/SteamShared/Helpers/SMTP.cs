using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;

namespace SteamShared.Helpers
{
    public class SMTP(string host, string from, int port, string user, string password)
    {
        public async Task Send(string to, string subject, string body)
        {
            using var smtpClient = new SmtpClient
            {
                Host = host,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(user, password),
                Port = port,
                EnableSsl = true,
                Timeout = 30000
            };

            using var message = new MailMessage(from, to, subject, body)
            {
                IsBodyHtml = true
            };

            await smtpClient.SendMailAsync(message);
        }
    }

    public class SmtpSettings
    {
        [Required]
        public string Host { get; set; } = null!;

        [Required]
        public string From { get; set; } = null!;

        public int Port { get; set; } = 587;

        [Required]
        public string User { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
