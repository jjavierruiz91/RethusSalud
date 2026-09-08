using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RethusSalud.Application.Common;
using RethusSalud.Application.Services;
using RethusSalud.Domain.Constants;
using RethusSalud.Infrastructure.Identity;
using RethusSalud.Web.Models;
using RethusSalud.Web.Models.Account;

namespace RethusSalud.Web.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SolicitanteService _solicitanteService;

    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        SolicitanteService solicitanteService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _solicitanteService = solicitanteService;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var existente = await _userManager.FindByEmailAsync(model.CorreoElectronico);
        if (existente is not null)
        {
            ModelState.AddModelError(string.Empty, "Ya existe una cuenta registrada con este correo.");
            return View(model);
        }

        var user = new ApplicationUser
        {
            UserName = model.CorreoElectronico,
            Email = model.CorreoElectronico,
            NombreCompleto = model.Nombre
        };

        var resultado = await _userManager.CreateAsync(user, model.Password);
        if (!resultado.Succeeded)
        {
            foreach (var error in resultado.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        await _userManager.AddToRoleAsync(user, Roles.Ciudadano);

        try
        {
            await _solicitanteService.RegistrarAsync(user.Id, model.Nombre, model.CorreoElectronico, model.NumeroIdentificacion);
        }
        catch (AppValidationException ex)
        {
            await _userManager.DeleteAsync(user);
            foreach (var error in ex.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            return View(model);
        }
        catch
        {
            await _userManager.DeleteAsync(user);
            throw;
        }

        await _signInManager.SignInAsync(user, isPersistent: false);
        return RedirectToAction("Dashboard", "Tramite");
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: true);

        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is not null)
            {
                if (await _userManager.IsInRoleAsync(user, Roles.Ciudadano))
                {
                    return RedirectToAction("Dashboard", "Tramite");
                }

                if (await _userManager.IsInRoleAsync(user, Roles.SuperAdmin))
                {
                    return RedirectToAction("Usuarios", "Admin");
                }

                if (await _userManager.IsInRoleAsync(user, Roles.FuncionarioEtapa1)
                    || await _userManager.IsInRoleAsync(user, Roles.FuncionarioEtapa2)
                    || await _userManager.IsInRoleAsync(user, Roles.FuncionarioEtapa3)
                    || await _userManager.IsInRoleAsync(user, Roles.Inventario))
                {
                    return RedirectToAction("Bandeja", "Revision");
                }
            }

            return RedirectToAction("Index", "Home");
        }

        if (result.IsLockedOut)
        {
            ModelState.AddModelError(string.Empty, "Tu cuenta esta bloqueada temporalmente por intentos fallidos.");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Correo o contrasena incorrectos.");
        }

        return View(model);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarMiPassword(CambiarMiPasswordViewModel model, string? returnUrl)
    {
        var referer = Request.Headers.Referer.ToString();
        var volverA = !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
            ? returnUrl
            : !string.IsNullOrEmpty(referer) && Url.IsLocalUrl(referer)
                ? referer
                : Url.Action("Index", "Home")!;

        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Revisa los datos de la contrasena e intenta nuevamente.";
            return Redirect(volverA);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return Redirect(volverA);
        }

        var resultado = await _userManager.ChangePasswordAsync(user, model.PasswordActual, model.NuevaPassword);
        if (!resultado.Succeeded)
        {
            TempData["Error"] = string.Join(" ", resultado.Errors.Select(e => e.Description));
            return Redirect(volverA);
        }

        await _signInManager.RefreshSignInAsync(user);
        TempData["Mensaje"] = "Tu contrasena fue actualizada correctamente.";
        return Redirect(volverA);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}
