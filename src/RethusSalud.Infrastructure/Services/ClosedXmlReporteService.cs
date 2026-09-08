using ClosedXML.Excel;
using RethusSalud.Application.Interfaces;
using RethusSalud.Domain.Entities;

namespace RethusSalud.Infrastructure.Services;

public class ClosedXmlReporteService : IReporteExcelService
{
    public byte[] GenerarReporteBandeja(IEnumerable<Solicitud> solicitudes)
    {
        using var workbook = new XLWorkbook();
        var hoja = workbook.Worksheets.Add("Bandeja");

        var encabezados = new[] { "Identificacion", "Nombre", "Tipo de tramite", "Estado", "Etapa actual", "Consecutivo", "Fecha de creacion" };
        for (var i = 0; i < encabezados.Length; i++)
        {
            hoja.Cell(1, i + 1).Value = encabezados[i];
        }
        hoja.Row(1).Style.Font.Bold = true;

        var fila = 2;
        foreach (var solicitud in solicitudes)
        {
            hoja.Cell(fila, 1).Value = solicitud.Solicitante.NumeroIdentificacion;
            hoja.Cell(fila, 2).Value = $"{solicitud.Solicitante.Nombres} {solicitud.Solicitante.Apellidos}";
            hoja.Cell(fila, 3).Value = solicitud.TipoTramite.ToString();
            hoja.Cell(fila, 4).Value = solicitud.Estado.ToString();
            hoja.Cell(fila, 5).Value = solicitud.EtapaActual?.ToString() ?? string.Empty;
            hoja.Cell(fila, 6).Value = solicitud.Consecutivo?.Numero ?? string.Empty;
            hoja.Cell(fila, 7).Value = solicitud.FechaCreacion.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
            fila++;
        }

        hoja.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
