using HtmlToPdfMaster;
using System.Diagnostics;

public class ConverPdfService
{
    public async Task ConvertHtmlToPdf(string htmlContent, string outputPath)
    {
        try
        {
            var pdfBytes = HtmlConverter.FromHtmlString(htmlContent, 220, 335);
            File.WriteAllBytes(outputPath, pdfBytes);
        }
        catch (System.Exception ex)
        {
            EventLog.WriteEntry("Application", "Exepcion manejada", EventLogEntryType.Error);
            EventLog.WriteEntry(
                "Application",
                $"Excepción: {ex.Message}\nStack Trace: {ex.StackTrace}",
                EventLogEntryType.Error
            );
        }
    }
}
