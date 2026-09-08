using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RethusSalud.Application.Services;
using RethusSalud.Domain.Enums;
using RethusSalud.Web.Models;

namespace RethusSalud.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly CatalogoService _catalogos;

    public HomeController(ILogger<HomeController> logger, CatalogoService catalogos)
    {
        _logger = logger;
        _catalogos = catalogos;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.ProfesionesRethus = await _catalogos.ObtenerProfesionesAsync(TipoTramite.ReTHUS);
        return View();
    }

    [Authorize]
    public IActionResult Panel()
    {
        return View();
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
