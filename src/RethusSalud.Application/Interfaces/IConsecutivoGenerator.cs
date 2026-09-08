using RethusSalud.Domain.Enums;

namespace RethusSalud.Application.Interfaces;

public interface IConsecutivoGenerator
{
    Task<string> GenerarSiguienteAsync(TipoTramite tipoTramite);
}
