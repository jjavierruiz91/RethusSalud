using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RethusSalud.Application.Services;
using RethusSalud.Domain.Enums;
using RethusSalud.Infrastructure.Identity;
using RethusSalud.Web.Models;

namespace RethusSalud.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly CatalogoService _catalogos;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ILogger<HomeController> logger, CatalogoService catalogos, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _catalogos = catalogos;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.ProfesionesRethus = await _catalogos.ObtenerProfesionesAsync(TipoTramite.Rethus);
        return View();
    }

    [Authorize]
    public async Task<IActionResult> Panel()
    {
        var usuario = await _userManager.GetUserAsync(User);
        if (usuario is null)
        {
            return Challenge();
        }

        return View(usuario);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
