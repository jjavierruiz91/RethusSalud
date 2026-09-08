using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RethusSalud.Application.Services;
using RethusSalud.Web.Models;

namespace RethusSalud.Web.Controllers;

[EnableRateLimiting("consulta-publica")]
public class ConsultaController : Controller
{
    private readonly ConsultaPublicaService _consulta;

    public ConsultaController(ConsultaPublicaService consulta)
    {
        _consulta = consulta;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new ConsultaEstadoViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ConsultaEstadoViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var resultado = await _consulta.ConsultarAsync(model.NumeroIdentificacion);
        model.Resultado = resultado is null
            ? "No se encontro ninguna solicitud con esta identificacion."
            : $"El proceso de {resultado.NombreCompleto} se encuentra en estado: {resultado.Estado}";

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> VerificarFolio(string numero)
    {
        var resultado = await _consulta.VerificarFolioAsync(numero);
        return View(resultado);
    }
}
