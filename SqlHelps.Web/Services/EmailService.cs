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
            _logger.LogInformation("Attempting to send email to {To} with subject {Subject}", to, subject);
            
            try
            {
                using var client = new System.Net.Mail.SmtpClient(_configuration["Email:SmtpServer"])
                {
                    Port = int.Parse(_configuration["Email:Port"] ?? "587"),
                    Credentials = new System.Net.NetworkCredential(_configuration["Email:Username"], _configuration["Email:Password"]),
                    EnableSsl = true,
                };

                var mailMessage = new System.Net.Mail.MailMessage
                {
                    From = new System.Net.Mail.MailAddress(_configuration["Email:FromAddress"] ?? "noreply@sqlhelps.com", "SQLHelps Website"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(to);

                await client.SendMailAsync(mailMessage);
                _logger.LogInformation("Email sent successfully to {To}", to);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {To}. Ensure SMTP settings are configured in appsettings.json.", to);
                // We rethrow to ensure the Controller handles the failure and shows an error to the user
                throw;
            }
        }
    }
}
