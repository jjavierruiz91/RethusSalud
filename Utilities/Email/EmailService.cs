using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using rethus_backend.Utilities.Constants.Email.EmailDto;
using rethus_backend.Utilities.FileHelper;

namespace rethus_backend.Utilities.Email.EmailService
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<bool> SendEmail(SendEmailDto payload)
        {
            try
            {
                var credentialEmail = _config.GetSection("Credenciales:Correo").Value;
                var credentialPassword = _config.GetSection("Credenciales:Password").Value;
                var configurationServerSmtp = _config.GetSection("ConfigurationEmail:Smtp").Value;
                var configurationServerAddress = _config
                    .GetSection("ConfigurationEmail:EmailAddress")
                    .Value;

                string body = await FileHelper.FileHelper.ReadFileContentAsync(payload.BodyPath);

                SmtpClient client = new SmtpClient(configurationServerSmtp);
                client.Port = 587;
                client.Credentials = new NetworkCredential(credentialEmail, credentialPassword);
                client.EnableSsl = true;

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(configurationServerAddress);
                foreach (string recipient in payload.To)
                {
                    mailMessage.To.Add(recipient);
                }
                mailMessage.Subject = payload.Subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = payload.IsBodyHtml;

                client.Send(mailMessage);
                Console.WriteLine("Correo enviado exitosamente.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar el correo: " + ex);
                Console.WriteLine("Error al enviar el correo: " + ex.Message);
                return true;
            }
        }
    }
}
