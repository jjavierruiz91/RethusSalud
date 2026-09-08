using RethusSalud.Domain.Entities;

namespace RethusSalud.Application.Interfaces;

public interface IReporteExcelService
{
    byte[] GenerarReporteBandeja(IEnumerable<Solicitud> solicitudes);
}
