using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Models.Dto.UserFormFiles;
using rethus_backend.Repository.IRepository;
using rethus_backend.Utilities.Constants.User.UserFormConstants;
using rethus_backend.Utilities.Constants.UserConstants;
using rethus_backend.Utilities.FileHelper;
using rethus_backend.Utilities.Templates.dto;

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
                .Select(columns => new UserFormFiles { url = columns.url, type = columns.type })
                .ToList();

            List<string> pdfPaths = filesUrls.Select(file => file.url).ToList();
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

        public ApiResponse RegisterUserFormFile(string userId, UserFormFilesCreateDto payload)
        {
            var response = new ApiResponse();
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

            if (payload.files.Count == 0)
            {
                response.IsSuccess = false;
                response.AddError("La lista de archivo no puede estar vacia");
            }

            // var amount = GetAmountFilesByTypeProcedure(userForm.typeProcedure);

            // if (amount == 0 || payload.files.Count != (int)amount)
            // {
            //   response.IsSuccess = false;
            //   response.AddError("El tipo de tramite no coincide con la cantidad de archivo requerida");
            // }

            var ruta =
                _config.GetSection("routeFileProcedures").Value + userForm.PersonalIdentification;

            FileHelper.CreateFolder(ruta);

            foreach (var item in payload.files)
            {
                var baseUrlFile = FileHelper.AddAsync(item, ruta);

                var form = new UserFormFiles
                {
                    size = item.Length,
                    filename = item.FileName,
                    type = item.ContentType,
                    url = baseUrlFile,
                    UserFormId = userForm.UserFormId
                };

                var createRegister = _context.UserFormFiles.Add(form);
                _context.SaveChanges();
            }
            return response;
        }

        public EnumMaximumAmountFiles GetAmountFilesByTypeProcedure(int typeProcedure)
        {
            var procedure = (EnumProcedure)typeProcedure;

            if (procedure == EnumProcedure.RGNTHST)
                return EnumMaximumAmountFiles.RGNTHST;

            if (procedure == EnumProcedure.TCSSO)
                return EnumMaximumAmountFiles.TCSSO;

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
                            FileName = columns.filename
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
                            url = columns.url,
                            filename = columns.filename,
                            type = columns.type
                        }
                )
                .FirstOrDefault();

            var ruta = "";
            var url = await FileHelper.GetFileAsBase64Async(formFile.url, ruta);

            return new UserFormFileDetails
            {
                FileName = formFile.filename,
                Type = formFile.type,
                Base64Content = url
            };
        }

        public async void CreateCertificate(string userFormId)
        {
            UserForm form = _useForm.GetById(userFormId);

            if (form.StepForm != "inventory")
                return;

            var outputPath =
                _config.GetSection("routeFileProcedures").Value
                + form.PersonalIdentification
                + "//certifcate-"
                + form.PersonalIdentification
                + ".pdf";

            string certificateRethus = "";

            if (form.TypeProcedure == "RETHUS")
            {
                var rethusDto = new TemplateRethusDto { };

                certificateRethus = await _useForm.DownloadCertificateRethus(rethusDto);
                await ConverPdfService.ConvertHtmlToPdf(certificateRethus, outputPath);

                var fileInfov1 = new FileInfo(outputPath);
                if (!fileInfov1.Exists)
                {
                    throw new FileNotFoundException("El archivo no existe.");
                }

                return;
            }
        }
    }
}
