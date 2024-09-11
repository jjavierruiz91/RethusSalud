using DinkToPdf;
using DinkToPdf.Contracts;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

public class ConvertPdfService
{
    private readonly IConverter _converter;

    public ConvertPdfService()
    {
        // Obtén la ruta absoluta del directorio de salida
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string dllPath = Path.Combine(basePath, "lib", "wkhtmltox", "bin", "libwkhtmltox.dll");

        // Cargar la DLL manualmente
        LoadUnmanagedLibrary(dllPath);
        _converter = new SynchronizedConverter(new PdfTools());
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr LoadLibrary(string lpFileName);

    private void LoadUnmanagedLibrary(string path)
    {
        IntPtr handle = LoadLibrary(path);
        if (handle == IntPtr.Zero)
        {
            int errorCode = Marshal.GetLastWin32Error();
            EventLog.WriteEntry(
                "Application",
                $"No se pudo cargar la biblioteca '{path}'. Código de error: {errorCode}",
                EventLogEntryType.Error
            );
        }
    }

    public async Task ConvertHtmlToPdf(string htmlContent, string outputPath)
    {
        try
        {
            var document = new HtmlToPdfDocument()
            {
                GlobalSettings =
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Out = outputPath // Ruta de salida
                },
                Objects =
                {
                    new ObjectSettings()
                    {
                        HtmlContent = htmlContent,
                        WebSettings = { DefaultEncoding = "utf-8" }
                    }
                }
            };

            _converter.Convert(document);
            await Task.CompletedTask;
        }
        catch (System.Exception ex)
        {
            EventLog.WriteEntry("Application", "Excepción manejada", EventLogEntryType.Error);
            EventLog.WriteEntry(
                "Application",
                $"Excepción: {ex.Message}\nStack Trace: {ex.StackTrace}",
                EventLogEntryType.Error
            );
        }
    }
}
