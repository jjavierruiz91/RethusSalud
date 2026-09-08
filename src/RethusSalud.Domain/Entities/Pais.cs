using RethusSalud.Domain.Common;

namespace RethusSalud.Domain.Entities;

public class Pais : Entity
{
    public string Nombre { get; set; } = string.Empty;

    public ICollection<Departamento> Departamentos { get; set; } = new List<Departamento>();
}
