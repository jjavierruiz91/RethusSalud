using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RethusSalud.Application.Dtos;
using RethusSalud.Application.Interfaces;
using RethusSalud.Application.Services;
using RethusSalud.Domain.Constants;
using RethusSalud.Infrastructure.Identity;
using RethusSalud.Web.Helpers;
using RethusSalud.Web.Models.Admin;

namespace RethusSalud.Web.Controllers;

[Authorize]
public class AdminController : Controller
{
    private static readonly string[] TiposFotoPermitidos = { "image/jpeg", "image/png" };
    private const long TamanoMaximoFotoBytes = 5 * 1024 * 1024;

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _environment;
    private readonly SolicitudService _solicitudes;
    private readonly DocumentoService _documentos;
    private readonly IReporteExcelService _reportes;
    private readonly ConfiguracionInstitucionalService _configuracionInstitucional;

    public AdminController(
        UserManager<ApplicationUser> userManager,
        IWebHostEnvironment environment,
        SolicitudService solicitudes,
        DocumentoService documentos,
        IReporteExcelService reportes,
        ConfiguracionInstitucionalService configuracionInstitucional)
    {
        _userManager = userManager;
        _environment = environment;
        _solicitudes = solicitudes;
        _documentos = documentos;
        _reportes = reportes;
        _configuracionInstitucional = configuracionInstitucional;
    }

    private const int TamanoPagina = 15;

    [Authorize(Roles = Roles.SuperAdmin)]
    [HttpGet]
    public async Task<IActionResult> SeguimientoBandeja(SeguimientoFiltroViewModel filtro)
    {
        var filtroDto = new BandejaFiltroDto(filtro.NumeroIdentificacion, filtro.TipoTramite, filtro.Estado, filtro.Desde, filtro.Hasta, filtro.Etapa);
        var pagina = Math.Max(filtro.Pagina, 1);
        var resultado = await _solicitudes.ObtenerSeguimientoConEstadisticasAsync(filtroDto, pagina, TamanoPagina);

        return View(new SeguimientoBandejaViewModel
        {
            Filtro = filtro,
            Pagina = resultado.Pagina,
            TotalEnProceso = resultado.TotalEnProceso,
            TotalAprobadas = resultado.TotalAprobadas,
            TotalRechazadas = resultado.TotalRechazadas
        });
    }

    [Authorize(Roles = Roles.SuperAdmin)]
    [HttpGet]
    public async Task<IActionResult> ExportarSeguimientoExcel(SeguimientoFiltroViewModel filtro)
    {
        var filtroDto = new BandejaFiltroDto(filtro.NumeroIdentificacion, filtro.TipoTramite, filtro.Estado, filtro.Desde, filtro.Hasta, filtro.Etapa);
        var solicitudes = await _solicitudes.ObtenerSeguimientoAsync(filtroDto);

        var excel = _reportes.GenerarReporteBandeja(solicitudes);
        return File(excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "seguimiento-revisiones.xlsx");
    }

    [Authorize(Roles = Roles.SuperAdmin)]
    [HttpGet]
    public async Task<IActionResult> SeguimientoDetalle(int id)
    {
        var solicitud = await _solicitudes.ObtenerPorIdAsync(id);
        if (solicitud is null)
        {
            return NotFound();
        }

        return View(solicitud);
    }

    [Authorize(Roles = Roles.SuperAdmin)]
    [HttpGet]
    public async Task<IActionResult> DescargarArchivoSeguimiento(int archivoId)
    {
        var (archivo, contenido) = await _documentos.AbrirArchivoAsync(archivoId);
        return File(contenido, archivo.ContentType, archivo.NombreArchivo);
    }

    [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.FuncionarioEtapa1}")]
    [HttpGet]
    public async Task<IActionResult> Usuarios(string? nombre, string? rol)
    {
        var esSuperAdmin = User.IsInRole(Roles.SuperAdmin);
        var usuarios = _userManager.Users.ToList();
        var resultado = new List<UsuarioInternoViewModel>();

        foreach (var usuario in usuarios)
        {
            var roles = await _userManager.GetRolesAsync(usuario);
            var rolPrincipal = roles.FirstOrDefault(r => r != Roles.Ciudadano);
            if (rolPrincipal is null)
            {
                continue;
            }

            if (rolPrincipal == Roles.SuperAdmin && !esSuperAdmin)
            {
                continue;
            }

            if (!string.IsNullOrWhiteSpace(rol) && rolPrincipal != rol)
            {
                continue;
            }

            if (!string.IsNullOrWhiteSpace(nombre) &&
                !usuario.NombreCompleto.Contains(nombre, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            resultado.Add(new UsuarioInternoViewModel
            {
                Id = usuario.Id,
                Nombre = usuario.NombreCompleto,
                Email = usuario.Email ?? string.Empty,
                Rol = rolPrincipal,
                Activo = usuario.Activo,
                FotoUrl = usuario.FotoUrl,
                Cargo = usuario.Cargo,
                FirmaUrl = usuario.FirmaUrl
            });
        }

        ViewBag.Nombre = nombre;
        ViewBag.Rol = rol;
        ViewBag.Roles = Roles.All.Where(r => r != Roles.Ciudadano && (esSuperAdmin || r != Roles.SuperAdmin)).ToList();
        return View(resultado.OrderBy(u => u.Nombre).ToList());
    }

    [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.FuncionarioEtapa1}")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(CrearUsuarioInternoViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Revisa los datos del nuevo usuario e intenta nuevamente.";
            return RedirectToAction(nameof(Usuarios));
        }

        if (model.Rol == Roles.SuperAdmin && !User.IsInRole(Roles.SuperAdmin))
        {
            TempData["Error"] = "No tienes permisos para asignar el rol SuperAdmin.";
            return RedirectToAction(nameof(Usuarios));
        }

        if (await _userManager.FindByEmailAsync(model.Email) is not null)
        {
            TempData["Error"] = "Ya existe una cuenta con este correo.";
            return RedirectToAction(nameof(Usuarios));
        }

        var usuario = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            NombreCompleto = model.Nombre,
            EmailConfirmed = true
        };

        var resultado = await _userManager.CreateAsync(usuario, model.Password);
        if (!resultado.Succeeded)
        {
            TempData["Error"] = string.Join(" ", resultado.Errors.Select(e => e.Description));
            return RedirectToAction(nameof(Usuarios));
        }

        await _userManager.AddToRoleAsync(usuario, model.Rol);
        TempData["Mensaje"] = "Funcionario creado correctamente.";
        return RedirectToAction(nameof(Usuarios));
    }

    [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.FuncionarioEtapa1}")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(EditarUsuarioInternoViewModel model)
    {
        IActionResult VolverAOrigen() => model.Origen == nameof(ConfiguracionInstitucional)
            ? RedirectToAction(nameof(ConfiguracionInstitucional))
            : RedirectToAction(nameof(Usuarios));

        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Revisa los datos del usuario e intenta nuevamente.";
            return VolverAOrigen();
        }

        var usuario = await _userManager.FindByIdAsync(model.Id);
        if (usuario is null)
        {
            return NotFound();
        }

        var esSuperAdmin = User.IsInRole(Roles.SuperAdmin);
        var rolesActuales = await _userManager.GetRolesAsync(usuario);
        var rolActual = rolesActuales.FirstOrDefault(r => r != Roles.Ciudadano);

        if (!esSuperAdmin && (rolActual == Roles.SuperAdmin || model.Rol == Roles.SuperAdmin))
        {
            TempData["Error"] = "No tienes permisos para gestionar cuentas SuperAdmin.";
            return VolverAOrigen();
        }

        var existente = await _userManager.FindByEmailAsync(model.Email);
        if (existente is not null && existente.Id != usuario.Id)
        {
            TempData["Error"] = "Ya existe otra cuenta con este correo.";
            return VolverAOrigen();
        }

        usuario.NombreCompleto = model.Nombre;
        usuario.Email = model.Email;
        usuario.UserName = model.Email;
        usuario.Cargo = model.Cargo;

        if (rolActual != model.Rol)
        {
            if (rolActual is not null)
            {
                await _userManager.RemoveFromRoleAsync(usuario, rolActual);
            }

            await _userManager.AddToRoleAsync(usuario, model.Rol);
        }

        if (model.Foto is not null && model.Foto.Length > 0)
        {
            var error = ValidarFoto(model.Foto);
            if (error is not null)
            {
                TempData["Error"] = error;
                return VolverAOrigen();
            }

            usuario.FotoUrl = await GuardarFotoAsync(usuario.Id, model.Foto);
        }

        if (model.Firma is not null && model.Firma.Length > 0)
        {
            var error = ImagenHelper.Validar(model.Firma);
            if (error is not null)
            {
                TempData["Error"] = error;
                return VolverAOrigen();
            }

            usuario.FirmaUrl = await ImagenHelper.GuardarAsync(_environment, "firmas", usuario.Id, model.Firma);
        }

        var resultado = await _userManager.UpdateAsync(usuario);
        if (!resultado.Succeeded)
        {
            TempData["Error"] = string.Join(" ", resultado.Errors.Select(e => e.Description));
            return VolverAOrigen();
        }

        TempData["Mensaje"] = "Usuario actualizado correctamente.";
        return VolverAOrigen();
    }

    [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.FuncionarioEtapa1}")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarPassword(CambiarPasswordUsuarioViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Revisa la contrasena e intenta nuevamente.";
            return RedirectToAction(nameof(Usuarios));
        }

        var usuario = await _userManager.FindByIdAsync(model.Id);
        if (usuario is null)
        {
            return NotFound();
        }

        if (!User.IsInRole(Roles.SuperAdmin) && await _userManager.IsInRoleAsync(usuario, Roles.SuperAdmin))
        {
            TempData["Error"] = "No tienes permisos para gestionar cuentas SuperAdmin.";
            return RedirectToAction(nameof(Usuarios));
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);
        var resultado = await _userManager.ResetPasswordAsync(usuario, token, model.NuevaPassword);
        if (!resultado.Succeeded)
        {
            TempData["Error"] = string.Join(" ", resultado.Errors.Select(e => e.Description));
            return RedirectToAction(nameof(Usuarios));
        }

        TempData["Mensaje"] = "Contrasena actualizada correctamente.";
        return RedirectToAction(nameof(Usuarios));
    }

    [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.FuncionarioEtapa1}")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActivo(string id)
    {
        var usuario = await _userManager.FindByIdAsync(id);
        if (usuario is null)
        {
            return NotFound();
        }

        if (!User.IsInRole(Roles.SuperAdmin) && await _userManager.IsInRoleAsync(usuario, Roles.SuperAdmin))
        {
            TempData["Error"] = "No tienes permisos para gestionar cuentas SuperAdmin.";
            return RedirectToAction(nameof(Usuarios));
        }

        usuario.Activo = !usuario.Activo;

        if (!usuario.Activo)
        {
            await _userManager.SetLockoutEnabledAsync(usuario, true);
            await _userManager.SetLockoutEndDateAsync(usuario, DateTimeOffset.MaxValue);
        }
        else
        {
            await _userManager.SetLockoutEndDateAsync(usuario, null);
        }

        await _userManager.UpdateAsync(usuario);
        return RedirectToAction(nameof(Usuarios));
    }

    private static readonly string[] RolesFirmantes =
    {
        Roles.FuncionarioEtapa1, Roles.FuncionarioEtapa2, Roles.FuncionarioEtapa3, Roles.Inventario
    };

    [Authorize(Roles = Roles.SuperAdmin)]
    [HttpGet]
    public async Task<IActionResult> ConfiguracionInstitucional()
    {
        var configuracion = await _configuracionInstitucional.ObtenerAsync();

        var funcionarios = new List<UsuarioInternoViewModel>();
        foreach (var rol in RolesFirmantes)
        {
            var usuariosDelRol = await _userManager.GetUsersInRoleAsync(rol);
            funcionarios.AddRange(usuariosDelRol.Select(u => new UsuarioInternoViewModel
            {
                Id = u.Id,
                Nombre = u.NombreCompleto,
                Email = u.Email ?? string.Empty,
                Rol = rol,
                Activo = u.Activo,
                FotoUrl = u.FotoUrl,
                Cargo = u.Cargo,
                FirmaUrl = u.FirmaUrl
            }));
        }

        return View(new ConfiguracionInstitucionalViewModel
        {
            Nombre = configuracion?.Nombre ?? string.Empty,
            Cargo = configuracion?.Cargo ?? string.Empty,
            FirmaUrl = configuracion?.FirmaUrl,
            Funcionarios = funcionarios.OrderBy(f => f.Rol).ThenBy(f => f.Nombre).ToList()
        });
    }

    [Authorize(Roles = Roles.SuperAdmin)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfiguracionInstitucional(ConfiguracionInstitucionalViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Revisa el nombre y el cargo e intenta nuevamente.";
            return RedirectToAction(nameof(ConfiguracionInstitucional));
        }

        string? firmaUrl = null;
        if (model.Firma is not null && model.Firma.Length > 0)
        {
            var error = ImagenHelper.Validar(model.Firma);
            if (error is not null)
            {
                TempData["Error"] = error;
                return RedirectToAction(nameof(ConfiguracionInstitucional));
            }

            firmaUrl = await ImagenHelper.GuardarAsync(_environment, "firmas", "jefe-institucional", model.Firma);
        }

        await _configuracionInstitucional.GuardarAsync(model.Nombre, model.Cargo, firmaUrl);
        TempData["Mensaje"] = "Configuración institucional actualizada correctamente.";
        return RedirectToAction(nameof(ConfiguracionInstitucional));
    }

    private string? ValidarFoto(Microsoft.AspNetCore.Http.IFormFile foto)
    {
        if (!TiposFotoPermitidos.Contains(foto.ContentType))
        {
            return "La foto debe ser un archivo JPG o PNG.";
        }

        if (foto.Length > TamanoMaximoFotoBytes)
        {
            return "La foto debe pesar menos de 5 MB.";
        }

        return null;
    }

    private async Task<string> GuardarFotoAsync(string usuarioId, Microsoft.AspNetCore.Http.IFormFile foto)
    {
        var carpeta = Path.Combine(_environment.WebRootPath, "img", "usuarios");
        Directory.CreateDirectory(carpeta);

        var extension = foto.ContentType == "image/png" ? ".png" : ".jpg";
        var nombreArchivo = $"{usuarioId}{extension}";
        var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

        await using (var destino = System.IO.File.Create(rutaCompleta))
        {
            await foto.CopyToAsync(destino);
        }

        return $"/img/usuarios/{nombreArchivo}?v={DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
    }
}
