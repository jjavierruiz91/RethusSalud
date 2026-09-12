using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RethusSalud.Web.Models;

namespace RethusSalud.Web.Controllers;

[Route("Error")]
public class ErrorController : Controller
{
    [Route("{codigo:int}")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Index(int codigo)
    {
        var (titulo, mensaje) = codigo switch
        {
            429 => ("Estamos protegiendo el sistema",
                "Detectamos demasiadas solicitudes seguidas desde tu conexión. Es una medida de seguridad para que el servicio siga funcionando bien para todos los ciudadanos. Espera un momento y vuelve a intentarlo."),
            404 => ("No encontramos esta página",
                "Es posible que el enlace esté mal escrito o que la página ya no exista."),
            403 => ("No tienes permiso para ver esta página",
                "Si crees que esto es un error, comunícate con el administrador del sistema."),
            _ => ("Ocurrió un problema",
                "No pudimos procesar tu solicitud. Intenta de nuevo en unos minutos.")
        };

        var volverUrl = HttpContext.Features.Get<IStatusCodeReExecuteFeature>()?.OriginalPath;
        var retryAfter = HttpContext.Items.TryGetValue("RetryAfterSeconds", out var valor) ? (int?)valor : null;

        return View(new ErrorPageViewModel
        {
            Codigo = codigo,
            Titulo = titulo,
            Mensaje = mensaje,
            RetryAfterSegundos = retryAfter,
            VolverUrl = string.IsNullOrEmpty(volverUrl) ? "/" : volverUrl
        });
    }
}
