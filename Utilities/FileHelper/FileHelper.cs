using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using System.IO.Compression;

namespace rethus_backend.Utilities.FileHelper
{
    public class FileHelper
    {
        public static string newPath(IFormFile file, string basePath)
        {
            var creatingUniqueFilename = CreatingUniqueFilename(file);
            string result = $@"{basePath}//{creatingUniqueFilename}";

            return result;
        }

        public static string AddAsync(IFormFile file, string basePath)
        {
            var result = newPath(file, basePath);
            var sourcePath = CreateTempFile(file);

            try
            {
                CopyToTempFile(file, sourcePath);
                MoveTempFile(sourcePath, result);
            }
            catch (Exception exception)
            {
                return exception.Message;
            }

            return result;
        }

        public static string UpdateAsync(string sourcePath, IFormFile file, string basePath)
        {
            var result = newPath(file, basePath);

            try
            {
                if (sourcePath.Length > 0)
                {
                    using (var stream = new FileStream(result, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                File.Delete(sourcePath);
            }
            catch (Exception exception)
            {
                return exception.Message;
            }

            return result;
        }

        public static bool DeleteAsync(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception exception)
            {
                return false;
            }

            return true;
        }

        public static bool ValidatePath(string path)
        {
            if (System.IO.File.Exists(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ValidateDirectoryPath(string path)
        {
            if (System.IO.Directory.Exists(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CreateFolder(string sourcePath)
        {
            if (!ValidatePath(sourcePath))
            {
                Directory.CreateDirectory(sourcePath);
                return false;
            }

            return true;
        }

        public static string CreatingUniqueFilename(IFormFile file)
        {
            System.IO.FileInfo ff = new System.IO.FileInfo(file.FileName);
            string fileExtension = ff.Extension;

            var uniqueFilename =
                Guid.NewGuid().ToString("N")
                + "_"
                + DateTime.Now.Month
                + "_"
                + DateTime.Now.Day
                + "_"
                + DateTime.Now.Year
                + fileExtension;

            return uniqueFilename;
        }

        public static string CreateTempFile(IFormFile file)
        {
            return Path.GetTempFileName();
        }

        public static void CopyToTempFile(IFormFile file, string sourcePath)
        {
            if (file.Length > 0)
            {
                using (var stream = new FileStream(sourcePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
        }

        public static void MoveTempFile(string sourcePath, string result)
        {
            File.Move(sourcePath, result);
        }

        public static async Task<List<byte[]>> GetPdfFilesAsync(
            List<string> pdfPath,
            string _basePath
        )
        {
            var pdfFiles = new List<byte[]>();

            foreach (var file in pdfPath)
            {
                try
                {
                    var fullPath = Path.Combine(_basePath, file);
                    var pdfBytes = await File.ReadAllBytesAsync(fullPath);
                    pdfFiles.Add(pdfBytes);
                }
                catch (Exception ex)
                {
                    // Handle exception, e.g., log it
                    Console.WriteLine($"Failed to read PDF file {pdfPath}: {ex.Message}");
                }
            }

            return pdfFiles;
        }

        public static async Task<string> GetFileAsBase64Async(string file, string basePath)
        {
            try
            {
                var fullPath = Path.Combine(basePath, file);
                var pdfBytes = await File.ReadAllBytesAsync(fullPath);
                var base64String = Convert.ToBase64String(pdfBytes);
                return base64String;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to read file: {ex.Message}");
                return null;
            }
        }

        public static async Task<string> ReadFileContentAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found", filePath);
            }

            using (var reader = new StreamReader(filePath))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public static async Task<byte[]> GetPdfFileAsync(string _basePath)
        {
            byte[] pdfBytes;
            try
            {
                pdfBytes = await File.ReadAllBytesAsync(_basePath);
                return pdfBytes;
            }
            catch (Exception ex)
            {
                // Handle exception, e.g., log it
                Console.WriteLine($"Failed to read PDF file {_basePath}: {ex.Message}");
                return null;
            }
        }

        public static async Task<string> FileAsBase64Async(byte[] basePath)
        {
            try
            {
                var base64String = Convert.ToBase64String(basePath);
                return base64String;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to read file: {ex.Message}");
                return null;
            }
        }

        public static async Task AddPdfToZip(string pdfFilePath, string zipFilePath)
        {
            try
            {
                // Verificar si el archivo ZIP existe
                if (!File.Exists(zipFilePath))
                {
                    // Si no existe, crear un nuevo archivo ZIP
                    using (var zip = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
                    {
                        // Obtener el nombre del archivo PDF (solo el nombre, no la ruta completa)
                        string fileName = Path.GetFileName(pdfFilePath);

                        // Agregar el archivo PDF al ZIP
                        zip.CreateEntryFromFile(pdfFilePath, fileName);
                        Console.WriteLine($"Created and added PDF {fileName} to ZIP.");
                    }
                }
                else
                {
                    // Si el archivo ZIP ya existe, actualizarlo
                    using (var zip = ZipFile.Open(zipFilePath, ZipArchiveMode.Update))
                    {
                        // Obtener el nombre del archivo PDF (solo el nombre, no la ruta completa)
                        string fileName = Path.GetFileName(pdfFilePath);

                        // Verificar si el archivo ya existe en el ZIP
                        var existingEntry = zip.GetEntry(fileName);
                        if (existingEntry == null)
                        {
                            // Agregar el archivo PDF al ZIP
                            zip.CreateEntryFromFile(pdfFilePath, fileName);
                            Console.WriteLine($"Added PDF {fileName} to ZIP.");
                        }
                        else
                        {
                            Console.WriteLine($"File {fileName} already exists in the ZIP.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding file to ZIP: {ex.Message}");
            }
        }

        public static string GenerateUniqueZipName()
        {
            // Obtener la fecha y hora actual
            string currentDate = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");

            // Generar un GUID único
            string uniqueId = Guid.NewGuid().ToString();

            // Combinar la fecha y el GUID para formar el nombre del archivo ZIP
            string zipName = $"generate_zip_{currentDate}_{uniqueId}.zip";

            return zipName;
        }
    }
}
