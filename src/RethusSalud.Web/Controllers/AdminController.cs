using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RethusSalud.Domain.Constants;
using RethusSalud.Infrastructure.Identity;
using RethusSalud.Web.Models.Admin;

namespace RethusSalud.Web.Controllers;

[Authorize(Roles = Roles.SuperAdmin)]
public class AdminController : Controller
{
    private static readonly string[] TiposFotoPermitidos = { "image/jpeg", "image/png" };
    private const long TamanoMaximoFotoBytes = 5 * 1024 * 1024;

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _environment;

    public AdminController(UserManager<ApplicationUser> userManager, IWebHostEnvironment environment)
    {
        _userManager = userManager;
        _environment = environment;
    }

    [HttpGet]
    public async Task<IActionResult> Usuarios(string? nombre, string? rol)
    {
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
                FotoUrl = usuario.FotoUrl
            });
        }

        ViewBag.Nombre = nombre;
        ViewBag.Rol = rol;
        ViewBag.Roles = Roles.All.Where(r => r != Roles.Ciudadano).ToList();
        return View(resultado.OrderBy(u => u.Nombre).ToList());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(CrearUsuarioInternoViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Revisa los datos del nuevo usuario e intenta nuevamente.";
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(EditarUsuarioInternoViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Revisa los datos del usuario e intenta nuevamente.";
            return RedirectToAction(nameof(Usuarios));
        }

        var usuario = await _userManager.FindByIdAsync(model.Id);
        if (usuario is null)
        {
            return NotFound();
        }

        var existente = await _userManager.FindByEmailAsync(model.Email);
        if (existente is not null && existente.Id != usuario.Id)
        {
            TempData["Error"] = "Ya existe otra cuenta con este correo.";
            return RedirectToAction(nameof(Usuarios));
        }

        usuario.NombreCompleto = model.Nombre;
        usuario.Email = model.Email;
        usuario.UserName = model.Email;

        var rolesActuales = await _userManager.GetRolesAsync(usuario);
        var rolActual = rolesActuales.FirstOrDefault(r => r != Roles.Ciudadano);
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
                return RedirectToAction(nameof(Usuarios));
            }

            usuario.FotoUrl = await GuardarFotoAsync(usuario.Id, model.Foto);
        }

        var resultado = await _userManager.UpdateAsync(usuario);
        if (!resultado.Succeeded)
        {
            TempData["Error"] = string.Join(" ", resultado.Errors.Select(e => e.Description));
            return RedirectToAction(nameof(Usuarios));
        }

        TempData["Mensaje"] = "Usuario actualizado correctamente.";
        return RedirectToAction(nameof(Usuarios));
    }

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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActivo(string id)
    {
        var usuario = await _userManager.FindByIdAsync(id);
        if (usuario is null)
        {
            return NotFound();
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
