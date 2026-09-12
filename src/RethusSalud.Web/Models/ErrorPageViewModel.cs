namespace RethusSalud.Web.Models;

public class ErrorPageViewModel
{
    public int Codigo { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
    public int? RetryAfterSegundos { get; set; }
    public string VolverUrl { get; set; } = "/";
}
