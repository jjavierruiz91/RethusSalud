using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RethusSalud.Application.Common;
using RethusSalud.Application.Dtos;
using RethusSalud.Application.Interfaces;
using RethusSalud.Application.Services;
using RethusSalud.Domain.Constants;
using RethusSalud.Domain.Entities;
using RethusSalud.Domain.Enums;
using RethusSalud.Infrastructure.Identity;
using RethusSalud.Web.Models.Tramite;

namespace RethusSalud.Web.Controllers;

[Authorize(Roles = Roles.Ciudadano)]
public class TramiteController : Controller
{
    private readonly SolicitanteService _solicitantes;
    private readonly SolicitudService _solicitudes;
    private readonly CatalogoService _catalogos;
    private readonly DocumentoService _documentos;
    private readonly ICertificadoPdfService _certificados;
    private readonly UserManager<ApplicationUser> _userManager;

    public TramiteController(
        SolicitanteService solicitantes,
        SolicitudService solicitudes,
        CatalogoService catalogos,
        DocumentoService documentos,
        ICertificadoPdfService certificados,
        UserManager<ApplicationUser> userManager)
    {
        _documentos = documentos;
        _certificados = certificados;
        _solicitantes = solicitantes;
        _solicitudes = solicitudes;
        _catalogos = catalogos;
        _userManager = userManager;
    }

    private string UserId => _userManager.GetUserId(User)!;

    public async Task<IActionResult> Dashboard()
    {
        var solicitante = await _solicitantes.ObtenerPorUsuarioAsync(UserId);
        var solicitud = await _solicitudes.ObtenerUltimaAsync(solicitante.Id);

        ViewBag.Solicitante = solicitante;
        return View(solicitud);
    }

    [HttpGet]
    public async Task<IActionResult> DescargarCertificado(int solicitudId)
    {
        var solicitante = await _solicitantes.ObtenerPorUsuarioAsync(UserId);
        var solicitud = await _solicitudes.ObtenerPorIdAsync(solicitudId);

        if (solicitud is null || solicitud.SolicitanteId != solicitante.Id || solicitud.Estado != EstadoSolicitud.Aprobado)
        {
            return NotFound();
        }

        var pdf = _certificados.Generar(solicitud);
        return File(pdf, "application/pdf", $"certificado-{solicitud.Consecutivo!.Numero}.pdf");
    }

    [HttpGet]
    public async Task<IActionResult> Terminos()
    {
        var solicitante = await _solicitantes.ObtenerPorUsuarioAsync(UserId);
        if (solicitante.FechaAceptacionTerminos is not null)
        {
            return RedirectToAction(nameof(SeleccionarTramite));
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Terminos(bool acepto)
    {
        if (!acepto)
        {
            ModelState.AddModelError(string.Empty, "Debes aceptar los terminos y condiciones para continuar.");
            return View();
        }

        await _solicitantes.AceptarTerminosAsync(UserId);
        return RedirectToAction(nameof(SeleccionarTramite));
    }

    [HttpGet]
    public async Task<IActionResult> SeleccionarTramite(TipoTramite tipoTramite = TipoTramite.ReTHUS)
    {
        var solicitante = await _solicitantes.ObtenerPorUsuarioAsync(UserId);
        if (solicitante.FechaAceptacionTerminos is null)
        {
            return RedirectToAction(nameof(Terminos));
        }

        var profesiones = await _catalogos.ObtenerProfesionesAsync(tipoTramite);

        var borrador = await _solicitudes.ObtenerBorradorActivoAsync(solicitante.Id);
        ViewBag.SolicitudId = borrador?.Id;
        ViewBag.PasoMaximo = borrador is null ? 1 : borrador.DatosAcademicos is not null ? 4 : 3;

        return View(new SeleccionarTramiteViewModel { TipoTramite = tipoTramite, Profesiones = profesiones });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SeleccionarTramite(int profesionId)
    {
        var solicitante = await _solicitantes.ObtenerPorUsuarioAsync(UserId);
        var solicitud = await _solicitudes.IniciarOContinuarBorradorAsync(solicitante, profesionId);
        return RedirectToAction(nameof(DatosPersonales), new { solicitudId = solicitud.Id });
    }

    [HttpGet]
    public async Task<IActionResult> DatosPersonales(int solicitudId)
    {
        var solicitante = await _solicitantes.ObtenerPorUsuarioAsync(UserId);

        var vm = new DatosPersonalesViewModel
        {
            SolicitudId = solicitudId,
            TipoIdentificacion = solicitante.TipoIdentificacion,
            NumeroIdentificacion = solicitante.NumeroIdentificacion,
            LugarExpedicion = solicitante.LugarExpedicion,
            Genero = solicitante.Genero,
            Nombres = solicitante.Nombres,
            Apellidos = solicitante.Apellidos,
            PaisNacimientoId = solicitante.PaisNacimientoId,
            DepartamentoNacimientoId = solicitante.DepartamentoNacimientoId,
            MunicipioNacimientoId = solicitante.MunicipioNacimientoId,
            FechaNacimiento = solicitante.FechaNacimiento,
            PaisResidenciaId = solicitante.PaisResidenciaId,
            DepartamentoResidenciaId = solicitante.DepartamentoResidenciaId,
            MunicipioResidenciaId = solicitante.MunicipioResidenciaId,
            DireccionDomicilio = solicitante.DireccionDomicilio,
            TelefonoFijo = solicitante.TelefonoFijo,
            Celular = solicitante.Celular,
            CorreoElectronico = solicitante.CorreoElectronico,
            GrupoEtnico = solicitante.GrupoEtnico
        };

        var solicitud = await _solicitudes.ObtenerPorIdAsync(solicitudId);
        ViewBag.PasoMaximo = solicitud?.DatosAcademicos is not null ? 4 : 3;

        await CargarPaisesAsync();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DatosPersonales(DatosPersonalesViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            var solicitudEnCurso = await _solicitudes.ObtenerPorIdAsync(vm.SolicitudId);
            ViewBag.PasoMaximo = solicitudEnCurso?.DatosAcademicos is not null ? 4 : 3;
            await CargarPaisesAsync();
            return View(vm);
        }

        var dto = new DatosPersonalesDto
        {
            TipoIdentificacion = vm.TipoIdentificacion,
            NumeroIdentificacion = vm.NumeroIdentificacion,
            LugarExpedicion = vm.LugarExpedicion,
            Genero = vm.Genero,
            Nombres = vm.Nombres,
            Apellidos = vm.Apellidos,
            PaisNacimientoId = vm.PaisNacimientoId,
            DepartamentoNacimientoId = vm.DepartamentoNacimientoId,
            MunicipioNacimientoId = vm.MunicipioNacimientoId,
            FechaNacimiento = vm.FechaNacimiento,
            PaisResidenciaId = vm.PaisResidenciaId,
            DepartamentoResidenciaId = vm.DepartamentoResidenciaId,
            MunicipioResidenciaId = vm.MunicipioResidenciaId,
            DireccionDomicilio = vm.DireccionDomicilio,
            TelefonoFijo = vm.TelefonoFijo,
            Celular = vm.Celular,
            CorreoElectronico = vm.CorreoElectronico,
            GrupoEtnico = vm.GrupoEtnico
        };

        try
        {
            await _solicitantes.ActualizarDatosPersonalesAsync(UserId, dto);
        }
        catch (AppValidationException ex)
        {
            foreach (var error in ex.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            var solicitudEnCurso = await _solicitudes.ObtenerPorIdAsync(vm.SolicitudId);
            ViewBag.PasoMaximo = solicitudEnCurso?.DatosAcademicos is not null ? 4 : 3;
            await CargarPaisesAsync();
            return View(vm);
        }

        return RedirectToAction(nameof(DatosAcademicos), new { solicitudId = vm.SolicitudId });
    }

    [HttpGet]
    public async Task<IActionResult> DatosAcademicos(int solicitudId)
    {
        var solicitud = await _solicitudes.ObtenerBorradorActivoAsync(
            (await _solicitantes.ObtenerPorUsuarioAsync(UserId)).Id);

        var vm = new DatosAcademicosViewModel { SolicitudId = solicitudId, PaisInstitucionId = 1 };

        if (solicitud?.Id == solicitudId && solicitud.DatosAcademicos is not null)
        {
            var d = solicitud.DatosAcademicos;
            vm.OrigenTitulo = d.OrigenTitulo;
            vm.TipoInstitucion = d.TipoInstitucion;
            vm.TipoPrograma = d.TipoPrograma;
            vm.PaisInstitucionId = d.PaisInstitucionId;
            vm.DepartamentoInstitucionId = d.DepartamentoInstitucionId;
            vm.MunicipioInstitucionId = d.MunicipioInstitucionId;
            vm.NombreInstitucion = d.NombreInstitucion;
            vm.NombrePrograma = d.NombrePrograma;
            vm.FechaGrado = d.FechaGrado;
            vm.NumeroConvalidacion = d.NumeroConvalidacion;
            vm.FechaConvalidacion = d.FechaConvalidacion;
            vm.TituloEquivalente = d.TituloEquivalente;
        }

        ViewBag.PasoMaximo = solicitud?.Id == solicitudId && solicitud.DatosAcademicos is not null ? 4 : 3;

        await CargarPaisesAsync();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DatosAcademicos(DatosAcademicosViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            var solicitudEnCurso = await _solicitudes.ObtenerPorIdAsync(vm.SolicitudId);
            ViewBag.PasoMaximo = solicitudEnCurso?.DatosAcademicos is not null ? 4 : 3;
            await CargarPaisesAsync();
            return View(vm);
        }

        var dto = new DatosAcademicosDto
        {
            OrigenTitulo = vm.OrigenTitulo,
            TipoInstitucion = vm.TipoInstitucion,
            TipoPrograma = vm.TipoPrograma,
            PaisInstitucionId = vm.PaisInstitucionId,
            DepartamentoInstitucionId = vm.DepartamentoInstitucionId,
            MunicipioInstitucionId = vm.MunicipioInstitucionId,
            NombreInstitucion = vm.NombreInstitucion,
            NombrePrograma = vm.NombrePrograma,
            FechaGrado = vm.FechaGrado,
            NumeroConvalidacion = vm.NumeroConvalidacion,
            FechaConvalidacion = vm.FechaConvalidacion,
            TituloEquivalente = vm.TituloEquivalente
        };

        try
        {
            await _solicitudes.GuardarDatosAcademicosAsync(vm.SolicitudId, dto);
        }
        catch (AppValidationException ex)
        {
            foreach (var error in ex.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            var solicitudEnCurso = await _solicitudes.ObtenerPorIdAsync(vm.SolicitudId);
            ViewBag.PasoMaximo = solicitudEnCurso?.DatosAcademicos is not null ? 4 : 3;
            await CargarPaisesAsync();
            return View(vm);
        }

        return RedirectToAction(nameof(Documentos), new { solicitudId = vm.SolicitudId });
    }

    [HttpGet]
    public async Task<IActionResult> Documentos(int solicitudId)
    {
        var solicitante = await _solicitantes.ObtenerPorUsuarioAsync(UserId);
        var solicitud = await _solicitudes.ObtenerBorradorActivoAsync(solicitante.Id);
        if (solicitud is null || solicitud.Id != solicitudId)
        {
            return NotFound();
        }

        ViewBag.DocumentosRequeridos = DocumentoService.DocumentosRequeridos(solicitud.Profesion);
        return View(solicitud);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CargarDocumento(int solicitudId, TipoDocumentoAdjunto tipoDocumento, IFormFile archivo)
    {
        if (archivo is null || archivo.Length == 0)
        {
            TempData["Error"] = "Selecciona un archivo antes de subirlo.";
            return RedirectToAction(nameof(Documentos), new { solicitudId });
        }

        try
        {
            await using var stream = archivo.OpenReadStream();
            await _documentos.CargarDocumentoAsync(solicitudId, tipoDocumento, archivo.FileName, archivo.ContentType, archivo.Length, stream);
        }
        catch (AppValidationException ex)
        {
            TempData["Error"] = string.Join(" ", ex.Errors);
        }

        return RedirectToAction(nameof(Documentos), new { solicitudId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Enviar(int solicitudId)
    {
        try
        {
            await _documentos.RadicarAsync(solicitudId, UserId);
        }
        catch (AppValidationException ex)
        {
            TempData["Error"] = string.Join(" ", ex.Errors);
            return RedirectToAction(nameof(Documentos), new { solicitudId });
        }

        TempData["Mensaje"] = "Tu solicitud fue radicada exitosamente y sera revisada por los funcionarios.";
        return RedirectToAction(nameof(Dashboard));
    }

    [HttpGet]
    public async Task<IActionResult> Departamentos(int paisId)
    {
        return Json(await _catalogos.ObtenerDepartamentosAsync(paisId));
    }

    [HttpGet]
    public async Task<IActionResult> Municipios(int departamentoId)
    {
        return Json(await _catalogos.ObtenerMunicipiosAsync(departamentoId));
    }

    private async Task CargarPaisesAsync()
    {
        ViewBag.Paises = new SelectList(await _catalogos.ObtenerPaisesAsync(), nameof(CatalogoItemDto.Id), nameof(CatalogoItemDto.Nombre));
    }
}
