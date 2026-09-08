namespace RethusSalud.Application.Interfaces;

public interface IEmailSender
{
    Task EnviarAsync(string destinatario, string asunto, string cuerpoHtml);
}
