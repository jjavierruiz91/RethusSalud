using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RethusSalud.Application.Interfaces;
using RethusSalud.Application.Services;
using RethusSalud.Web.Models;
using RethusSalud.Web.Services;

namespace RethusSalud.Web.Controllers;

[EnableRateLimiting("consulta-publica")]
public class ConsultaController : Controller
{
    private readonly ConsultaPublicaService _consulta;
    private readonly ICertificadoPdfService _certificados;
    private readonly FirmantesService _firmantes;

    public ConsultaController(ConsultaPublicaService consulta, ICertificadoPdfService certificados, FirmantesService firmantes)
    {
        _consulta = consulta;
        _certificados = certificados;
        _firmantes = firmantes;
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

        model.Consultado = true;
        model.Resultado = await _consulta.ConsultarAsync(model.NumeroIdentificacion);

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> DescargarCertificado(string numeroIdentificacion)
    {
        var solicitud = await _consulta.ObtenerSolicitudAprobadaAsync(numeroIdentificacion);
        if (solicitud is null)
        {
            return NotFound();
        }

        var firmantes = await _firmantes.ResolverAsync(solicitud);
        var pdf = _certificados.Generar(solicitud, firmantes);
        return File(pdf, "application/pdf", $"certificado-{solicitud.Consecutivo!.Numero}.pdf");
    }

    [HttpGet]
    public async Task<IActionResult> VerificarFolio(string numero)
    {
        var resultado = await _consulta.VerificarFolioAsync(numero);
        return View(resultado);
    }
}
