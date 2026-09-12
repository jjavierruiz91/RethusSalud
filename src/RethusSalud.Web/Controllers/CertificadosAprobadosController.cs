using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RethusSalud.Application.Dtos;
using RethusSalud.Application.Interfaces;
using RethusSalud.Application.Services;
using RethusSalud.Domain.Constants;
using RethusSalud.Domain.Entities;
using RethusSalud.Domain.Enums;
using RethusSalud.Web.Models.CertificadosAprobados;
using RethusSalud.Web.Services;

namespace RethusSalud.Web.Controllers;

[Authorize(Roles = $"{Roles.SuperAdmin},{Roles.FuncionarioEtapa1},{Roles.Inventario}")]
public class CertificadosAprobadosController : Controller
{
    private const int TamanoPagina = 15;

    private readonly SolicitudService _solicitudes;
    private readonly ICertificadoPdfService _certificados;
    private readonly FirmantesService _firmantes;

    public CertificadosAprobadosController(
        SolicitudService solicitudes,
        ICertificadoPdfService certificados,
        FirmantesService firmantes)
    {
        _solicitudes = solicitudes;
        _certificados = certificados;
        _firmantes = firmantes;
    }

    [HttpGet]
    public async Task<IActionResult> Index(AprobadasFiltroViewModel filtro)
    {
        var filtroDto = new BandejaFiltroDto(filtro.NumeroIdentificacion, filtro.TipoTramite, EstadoSolicitud.Aprobado, filtro.Desde, filtro.Hasta);
        var pagina = Math.Max(filtro.Pagina, 1);
        var resultado = await _solicitudes.ObtenerSeguimientoPaginadoAsync(filtroDto, pagina, TamanoPagina);

        return View(new AprobadasViewModel { Filtro = filtro, Pagina = resultado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DescargarSeleccionados(int[] ids)
    {
        if (ids is null || ids.Length == 0)
        {
            TempData["Error"] = "Selecciona al menos una solicitud aprobada para descargar.";
            return RedirectToAction(nameof(Index));
        }

        var items = new List<(Solicitud Solicitud, FirmantesDocumentoDto Firmantes)>();
        foreach (var id in ids.Distinct())
        {
            var solicitud = await _solicitudes.ObtenerPorIdAsync(id);
            if (solicitud is null || solicitud.Estado != EstadoSolicitud.Aprobado)
            {
                TempData["Error"] = $"La solicitud {id} no está aprobada o no existe.";
                return RedirectToAction(nameof(Index));
            }

            items.Add((solicitud, await _firmantes.ResolverAsync(solicitud)));
        }

        var pdf = _certificados.GenerarLote(items);
        return File(pdf, "application/pdf", $"certificados-aprobados-{DateTime.Now:yyyyMMdd-HHmm}.pdf");
    }
}
