using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;

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
            Console.WriteLine(filePath);
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
    }
}
