using RethusSalud.Application.Dtos;
using RethusSalud.Domain.Enums;

namespace RethusSalud.Web.Models.Tramite;

public class SeleccionarTramiteViewModel
{
    public TipoTramite TipoTramite { get; set; }
    public List<ProfesionDto> Profesiones { get; set; } = new();
}
