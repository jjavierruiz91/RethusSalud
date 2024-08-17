using HtmlToPdfMaster;

public class ConverPdfService
{
    public static async Task ConvertHtmlToPdf(string htmlContent, string outputPath)
    {
        try
        {
            var pdfBytes = HtmlConverter.FromHtmlString(htmlContent, 220, 335);
            File.WriteAllBytes(outputPath, pdfBytes);
        }
        catch (System.Exception)
        {
            Console.WriteLine("Exepcion manejada!");
        }
    }
}
