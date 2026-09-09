using Microsoft.AspNetCore.Identity;
using RethusSalud.Application.Dtos;
using RethusSalud.Application.Services;
using RethusSalud.Domain.Entities;
using RethusSalud.Domain.Enums;
using RethusSalud.Infrastructure.Identity;

namespace RethusSalud.Web.Services;

public class FirmantesService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ConfiguracionInstitucionalService _configuracion;

    public FirmantesService(UserManager<ApplicationUser> userManager, ConfiguracionInstitucionalService configuracion)
    {
        _userManager = userManager;
        _configuracion = configuracion;
    }

    public async Task<FirmantesDocumentoDto> ResolverAsync(Solicitud solicitud)
    {
        var historial = solicitud.Historial.OrderBy(h => h.Fecha).ToList();

        var proyectoId = historial.FirstOrDefault(h => h.EtapaResultante == EtapaSolicitud.Etapa2)?.UsuarioId;
        var aproboId = historial.FirstOrDefault(h => h.EtapaResultante == EtapaSolicitud.Etapa3)?.UsuarioId;
        var revisoId = historial.FirstOrDefault(h =>
            h.EtapaResultante == EtapaSolicitud.Inventario && h.EstadoResultante == EstadoSolicitud.EnProceso)?.UsuarioId;
        var generoId = historial.LastOrDefault(h =>
            h.EtapaResultante == EtapaSolicitud.Inventario && h.EstadoResultante == EstadoSolicitud.Aprobado)?.UsuarioId;

        var jefe = await _configuracion.ObtenerAsync();

        return new FirmantesDocumentoDto(
            await ResolverFirmanteAsync(proyectoId),
            await ResolverFirmanteAsync(aproboId),
            await ResolverFirmanteAsync(revisoId),
            await ResolverFirmanteAsync(generoId),
            jefe is null ? null : new FirmanteDto(jefe.Nombre, jefe.Cargo, jefe.FirmaUrl));
    }

    private async Task<FirmanteDto?> ResolverFirmanteAsync(string? usuarioId)
    {
        if (string.IsNullOrEmpty(usuarioId))
        {
            return null;
        }

        var usuario = await _userManager.FindByIdAsync(usuarioId);
        return usuario is null ? null : new FirmanteDto(usuario.NombreCompleto, usuario.Cargo, usuario.FirmaUrl);
    }
}
