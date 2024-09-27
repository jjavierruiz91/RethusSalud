using Microsoft.EntityFrameworkCore;
using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Models.Dto.UserFormFiles;
using rethus_backend.Repository.IRepository;
using rethus_backend.Utilities.Constants.User.UserFormConstants;
using rethus_backend.Utilities.Constants.UserConstants;
using rethus_backend.Utilities.FileHelper;
using System.Diagnostics;
using System.Net;

namespace rethus_backend.Repository
{
    public class UserFormFilesRepository : Repository<UserFormFiles>, IUserFormFilesRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly IUserConfigurationRepository _configuration;

        public UserFormFilesRepository(
            ApplicationDbContext db,
            IConfiguration config,
            IUserConfigurationRepository userConfiguration
        )
            : base(db)
        {
            _context = db;
            _config = config;
            _configuration = userConfiguration;
        }

        public IEnumerable<UserFormFiles> GetAll()
        {
            return _context.UserFormFiles;
        }

        public Task<List<byte[]>> GetFilesByUserFormId(string userFormId)
        {
            var filesUrls = _context.UserFormFiles
                .Where(user => user.UserFormId == userFormId)
                .Select(columns => new UserFormFiles { Url = columns.Url, Type = columns.Type })
                .ToList();

            List<string> pdfPaths = filesUrls.Select(file => file.Url).ToList();
            var ruta = "";
            var urls = FileHelper.GetPdfFilesAsync(pdfPaths, ruta);
            return urls;
        }

        public bool IsExist(string userFormId)
        {
            UserFormFiles user = _context.UserFormFiles.FirstOrDefault(
                x => x.UserFormId == userFormId
            );
            if (user == null)
                return false;
            return true;
        }

        public bool IsExistUserFormFile(string userFormFileId)
        {
            bool isExist = _context.UserFormFiles.Any(x => x.UserFormFilesId == userFormFileId);

            return isExist;
        }

        public bool IsUnique(string userFormId)
        {
            UserFormFiles user = _context.UserFormFiles.FirstOrDefault(
                x => x.UserFormId == userFormId
            );
            return user == null;
        }

        public async Task<ApiResponse> RegisterUserFormFileAsync(
            string userId,
            UserFormFilesCreateDto payload
        )
        {
            var response = new ApiResponse();

            if (payload.Files.Count == 0)
            {
                response.IsSuccess = false;
                response.AddError("La lista de archivo no puede estar vacia");
            }

            var userConfig = _context.Configurations.Any(
                c => c.UserId == userId && c.Step == ConfigurationStep.load_user_files
            );
            if (userConfig == null)
            {
                response.IsSuccess = false;
                response.AddError("EL usuario no se encuentra en el paso de cargar archivos");
                return response;
            }

            var userForm = _context.UserForm.FirstOrDefault(
                x => x.UserId == userId && x.Status == UserFormStatus.pending
            );

            if (userForm == null)
            {
                response.IsSuccess = false;
                response.AddError("EL usuario no tiene formulario activo");
                return response;
            }
            ;

            var ruta = _config.GetSection("routeFileProcedures").Value + userForm.UserFormId;

            FileHelper.CreateFolder(ruta);

            var tareas = new List<Task<string>>();

            foreach (var item in payload.Files)
            {
                tareas.Add(SaveFileToDiskAsync(item.File, ruta));
            }

            var urls = await Task.WhenAll(tareas);

            await SaveFileDetailsToDatabaseAsync(payload.Files, urls, userForm);
            await UpdateUserFormStatusAsync(userId);
            return response;
        }

        private async Task<string> SaveFileToDiskAsync(IFormFile item, string ruta)
        {
            var baseUrlFile = FileHelper.AddAsync(item, ruta);
            return baseUrlFile;
        }

        private async Task SaveFileDetailsToDatabaseAsync(
            List<FileUpload> files,
            string[] urls,
            UserForm userForm
        )
        {
            try
            {
                for (int i = 0; i < files.Count; i++)
                {
                    var form = new UserFormFiles
                    {
                        Size = files[i].File.Length,
                        Filename = files[i].File.FileName,
                        Type = files[i].File.ContentType,
                        Url = urls[i],
                        UserFormId = userForm.UserFormId,
                        TypeUploadFile = files[i].Id
                    };

                    _context.UserFormFiles.Add(form);
                }

                await _context.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                EventLog.WriteEntry(
                    "Application",
                    "Error al guarder archivo funcion SaveFileDetailsToDatabaseAsync",
                    EventLogEntryType.Error
                );
                EventLog.WriteEntry(
                    "Application",
                    $"Excepción: {ex.Message}\nStack Trace: {ex.StackTrace}",
                    EventLogEntryType.Error
                );
            }
        }

        private async Task SaveFileAsync(FileUpload item, string ruta, UserForm userForm)
        {
            var baseUrlFile = FileHelper.AddAsync(item.File, ruta);

            var form = new UserFormFiles
            {
                Size = item.File.Length,
                Filename = item.File.FileName,
                Type = item.File.ContentType,
                Url = baseUrlFile,
                UserFormId = userForm.UserFormId,
                TypeUploadFile = item.Id
            };
            _context.UserFormFiles.Add(form);

            await _context.SaveChangesAsync();
        }

        public EnumMaximumAmountFiles GetAmountFilesByTypeProcedure(int typeProcedure)
        {
            var procedure = (ConfigurationTypeProcedure)typeProcedure;

            if (procedure == ConfigurationTypeProcedure.RETHUS)
                return EnumMaximumAmountFiles.RETHUS;

            if (procedure == ConfigurationTypeProcedure.RETHUS)
                return EnumMaximumAmountFiles.SSO;

            return EnumMaximumAmountFiles.DF;
        }

        public List<GetUserFormIdDto> GetUserFormId(string userFormId)
        {
            var filesUrls = _context.UserFormFiles
                .Where(user => user.UserFormId == userFormId)
                .Select(
                    columns =>
                        new GetUserFormIdDto
                        {
                            UserFileId = columns.UserFormFilesId,
                            FileName = columns.Filename,
                            TypeUploadFile = columns.TypeUploadFile
                        }
                )
                .ToList();

            return filesUrls;
        }

        public async Task<UserFormFileDetails> GetFileByUserFormId(string userFormFileId)
        {
            var formFile = _context.UserFormFiles
                .Where(user => user.UserFormFilesId == userFormFileId)
                .Select(
                    columns =>
                        new UserFormFiles
                        {
                            Url = columns.Url,
                            Filename = columns.Filename,
                            Type = columns.Type
                        }
                )
                .FirstOrDefault();

            var ruta = "";
            var url = await FileHelper.GetFileAsBase64Async(formFile.Url, ruta);

            return new UserFormFileDetails
            {
                FileName = formFile.Filename,
                Type = formFile.Type,
                Base64Content = url
            };
        }

        private async Task UpdateUserFormStatusAsync(string userId)
        {
            try
            {
                var user_configuration = this._configuration.GetByUserId(userId);

                this._configuration.updateAutomaticStepConfiguration(
                    user_configuration.ConfigurationsId
                );

                this._configuration.updateAutomaticStateConfiguration(
                    user_configuration.ConfigurationsId
                );

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error al actualizar el estado del formulario de usuario: {ex.Message}"
                );
            }
        }

        public async Task<ApiResponse> UpdateUserFormFileAsync(
            string userFormId,
            UserFormFilesUpdateDto payload
        )
        {
            var response = new ApiResponse();

            if (payload.Files.Count == 0)
            {
                response.IsSuccess = false;
                response.AddError("La lista de archivo no puede estar vacia");
                return response;
            }

            var userForm = _context.UserForm
                .Where(x => x.UserFormId == userFormId)
                .Select(
                    x =>
                        new
                        {
                            UserFormId = x.UserFormId,
                            PersonalIdentification = x.PersonalIdentification,
                            UserId = x.UserId
                        }
                )
                .FirstOrDefault();

            if (userForm == null)
            {
                response.IsSuccess = false;
                response.AddError(
                    "EL usuario usuario no tiene formulario pendiente por actualizar"
                );
                return response;
            }

            string basePath = Directory.GetCurrentDirectory();
            string ruta = Path.Combine(
                basePath,
                _config.GetSection("routeFileProcedures").Value,
                userForm.UserFormId
            );

            var isValidPath = FileHelper.ValidateDirectoryPath(ruta);
            if (!isValidPath)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.AddError(
                    "Se produjo un error, el usuario no tiene creado el archivo en el sitemas, contactar soporte"
                );
                return response;
            }

            var updateTask = new List<Task>();

            var rutaFile = _config.GetSection("routeFileProcedures").Value + userForm.UserFormId;
            foreach (var item in payload.Files)
            {
                await UpdateSingleFileAsync(item, rutaFile);
            }

            // await UpdateConfigurationStatusInPogress(userForm.UserId);

            response.Messages.Add("El sistema esta procesando tu archivo!");
            response.IsSuccess = true;
            return response;
        }

        private async Task UpdateSingleFileAsync(FileUploadUpdate item, string ruta)
        {
            var userFormFile = await _context.UserFormFiles.FirstOrDefaultAsync(
                f =>
                    f.UserFormFilesId == item.userFormFileId
                    && f.TypeUploadFile == item.typeUploadFile
            );

            if (userFormFile == null)
            {
                throw new FileNotFoundException($"File not found.");
            }

            var newFileUrl = await SaveFileToDiskAsync(item.File, ruta);

            userFormFile.Filename = item.File.FileName;
            userFormFile.Size = item.File.Length;
            userFormFile.Type = item.File.ContentType;
            userFormFile.Url = newFileUrl;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating the database.", ex);
            }
        }

        private async Task UpdateConfigurationStatusInPogress(string userId)
        {
            try
            {
                this._configuration.updateStateInPogressConfiguration(userId);

                var userForm = await _context.UserForm
                    .Where(c => c.UserId == userId)
                    .FirstOrDefaultAsync();

                if (userForm != null)
                {
                    userForm.Status = UserFormStatus.pending;
                    _context.UserForm.Update(userForm);
                    await _context.SaveChangesAsync();
                }
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error al actualizar el estado del formulario de usuario: {ex.Message}"
                );
            }
        }
    }
}
