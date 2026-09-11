using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RethusSalud.Application.Dtos;
using RethusSalud.Application.Services;
using RethusSalud.Domain.Constants;
using RethusSalud.Domain.Enums;
using RethusSalud.Infrastructure.Identity;
using RethusSalud.Web.Models.Dashboard;

namespace RethusSalud.Web.Controllers;

[Authorize(Roles = $"{Roles.SuperAdmin},{Roles.FuncionarioEtapa1},{Roles.FuncionarioEtapa2},{Roles.FuncionarioEtapa3},{Roles.Inventario}")]
public class DashboardController : Controller
{
    private readonly SolicitudService _solicitudes;
    private readonly CatalogoService _catalogos;
    private readonly UserManager<ApplicationUser> _userManager;

    public DashboardController(SolicitudService solicitudes, CatalogoService catalogos, UserManager<ApplicationUser> userManager)
    {
        _solicitudes = solicitudes;
        _catalogos = catalogos;
        _userManager = userManager;
    }

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

    [HttpGet]
    public async Task<IActionResult> Index(DashboardFiltroViewModel filtro)
    {
        var esSuperAdmin = User.IsInRole(Roles.SuperAdmin);
        EtapaSolicitud? etapaBloqueada = null;

        if (!esSuperAdmin)
        {
            etapaBloqueada = await ObtenerEtapaDelUsuarioAsync();
            filtro.Etapa = etapaBloqueada;
        }

        var resultado = await _solicitudes.ObtenerDashboardAsync(filtro.ToDto());
        var paises = await _catalogos.ObtenerPaisesAsync();
        var departamentos = await _catalogos.ObtenerTodosDepartamentosAsync();
        var municipios = filtro.DepartamentoId.HasValue
            ? await _catalogos.ObtenerMunicipiosAsync(filtro.DepartamentoId.Value)
            : new List<CatalogoItemDto>();

        return View(new DashboardViewModel
        {
            Filtro = filtro,
            EsSuperAdmin = esSuperAdmin,
            EtapaBloqueada = etapaBloqueada,
            Resultado = resultado,
            Paises = paises,
            Departamentos = departamentos,
            Municipios = municipios
        });
    }
}
