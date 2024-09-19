using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Models.Dto.UserFormFiles;
using rethus_backend.Repository.IRepository;
using rethus_backend.Utilities.Constants.User.UserFormConstants;
using rethus_backend.Utilities.Constants.UserConstants;
using rethus_backend.Utilities.FileHelper;
using System.Diagnostics;

namespace rethus_backend.Repository
{
    public class UserFormFilesRepository : Repository<UserFormFiles>, IUserFormFilesRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly IUserFormRepository _useForm;

        public UserFormFilesRepository(
            ApplicationDbContext db,
            IConfiguration config,
            IUserFormRepository userForm
        )
            : base(db)
        {
            _context = db;
            _config = config;
            _useForm = userForm;
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

            if (payload.Files.Count == 0)
            {
                response.IsSuccess = false;
                response.AddError("La lista de archivo no puede estar vacia");
            }

            var ruta =
                _config.GetSection("routeFileProcedures").Value + userForm.PersonalIdentification;

            FileHelper.CreateFolder(ruta);

            var tareas = new List<Task<string>>();

            foreach (var item in payload.Files)
            {
                tareas.Add(SaveFileToDiskAsync(item, ruta)); // Solo guarda el archivo
            }

            var urls = await Task.WhenAll(tareas); // Esperar que todos los archivos sean guardados

            await SaveFileDetailsToDatabaseAsync(payload.Files, urls, userForm);

            return response;
        }

        private async Task<string> SaveFileToDiskAsync(FileUpload item, string ruta)
        {
            var baseUrlFile = FileHelper.AddAsync(item.File, ruta); // Guardar el archivo en el disco
            return baseUrlFile; // Retornar la URL o path del archivo guardado
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
                // EventLog.WriteEntry("Application", "Error al guarder archivo funcion SaveFileDetailsToDatabaseAsync", EventLogEntryType.Error);
                // EventLog.WriteEntry(
                //     "Application",
                //     $"Excepción: {ex.Message}\nStack Trace: {ex.StackTrace}",
                //     EventLogEntryType.Error
                // );
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
                            FileName = columns.Filename
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
    }
}
