using RethusSalud.Application.Common;
using RethusSalud.Domain.Entities;

namespace RethusSalud.Web.Models.CertificadosAprobados;

public class AprobadasViewModel
{
    public AprobadasFiltroViewModel Filtro { get; set; } = new();
    public PagedResult<Solicitud> Pagina { get; set; } = new();
}
