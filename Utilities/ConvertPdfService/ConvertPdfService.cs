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
            // Verifica si el archivo ya existe y lo elimina
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            // Crear un archivo temporal para el contenido HTML
            string tempHtmlPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".html");
            Console.WriteLine(tempHtmlPath);
            File.WriteAllText(tempHtmlPath, htmlContent); // Escribir el contenido en el archivo

            // Prepara los argumentos de línea de comando para wkhtmltopdf
            var processInfo = new ProcessStartInfo
            {
                FileName = @"C:\Program Files\wkhtmltopdf\bin\wkhtmltopdf.exe",
                Arguments = $"\"{tempHtmlPath}\" \"{outputPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // Inicia el proceso
            var process = Process.Start(processInfo);

            if (process != null)
            {
                // Espera a que el proceso termine
                await process.WaitForExitAsync();

                // Verifica si el proceso finalizó con éxito
                if (process.ExitCode != 0)
                {
                    string errorMessage = await process.StandardError.ReadToEndAsync();
                    EventLog.WriteEntry(
                        "Application",
                        $"Error al generar PDF:  {errorMessage}",
                        EventLogEntryType.Error
                    );
                }
            }
            else
            {
                EventLog.WriteEntry(
                    "Application",
                    "No se pudo iniciar el proceso de wkhtmltopdf.",
                    EventLogEntryType.Error
                );
                throw new Exception("No se pudo iniciar el proceso de wkhtmltopdf");
            }

            // Elimina el archivo temporal
            File.Delete(tempHtmlPath);
        }
        catch (Exception ex)
        {
            // Manejo de errores y registro
            EventLog.WriteEntry(
                "Application",
                "Excepción manejada en ConvertHtmlToPdf",
                EventLogEntryType.Error
            );
            EventLog.WriteEntry(
                "Application",
                $"Excepción: {ex.Message}\nStack Trace: {ex.StackTrace}",
                EventLogEntryType.Error
            );
            throw new Exception("Excepción manejada en ConvertHtmlToPdf");
        }
    }
}
