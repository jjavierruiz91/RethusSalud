using RethusSalud.Domain.Common;
using RethusSalud.Domain.Enums;

namespace RethusSalud.Domain.Entities;

public class Profesion : Entity
{
    public string Nombre { get; set; } = string.Empty;
    public TipoTramite TipoTramite { get; set; }
    public NivelFormacion NivelFormacion { get; set; }
}
