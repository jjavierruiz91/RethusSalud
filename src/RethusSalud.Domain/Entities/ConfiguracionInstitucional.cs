using RethusSalud.Domain.Common;

namespace RethusSalud.Domain.Entities;

public class ConfiguracionInstitucional : Entity
{
    public string Nombre { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public string? FirmaUrl { get; set; }
}
