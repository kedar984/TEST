namespace SqlHelps.Web.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }

    public class SmtpEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SmtpEmailService> _logger;

        public SmtpEmailService(IConfiguration configuration, ILogger<SmtpEmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            // In a real production environment, you would use a service like SendGrid, Mailgun, or a configured SMTP server.
            // For this implementation, we will log the email and provide the structure for SMTP.
            
            _logger.LogInformation("Sending email to {To} with subject {Subject}", to, subject);
            
            /* 
            // Real SMTP Implementation Example:
            using var client = new System.Net.Mail.SmtpClient(_configuration["Email:SmtpServer"])
            {
                Port = int.Parse(_configuration["Email:Port"]),
                Credentials = new System.Net.NetworkCredential(_configuration["Email:Username"], _configuration["Email:Password"]),
                EnableSsl = true,
            };

            var mailMessage = new System.Net.Mail.MailMessage
            {
                From = new System.Net.Mail.MailAddress(_configuration["Email:FromAddress"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(to);

            await client.SendMailAsync(mailMessage);
            */

            await Task.CompletedTask;
        }
    }
}
