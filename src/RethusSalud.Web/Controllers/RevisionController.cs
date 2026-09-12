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
using RethusSalud.Web.Services;

namespace RethusSalud.Web.Controllers;

[Authorize(Roles = $"{Roles.FuncionarioEtapa1},{Roles.FuncionarioEtapa2},{Roles.FuncionarioEtapa3},{Roles.Inventario},{Roles.SuperAdmin}")]
public class RevisionController : Controller
{
    private readonly SolicitudService _solicitudes;
    private readonly DocumentoService _documentos;
    private readonly ICertificadoPdfService _certificados;
    private readonly IReporteExcelService _reportes;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly FirmantesService _firmantes;

    public RevisionController(
        SolicitudService solicitudes,
        DocumentoService documentos,
        ICertificadoPdfService certificados,
        IReporteExcelService reportes,
        UserManager<ApplicationUser> userManager,
        FirmantesService firmantes)
    {
        _solicitudes = solicitudes;
        _documentos = documentos;
        _reportes = reportes;
        _certificados = certificados;
        _userManager = userManager;
        _firmantes = firmantes;
    }

    private const int TamanoPagina = 15;

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
        var pagina = Math.Max(filtro.Pagina, 1);
        var resultado = await _solicitudes.ObtenerBandejaPaginadaAsync(etapa.Value, filtroDto, pagina, TamanoPagina);

        return View(new BandejaViewModel
        {
            Etapa = etapa.Value,
            Filtro = filtro,
            Pagina = resultado.Pagina,
            TotalEnProceso = resultado.TotalEnProceso,
            TotalAprobadas = resultado.TotalAprobadas,
            TotalEsperandoLargo = resultado.TotalEsperandoLargo
        });
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
    public async Task<IActionResult> Buscar(BuscarFiltroViewModel filtro)
    {
        var filtroDto = new BandejaFiltroDto(filtro.NumeroIdentificacion, filtro.TipoTramite, filtro.Estado, filtro.Desde, filtro.Hasta, filtro.Etapa);
        var pagina = Math.Max(filtro.Pagina, 1);
        var resultado = await _solicitudes.ObtenerSeguimientoPaginadoAsync(filtroDto, pagina, TamanoPagina);

        return View(new BuscarViewModel { Filtro = filtro, Pagina = resultado });
    }

    private static readonly HashSet<string> PasosDeRevision = new()
    {
        nameof(Detalle), nameof(DetalleAcademicos), nameof(DetalleDocumentos)
    };

    private async Task<IActionResult> VerPasoAsync(int id, string paso)
    {
        ViewBag.VolverA = paso;
        var solicitud = await _solicitudes.ObtenerPorIdAsync(id);
        if (solicitud is null)
        {
            return NotFound();
        }

        var etapaUsuario = await ObtenerEtapaDelUsuarioAsync();
        ViewBag.PuedeGestionar = solicitud.Estado == EstadoSolicitud.EnProceso && solicitud.EtapaActual == etapaUsuario;

        return View(solicitud);
    }

    private IActionResult RedirigirAPaso(string? volverA, int id)
    {
        var accion = volverA is not null && PasosDeRevision.Contains(volverA) ? volverA : nameof(Detalle);
        return RedirectToAction(accion, new { id });
    }

    [HttpGet]
    public Task<IActionResult> Detalle(int id) => VerPasoAsync(id, nameof(Detalle));

    [HttpGet]
    public Task<IActionResult> DetalleAcademicos(int id) => VerPasoAsync(id, nameof(DetalleAcademicos));

    [HttpGet]
    public Task<IActionResult> DetalleDocumentos(int id) => VerPasoAsync(id, nameof(DetalleDocumentos));

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Aprobar(int id, string? mensaje)
    {
        if (!await PuedeGestionarAsync(id))
        {
            return Forbid();
        }

        try
        {
            await _solicitudes.AprobarAsync(id, UserId, mensaje);
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
    public async Task<IActionResult> Rechazar(int id, string motivo, string? volverA)
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
            return RedirigirAPaso(volverA, id);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AgregarComentario(int id, string texto, string? volverA)
    {
        try
        {
            var usuario = await _userManager.GetUserAsync(User);
            var etapa = await ObtenerEtapaDelUsuarioAsync();
            var autorNombre = string.IsNullOrWhiteSpace(usuario?.NombreCompleto) ? "Funcionario" : usuario.NombreCompleto;
            await _solicitudes.AgregarComentarioAsync(id, UserId, autorNombre, usuario?.FotoUrl, etapa, texto);
        }
        catch (AppValidationException ex)
        {
            TempData["Error"] = string.Join(" ", ex.Errors);
        }

        return RedirigirAPaso(volverA, id);
    }

    [HttpGet]
    public async Task<IActionResult> DescargarArchivo(int archivoId)
    {
        var (archivo, contenido) = await _documentos.AbrirArchivoAsync(archivoId);
        return File(contenido, archivo.ContentType, archivo.NombreArchivo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AsignarConsecutivoManual(int id, string numero, DateOnly fecha, string? volverA)
    {
        if (!await PuedeGestionarAsync(id))
        {
            return Forbid();
        }

        try
        {
            await _solicitudes.AsignarConsecutivoManualAsync(id, numero, fecha, UserId);
            TempData["Mensaje"] = "Consecutivo asignado y solicitud aprobada.";
        }
        catch (AppValidationException ex)
        {
            TempData["Error"] = string.Join(" ", ex.Errors);
        }

        return RedirigirAPaso(volverA, id);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AsignarConsecutivoRango(int[] ids, string consecutivoInicial, string consecutivoFinal, DateOnly fecha)
    {
        var etapa = await ObtenerEtapaDelUsuarioAsync();
        if (etapa != EtapaSolicitud.Inventario)
        {
            return Forbid();
        }

        try
        {
            await _solicitudes.AsignarConsecutivoRangoAsync(ids, consecutivoInicial, consecutivoFinal, fecha, UserId);
            TempData["Mensaje"] = $"Se asigno consecutivo y se aprobaron {ids.Length} solicitud(es).";
        }
        catch (AppValidationException ex)
        {
            TempData["Error"] = string.Join(" ", ex.Errors);
        }

        return RedirectToAction(nameof(Bandeja));
    }

    [HttpGet]
    public async Task<IActionResult> DescargarCertificado(int id)
    {
        var solicitud = await _solicitudes.ObtenerPorIdAsync(id);
        if (solicitud is null || solicitud.Estado != EstadoSolicitud.Aprobado)
        {
            return NotFound();
        }

        var firmantes = await _firmantes.ResolverAsync(solicitud);
        var pdf = _certificados.Generar(solicitud, firmantes);
        return File(pdf, "application/pdf", $"certificado-{solicitud.Consecutivo!.Numero}.pdf");
    }
}
