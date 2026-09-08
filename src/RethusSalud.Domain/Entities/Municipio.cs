using RethusSalud.Domain.Common;

namespace RethusSalud.Domain.Entities;

public class Municipio : Entity
{
    public string Nombre { get; set; } = string.Empty;

    public int DepartamentoId { get; set; }
    public Departamento Departamento { get; set; } = null!;
}
