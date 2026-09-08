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
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
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
                Activo = usuario.Activo
            });
        }

        ViewBag.Nombre = nombre;
        ViewBag.Rol = rol;
        ViewBag.Roles = Roles.All.Where(r => r != Roles.Ciudadano).ToList();
        return View(resultado.OrderBy(u => u.Nombre).ToList());
    }

    [HttpGet]
    public IActionResult Crear()
    {
        ViewBag.Roles = Roles.All.Where(r => r != Roles.Ciudadano).ToList();
        return View(new CrearUsuarioInternoViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(CrearUsuarioInternoViewModel model)
    {
        ViewBag.Roles = Roles.All.Where(r => r != Roles.Ciudadano).ToList();

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (await _userManager.FindByEmailAsync(model.Email) is not null)
        {
            ModelState.AddModelError(string.Empty, "Ya existe una cuenta con este correo.");
            return View(model);
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
            foreach (var error in resultado.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        await _userManager.AddToRoleAsync(usuario, model.Rol);
        TempData["Mensaje"] = "Funcionario creado correctamente.";
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
}
