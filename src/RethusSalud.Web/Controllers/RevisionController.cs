using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RethusSalud.Application.Common;
using RethusSalud.Application.Dtos;
using RethusSalud.Application.Interfaces;
using RethusSalud.Application.Services;
using RethusSalud.Domain.Constants;
using RethusSalud.Domain.Enums;
using RethusSalud.Infrastructure.Identity;
using RethusSalud.Web.Models.Revision;

namespace RethusSalud.Web.Controllers;

[Authorize(Roles = $"{Roles.FuncionarioEtapa1},{Roles.FuncionarioEtapa2},{Roles.FuncionarioEtapa3},{Roles.Inventario}")]
public class RevisionController : Controller
{
    private readonly SolicitudService _solicitudes;
    private readonly DocumentoService _documentos;
    private readonly ICertificadoPdfService _certificados;
    private readonly IReporteExcelService _reportes;
    private readonly UserManager<ApplicationUser> _userManager;

    public RevisionController(
        SolicitudService solicitudes,
        DocumentoService documentos,
        ICertificadoPdfService certificados,
        IReporteExcelService reportes,
        UserManager<ApplicationUser> userManager)
    {
        _solicitudes = solicitudes;
        _documentos = documentos;
        _reportes = reportes;
        _certificados = certificados;
        _userManager = userManager;
    }

    private string UserId => _userManager.GetUserId(User)!;

    private async Task<EtapaSolicitud?> ObtenerEtapaDelUsuarioAsync()
    {
        var usuario = await _userManager.GetUserAsync(User);
        if (usuario is null)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(usuario);
        if (roles.Contains(Roles.FuncionarioEtapa1)) return EtapaSolicitud.Etapa1;
        if (roles.Contains(Roles.FuncionarioEtapa2)) return EtapaSolicitud.Etapa2;
        if (roles.Contains(Roles.FuncionarioEtapa3)) return EtapaSolicitud.Etapa3;
        if (roles.Contains(Roles.Inventario)) return EtapaSolicitud.Inventario;
        return null;
    }

    private async Task<bool> PuedeGestionarAsync(int solicitudId)
    {
        var etapaUsuario = await ObtenerEtapaDelUsuarioAsync();
        if (etapaUsuario is null)
        {
            return false;
        }

        var solicitud = await _solicitudes.ObtenerPorIdAsync(solicitudId);
        return solicitud is not null && solicitud.EtapaActual == etapaUsuario;
    }

    [HttpGet]
    public async Task<IActionResult> Bandeja(BandejaFiltroViewModel filtro)
    {
        var etapa = await ObtenerEtapaDelUsuarioAsync();
        if (etapa is null)
        {
            return Forbid();
        }

        var filtroDto = new BandejaFiltroDto(filtro.NumeroIdentificacion, filtro.TipoTramite, filtro.Estado, filtro.Desde, filtro.Hasta);
        var solicitudes = await _solicitudes.ObtenerBandejaAsync(etapa.Value, filtroDto);

        return View(new BandejaViewModel { Etapa = etapa.Value, Filtro = filtro, Solicitudes = solicitudes });
    }

    [HttpGet]
    public async Task<IActionResult> ExportarExcel(BandejaFiltroViewModel filtro)
    {
        var etapa = await ObtenerEtapaDelUsuarioAsync();
        if (etapa is null)
        {
            return Forbid();
        }

        var filtroDto = new BandejaFiltroDto(filtro.NumeroIdentificacion, filtro.TipoTramite, filtro.Estado, filtro.Desde, filtro.Hasta);
        var solicitudes = await _solicitudes.ObtenerBandejaAsync(etapa.Value, filtroDto);

        var excel = _reportes.GenerarReporteBandeja(solicitudes);
        return File(excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"bandeja-{etapa}.xlsx");
    }

    [HttpGet]
    public async Task<IActionResult> Detalle(int id)
    {
        var solicitud = await _solicitudes.ObtenerPorIdAsync(id);
        if (solicitud is null)
        {
            return NotFound();
        }

        return View(solicitud);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Aprobar(int id)
    {
        if (!await PuedeGestionarAsync(id))
        {
            return Forbid();
        }

        try
        {
            await _solicitudes.AprobarAsync(id, UserId);
            TempData["Mensaje"] = "Solicitud aprobada.";
        }
        catch (AppValidationException ex)
        {
            TempData["Error"] = string.Join(" ", ex.Errors);
        }

        return RedirectToAction(nameof(Bandeja));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Rechazar(int id, string motivo)
    {
        if (!await PuedeGestionarAsync(id))
        {
            return Forbid();
        }

        try
        {
            await _solicitudes.RechazarAsync(id, UserId, motivo);
            TempData["Mensaje"] = "Solicitud rechazada.";
            return RedirectToAction(nameof(Bandeja));
        }
        catch (AppValidationException ex)
        {
            TempData["Error"] = string.Join(" ", ex.Errors);
            return RedirectToAction(nameof(Detalle), new { id });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AgregarComentario(int id, string texto)
    {
        try
        {
            await _solicitudes.AgregarComentarioAsync(id, UserId, texto);
        }
        catch (AppValidationException ex)
        {
            TempData["Error"] = string.Join(" ", ex.Errors);
        }

        return RedirectToAction(nameof(Detalle), new { id });
    }

    [HttpGet]
    public async Task<IActionResult> DescargarArchivo(int archivoId)
    {
        var (archivo, contenido) = await _documentos.AbrirArchivoAsync(archivoId);
        return File(contenido, archivo.ContentType, archivo.NombreArchivo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AsignarConsecutivoAutomatico(int id)
    {
        if (!await PuedeGestionarAsync(id))
        {
            return Forbid();
        }

        try
        {
            await _solicitudes.AsignarConsecutivoAutomaticoAsync(id);
        }
        catch (AppValidationException ex)
        {
            TempData["Error"] = string.Join(" ", ex.Errors);
        }

        return RedirectToAction(nameof(Detalle), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AsignarConsecutivoManual(int id, string numero)
    {
        if (!await PuedeGestionarAsync(id))
        {
            return Forbid();
        }

        try
        {
            await _solicitudes.AsignarConsecutivoManualAsync(id, numero);
        }
        catch (AppValidationException ex)
        {
            TempData["Error"] = string.Join(" ", ex.Errors);
        }

        return RedirectToAction(nameof(Detalle), new { id });
    }

    [HttpGet]
    public async Task<IActionResult> DescargarCertificado(int id)
    {
        var solicitud = await _solicitudes.ObtenerPorIdAsync(id);
        if (solicitud is null || solicitud.Estado != EstadoSolicitud.Aprobado)
        {
            return NotFound();
        }

        var pdf = _certificados.Generar(solicitud);
        return File(pdf, "application/pdf", $"certificado-{solicitud.Consecutivo!.Numero}.pdf");
    }
}
