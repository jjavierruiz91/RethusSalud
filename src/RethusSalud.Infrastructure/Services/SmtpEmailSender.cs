using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using RethusSalud.Application.Interfaces;

namespace RethusSalud.Infrastructure.Services;

public class SmtpEmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SmtpEmailSender> _logger;

    public SmtpEmailSender(IConfiguration configuration, ILogger<SmtpEmailSender> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task EnviarAsync(string destinatario, string asunto, string cuerpoHtml)
    {
        var host = _configuration["Smtp:Host"];
        if (string.IsNullOrWhiteSpace(host))
        {
            _logger.LogInformation("SMTP no configurado; se omite el envio del correo '{Asunto}' a {Destinatario}.", asunto, destinatario);
            return;
        }

        try
        {
            var puerto = int.TryParse(_configuration["Smtp:Port"], out var p) ? p : 587;
            var usuario = _configuration["Smtp:Usuario"];
            var password = _configuration["Smtp:Password"];
            var remitente = _configuration["Smtp:Remitente"] ?? usuario ?? "no-reply@rethus.local";

            var mensaje = new MimeMessage();
            mensaje.From.Add(MailboxAddress.Parse(remitente));
            mensaje.To.Add(MailboxAddress.Parse(destinatario));
            mensaje.Subject = asunto;
            mensaje.Body = new TextPart("html") { Text = cuerpoHtml };

            using var client = new SmtpClient();
            await client.ConnectAsync(host, puerto, SecureSocketOptions.StartTls);
            if (!string.IsNullOrWhiteSpace(usuario))
            {
                await client.AuthenticateAsync(usuario, password ?? string.Empty);
            }

            await client.SendAsync(mensaje);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "No se pudo enviar el correo '{Asunto}' a {Destinatario}.", asunto, destinatario);
        }
    }
}
