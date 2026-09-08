using RethusSalud.Domain.Common;

namespace RethusSalud.Domain.Entities;

public class Departamento : Entity
{
    public string Nombre { get; set; } = string.Empty;

    public int PaisId { get; set; }
    public Pais Pais { get; set; } = null!;

    public ICollection<Municipio> Municipios { get; set; } = new List<Municipio>();
}
