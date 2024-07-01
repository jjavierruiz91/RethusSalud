using PuppeteerSharp;

public class ConverPdfService
{
    public static async Task ConvertHtmlToPdf(string htmlContent, string outputPath)
    {
        await new BrowserFetcher().DownloadAsync();
        var launchOptions = new LaunchOptions { Headless = true };
        using var browser = await Puppeteer.LaunchAsync(launchOptions);
        using var page = await browser.NewPageAsync();

        // Convertir HTML a PDF
        await page.SetContentAsync(htmlContent);
        byte[] pdfBytes = await page.PdfDataAsync();

        // Guardar el PDF en el disco
        File.WriteAllBytes(outputPath, pdfBytes);
    }
}
