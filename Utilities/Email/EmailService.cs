using System.Net;
using System.Net.Mail;
using rethus_backend.Models.Dto.UserPublic;
using rethus_backend.Utilities.Constants.Email.EmailDto;

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
                mailMessage.Body = payload.TemplateEmail;
                mailMessage.IsBodyHtml = payload.IsBodyHtml;

                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar el correo: " + ex);
                Console.WriteLine("Error al enviar el correo: " + ex.Message);
                return true;
            }
        }

        public async Task<string> ConfigurationTemplateRestorePassword(
            TemplateConfigurationDto payload
        )
        {
            var domailUrl = _config.GetSection("DomainWebUrl").Value;
            var urlViewRestorePassword = _config.GetSection("UrlViewNerPassword").Value;

            var linkUrl = domailUrl + urlViewRestorePassword;

            var linkUrlAddToken = linkUrl + "?token=" + payload.Token;

            string body = await FileHelper.FileHelper.ReadFileContentAsync(
                payload.TemplatePashEmail
            );

            var placeHoldersTemplateDto = new PlaceHoldersTemplate
            {
                LinkRestorePassword = linkUrlAddToken
            };
            var newBodyWithPlaceHolders = await ReplacePlacesHoldersRestorePassword(
                body,
                placeHoldersTemplateDto
            );

            return newBodyWithPlaceHolders;
        }

        public async static Task<string> ReplacePlacesHoldersRestorePassword(
            string bodyTemplate,
            PlaceHoldersTemplate placeHolders
        )
        {
            bodyTemplate = bodyTemplate.Replace(
                "${LINK_RESTORE_PASSWORD}",
                placeHolders.LinkRestorePassword
            );

            return bodyTemplate;
        }
    }
}
