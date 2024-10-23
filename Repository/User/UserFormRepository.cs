using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Models.Dto.UserForm;
using rethus_backend.Models.Dto.UserFormFiles;
using rethus_backend.Repository.IRepository;
using rethus_backend.Utilities.Constants.PaginatioConstants;
using rethus_backend.Utilities.Constants.UserConstants;
using rethus_backend.Utilities.FileHelper;
using rethus_backend.Utilities.Constants.User.UserFormConstants;

using System.Net;

using rethus_backend.Utilities.Templates.dto;
using rethus_backend.Utilities.Templates;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json;
using ClosedXML.Excel;
using rethus_backend.Models.Dto.Comments;
using rethus_backend.RepositoryV2;
using rethus_backend.Models.Dto.Pagination;
using rethus_backend.Utilities.Constants.User.UserConfiguration;
using DocumentFormat.OpenXml.Drawing;

namespace rethus_backend.Repository
{
    public class UserFormRepository : Repository<UserForm>, IUserFormRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        private readonly IPaginationRepositoryV2<UserForm> _repositoryPaginationV2;

        private readonly IUserConfigurationRepository _userConfiguration;

        public UserFormRepository(
            ApplicationDbContext db,
            IConfiguration config,
            IUserConfigurationRepository userConfiguration,
            IPaginationRepositoryV2<UserForm> repositoryPaginationV2
        )
            : base(db)
        {
            _context = db;
            _config = config;
            _userConfiguration = userConfiguration;
            _repositoryPaginationV2 = repositoryPaginationV2;
        }

        public PaginationResult<UserForm> GetAll(int? page)
        {
            int _page = page ?? 1;
            int pageSize = 10;

            int totalRecords = _context.UserForm.Count();
            int total_pages = (int)Math.Ceiling((decimal)totalRecords / pageSize);

            var pensiones = _context.UserForm.Skip((_page - 1) * pageSize).Take(pageSize).ToList();

            var paginationResult = new PaginationResult<UserForm>
            {
                TotalPages = total_pages,
                TotalRecords = totalRecords,
                CurrentPage = _page,
                Records = pensiones
            };

            return paginationResult;
        }

        public UserForm GetById(string id)
        {
            return _context.UserForm.Find(id);
        }

        public ValidateFileUserForm IsValidUserForm(string userId)
        {
            var userForm = _context.UserForm
                .Where(x => x.UserId == userId && x.Status == UserFormStatus.approved)
                .Select(
                    x =>
                        new ValidateFileUserForm
                        {
                            PersonalIdentification = x.PersonalIdentification,
                            TypeProcedure = x.TypeProcedure
                        }
                )
                .FirstOrDefault();

            if (userForm == null)
            {
                return null;
            }

            return userForm;
        }

        public DetailsProcessUserDto GetDetailProcess(UserFormProcessDto payload)
        {
            var userForm = _context.UserForm
                .Where(
                    user =>
                        user.PersonalTypeIdentification == payload.identificationType
                        && user.PersonalIdentification == payload.identification
                )
                .Select(
                    columns =>
                        new DetailsProcessUserDto
                        {
                            PersonalTypeIdentification = columns.PersonalTypeIdentification,
                            PersonalIdentification = columns.PersonalIdentification,
                            PersonalFirstName = columns.PersonalFirstName,
                            PersonalLastName = columns.PersonalLastName,
                            Status = columns.Status.ToString()
                        }
                )
                .FirstOrDefault();

            return userForm;
        }

        public bool IsExistUser(string userFormId)
        {
            bool userExist = _context.UserForm.Any(x => x.UserFormId == userFormId);

            return userExist;
        }

        public bool IsUniqueUser(string userId)
        {
            UserForm user = _context.UserForm.FirstOrDefault(x => x.PersonalEmail == userId);
            if (user == null)
                return false;
            return true;
        }

        public Task<ApiResponse> post(UserFormCreateDto createRequestDto)
        {
            var response = new ApiResponse();
            if (createRequestDto == null)
                return null;

            var user = _context.Configurations.FirstOrDefault(
                x => x.UserId == createRequestDto.userId
            );
            if (user == null)
            {
                response.AddError("El usuario no existe", HttpStatusCode.BadRequest, false);
                return Task.FromResult(response);
            }

            var userForm = _context.UserForm.FirstOrDefault(
                x => x.UserId == createRequestDto.userId
            );
            if (userForm != null && userForm.Status == UserFormStatus.pending)
            {
                response.AddError(
                    "El usuario tiene un tramite en proceso",
                    HttpStatusCode.BadRequest,
                    false
                );
                return Task.FromResult(response);
            }

            var form = new UserForm
            {
                PersonalTypeIdentification = createRequestDto.PersonalTypeIdentification,
                PersonalGender = createRequestDto.PersonalGender,
                PersonalIdentification = createRequestDto.PersonalIdentification,
                PersonalPlaceOfIssue = createRequestDto.PersonalPlaceOfIssue,
                PersonalFirstName = createRequestDto.PersonalFirstName,
                PersonalLastName = createRequestDto.PersonalLastName,
                PersonalCountryBirthId = createRequestDto.PersonalCountryBirth,
                PersonalDepartmentBirthId = createRequestDto.PersonalDepartmentBirth,
                PersonalMunicipalityBirthId = createRequestDto.PersonalMunicipalityBirth,
                DateBirth = createRequestDto.DateBirth,
                PersonalPlaceResidenceId = createRequestDto.PersonalPlaceResidence,
                PersonalDepartmentResidenceId = createRequestDto.PersonalDepartmentResidence,
                PersonalMunicipalityResidenceId = createRequestDto.PersonalMunicipalityResidence,
                PersonalAddress = createRequestDto.PersonalAddress,
                PersonalTelephone = createRequestDto.PersonalTelephone,
                PersonalPhone = createRequestDto.PersonalPhone,
                PersonalEmail = createRequestDto.PersonalEmail,
                PersonalEthnicGroup = createRequestDto.PersonalEthnicGroup,
                AcademicsOriginTitle = createRequestDto.AcademicsOriginTitle,
                AcademicsTypeInstitution = createRequestDto.AcademicsTypeInstitution,
                AcademicsProgramType = createRequestDto.AcademicsProgramType,
                AcademicsCountryInstitution = createRequestDto.AcademicsCountryInstitution,
                AcademicsDepartmentInstitutionId = createRequestDto.AcademicsDepartmentInstitution,
                AcademicsMunicipalityInstitutionId =
                    createRequestDto.AcademicsMunicipalityInstitution,
                AcademicsNameInstitution = createRequestDto.AcademicsNameInstitution,
                AcademicsProgramName = createRequestDto.AcademicsProgramName,
                AcademicsGradeDate = createRequestDto.AcademicsGradeDate,
                AcademicsEquivalentTitle = createRequestDto.AcademicsEquivalentTitle,
                AcademicsNumberConvalidation = createRequestDto.AcademicsNumberConvalidation,
                AcademicsDateConvalidation = createRequestDto.AcademicsDateConvalidation,
                TypeProcedure = user.TypeProcedure
            };

            form.UserId = createRequestDto.userId;
            form.Status = UserFormStatus.pending;
            form.StepForm = ReviewStepForm.officer1;
            form.CreatedAt = DateTime.Now;
            form.UpdatedAt = DateTime.Now;

            var createUserForm = _context.UserForm.Add(form);
            _context.SaveChanges();

            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Task.FromResult(response);
        }

        public ApiResponse RegisterUserFormFile(string userFormId, UserFormFilesCreateDto payload)
        {
            var response = new ApiResponse();
            var userForm = this.GetById(userFormId);

            if (userForm == null)
            {
                response.AddError(
                    "EL usuario no tiene formulario activo",
                    HttpStatusCode.BadRequest,
                    false
                );
                return response;
            }
            ;

            if (payload.Files.Count == 0)
            {
                response.AddError("La lista de archivo no puede estar vacia");
            }

            // var amount = GetAmountFilesByTypeProcedure(userForm.typeProcedure);

            // if (amount == 0 || payload.files.Count != (int)amount)
            // {
            //   response.AddError("El tipo de tramite no coincide con la cantidad de archivo requerida");
            // }

            var ruta = _config.GetSection("routeFileProcedures").Value + userForm.UserFormId;

            FileHelper.CreateFolder(ruta);

            foreach (var item in payload.Files)
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

                var createRegister = _context.UserFormFiles.Add(form);
                _context.SaveChanges();
            }
            return response;
        }

        public EnumMaximumAmountFiles GetAmountFilesByTypeProcedure(int typeProcedure)
        {
            var procedure = (ConfigurationTypeProcedure)typeProcedure;

            if (procedure == ConfigurationTypeProcedure.RETHUS)
                return EnumMaximumAmountFiles.RETHUS;

            if (procedure == ConfigurationTypeProcedure.SSO)
                return EnumMaximumAmountFiles.SSO;

            return EnumMaximumAmountFiles.DF;
        }

        public DetailsProccessPersonalDto GetPersonalInformation(string userFormId)
        {
            var userForm = _context.UserForm
                .Where(user => user.UserFormId == userFormId)
                .Include(uf => uf.CountryOfBirth)
                .Include(uf => uf.DepartmentBirth)
                .Include(uf => uf.MunicipalityBirth)
                .Include(uf => uf.PlaceResidence)
                .Include(uf => uf.DepartmentResidence)
                .Include(uf => uf.MunicipalityResidence)
                .Include(uf => uf.CountryInstitution)
                .Include(uf => uf.DepartmentInstitution)
                .Include(uf => uf.MunicipalityInstitution)
                .Select(
                    columns =>
                        new DetailsProccessPersonalDto
                        {
                            PersonalTypeIdentification =
                                columns.PersonalTypeIdentification.ToString(),
                            PersonalGender = columns.PersonalGender.ToString(),
                            PersonalIdentification = columns.PersonalIdentification,
                            PersonalFirstName = columns.PersonalFirstName,
                            PersonalLastName = columns.PersonalLastName,
                            PersonalCountryBirth = columns.CountryOfBirth.Name,
                            PersonalDepartmentBirth = columns.DepartmentBirth.Name,
                            PersonalMunicipalityBirth = columns.MunicipalityBirth.Name,
                            DateBirth = columns.DateBirth,
                            PersonalPlaceResidence = columns.PlaceResidence.Name,
                            PersonalDepartmentResidence = columns.DepartmentResidence.Name,
                            PersonalMunicipalityResidence = columns.MunicipalityResidence.Name,
                            PersonalAddress = columns.PersonalAddress,
                            PersonalTelephone = columns.PersonalTelephone,
                            PersonalPhone = columns.PersonalPhone,
                            PersonalEmail = columns.PersonalEmail,
                            PersonalEthnicGroup = columns.PersonalEthnicGroup.ToString(),
                            PersonalPlaceOfIssue = columns.PersonalPlaceOfIssue,
                            Status = columns.Status.ToString()
                        }
                )
                .FirstOrDefault();

            return userForm;
        }

        public ValidateFormUserDto ValidateFormUserId(string UserId)
        {
            var UserForm = _context.UserForm
                .Where(user => user.UserId == UserId)
                .Select(
                    columns =>
                        new ValidateFormUserDto
                        {
                            UserId = columns.UserId,
                            UserFormId = columns.UserFormId,
                        }
                )
                .FirstOrDefault();

            return UserForm;
        }

        public DetailsProccessAcademicDto GetProccessAcademic(string userFormId)
        {
            var userForm = _context.UserForm
                .Where(user => user.UserFormId == userFormId)
                .Include(uf => uf.DepartmentInstitution)
                .Include(uf => uf.MunicipalityInstitution)
                .Select(
                    columns =>
                        new DetailsProccessAcademicDto
                        {
                            AcademicsOriginTitle = columns.AcademicsOriginTitle,
                            AcademicsTypeInstitution = columns.AcademicsTypeInstitution.ToString(),
                            AcademicsProgramType = columns.AcademicsProgramType,
                            AcademicsDepartmentInstitution = columns.DepartmentInstitution.Name,
                            AcademicsMunicipalityInstitution = columns.MunicipalityInstitution.Name,
                            AcademicsCountryInstitution = columns.CountryInstitution.Name,
                            AcademicsNameInstitution = columns.AcademicsNameInstitution,
                            AcademicsProgramName = columns.AcademicsProgramName,
                            AcademicsGradeDate = columns.AcademicsGradeDate,
                            AcademicsNumberConvalidation = columns.AcademicsNumberConvalidation,
                            AcademicsDateConvalidation = columns.AcademicsDateConvalidation,
                            AcademicsEquivalentTitle = columns.AcademicsEquivalentTitle,
                        }
                )
                .FirstOrDefault();
            return userForm;
        }

        public ApiResponse ApprovedForm(string userFormId)
        {
            var response = new ApiResponse();

            UserForm form = GetFormUserId(userFormId);
            var oldStatus = form.Status;
            if (form == null)
            {
                response.AddError("El formulario no existe", HttpStatusCode.NotFound, false);
                return response;
            }

            form.StepForm = UserFormConstants.GetNextRebiewStepForm(form.StepForm);
            form.Status = UserFormStatus.pending;

            _context.SaveChanges();
            if (oldStatus == UserFormStatus.reject)
            {
                _userConfiguration.updateStateInPogressConfiguration(form.UserId);
            }

            response.Messages.Add("El formulario ha sido aprobado");
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }

        public ApiResponse RejectForm(string userFormId)
        {
            var response = new ApiResponse();

            UserForm comment = GetById(userFormId);

            if (comment == null)
            {
                response.AddError("El formulario no existe", HttpStatusCode.NotFound, false);
                return response;
            }

            comment.StepForm = ReviewStepForm.officer1;
            comment.Status = UserFormStatus.reject;

            _userConfiguration.updateStateRejectConfiguration(comment.UserId);

            _context.SaveChanges();
            response.Messages.Add("El formulario ha sido rechazado");
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }

        public UserForm GetFormUserId(string userFormId)
        {
            UserForm user = _context.UserForm.FirstOrDefault(x => x.UserFormId == userFormId);
            return user;
        }

        public UserForm IsDownloadCertificate(string userId)
        {
            UserForm user = _context.UserForm.FirstOrDefault(
                x => x.UserId == userId && x.StepForm == ReviewStepForm.success
            );
            return user;
        }

        public async Task<string> DownloadCertificateRethus(TemplateRethusDto dto)
        {
            var response = new ApiResponse();

            var filePath = _config.GetSection("routeTemplateRethus").Value;
            string templateContent = await FileHelper.ReadFileContentAsync(filePath);

            string resultContent = TemplateHelper.ReplacePlaceholdersRethus(templateContent, dto);

            // byte[] pdfBytes = _dinkToPdfService.ConvertHtmlToPdf(resultContent); para convertir el archivo en pdf

            // string resultContentBase64 = ConvertToBase64(resultContent);

            // response.Result = resultContentBase64;
            return resultContent;
        }

        public async Task<string> DownloadCertificateSso(TemplateSSODto ssoDto)
        {
            var filePath = _config.GetSection("routeTemplateRethus").Value;
            string templateContent = await FileHelper.ReadFileContentAsync(filePath);

            string resultContent = TemplateHelper.ReplacePlaceholdersSSO(templateContent, ssoDto);
            // string resultContentBase64 = ConvertToBase64(resultContent);

            return resultContent;
        }

        public async Task<ApiResponse> AddConsecutive(
            string userFormId,
            UserFormConsecutiveDto payload
        )
        {
            var response = new ApiResponse();

            if (payload.consecutive.Length == 0)
            {
                response.AddError(
                    "El consecutivo no puede estar vacio",
                    HttpStatusCode.BadRequest,
                    false
                );
                response.IsSuccess = false;
                return response;
            }

            if (
                !DateTime.TryParseExact(
                    payload.consecutiveDate,
                    "dd/MM/yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var parsedDate
                )
            )
            {
                response.AddError(
                    "La fecha es inválida o no está en el formato dd/MM/yyyy",
                    HttpStatusCode.BadRequest,
                    false
                );
                response.IsSuccess = false;
                return response;
            }

            UserForm userForm = GetById(userFormId);

            if (userForm == null)
            {
                response.AddError("El formulario no existe", HttpStatusCode.NotFound, false);
                response.IsSuccess = false;
                return response;
            }
            if (
                userForm.StepForm != ReviewStepForm.success
                || userForm.Status != UserFormStatus.pending
            )
            {
                response.AddError(
                    "El formulario no esta listo para agregar el consecutivo",
                    HttpStatusCode.BadRequest,
                    false
                );
                return response;
            }

            userForm.Consecutive = payload.consecutive;
            userForm.ConsecutiveDate = payload.consecutiveDate;
            userForm.Status = UserFormStatus.approved;

            _context.SaveChanges();

            response.Messages.Add("El consecutivo a sido agregado");
            response.StatusCode = HttpStatusCode.OK;
            response.IsSuccess = true;
            return response;
        }

        public async Task<string> GetFileInventory(string userFormFileId)
        {
            var form = _context.UserForm
                .Where(form => form.UserFormId == userFormFileId)
                .Select(
                    columns =>
                        new SelectInformationFileIventory
                        {
                            Status = columns.Status,
                            TypeProcedure = columns.TypeProcedure,
                            PersonalIdentification = columns.PersonalIdentification,
                            UserFormId = columns.UserFormId
                        }
                )
                .FirstOrDefault();

            var outputPath =
                _config.GetSection("routeFileProcedures").Value
                + form.UserFormId
                + "//certifcate-"
                + form.TypeProcedure
                + "-"
                + form.UserFormId
                + ".pdf";
            var url = await FileHelper.GetPdfFileAsync(outputPath);

            if (url == null)
            {
                return null;
            }

            var fileBase64 = await FileHelper.FileAsBase64Async(url);

            return fileBase64;
        }

        public async Task ValidateCertificateUserForm(string userFormId)
        {
            UserForm formFile = _context.UserForm.FirstOrDefault(x => x.UserFormId == userFormId);

            if (formFile == null)
            {
                return;
            }

            if (formFile != null && formFile.Status != UserFormStatus.approved)
            {
                return;
            }

            if (formFile.Consecutive == null || formFile.ConsecutiveDate == null)
            {
                return;
            }

            if (formFile.TypeProcedure == ConfigurationTypeProcedure.RETHUS)
            {
                await Task.Run(async () => CreateCertificateRethus(formFile));
            }
            else
            {
                await Task.Run(async () => CreateCertificateSso(formFile));
            }
        }

        public async void CreateCertificateRethus(UserForm form)
        {
            var outputPath =
                _config.GetSection("routeFileProcedures").Value
                + form.UserFormId
                + "//certifcate-rethus-"
                + form.PersonalIdentification
                + ".pdf";

            if (FileHelper.ValidatePath(outputPath))
            {
                var deleted = FileHelper.DeleteAsync(outputPath);

                outputPath =
                    _config.GetSection("routeFileProcedures").Value
                    + form.UserFormId
                    + "//certifcate-rethus-"
                    + form.PersonalIdentification
                    + ".pdf";

                if (!deleted)
                {
                    throw new Exception("No se pudo eliminar el archivo existente.");
                }
            }

            string certificateRethus = "";

            var rethusDto = new TemplateRethusDto
            {
                CONSECUTIVO = form.Consecutive,
                CONSECUTIVO_FECHA = form.ConsecutiveDate,
                NOMBRE_PROFESIONAL = form.PersonalFirstName + form.PersonalLastName,
                EXPEDIDA_PROFESIONAL = form.PersonalPlaceOfIssue,
                PROFESION_PROFESIONAL = form.AcademicsProgramName,
                UNIVERSIDAD_PROFESIONAL = form.AcademicsNameInstitution,
                CEDULA_PROFESIONAL = form.PersonalIdentification,
            };

            string jsonContent = await File.ReadAllTextAsync(
                "./resources/templates/certificate/data_config_users.json"
            );

            ConfigTemplate config = JsonSerializer.Deserialize<ConfigTemplate>(jsonContent);

            if (config != null)
            {
                rethusDto.FIRMA_PRINCIPAL = config.FIRMA_PRINCIPAL;
                rethusDto.NOMBRE_FIRMANTE = config.NOMBRE_FIRMANTE;
                rethusDto.TIPO_TRABAJO = config.TIPO_TRABAJO;
                rethusDto.FIRMA_PROYECTO = config.FIRMA_PROYECTO;
                rethusDto.FIRMA_1 = config.FIRMA_1;
                rethusDto.FIRMA_REVISION = config.FIRMA_REVISION;
                rethusDto.FIRMA_2 = config.FIRMA_2;
                rethusDto.FIRMA_APROBO = config.FIRMA_APROBO;
                rethusDto.FIRMA_3 = config.FIRMA_3;
            }
            certificateRethus = await DownloadCertificateRethus(rethusDto);
            var file = new ConvertPdfService();
            await file.ConvertHtmlToPdf(certificateRethus, outputPath);
        }

        public async void CreateCertificateSso(UserForm form)
        {
            var outputPath =
                _config.GetSection("routeFileProcedures").Value
                + form.UserFormId
                + "//certifcate-sso-"
                + form.PersonalIdentification
                + ".pdf";

            string certificateRethus = "";

            var rethusDto = new TemplateSSODto
            {
                CODIGO_PLAZA = form.Consecutive,
                MODALIDAD = "ESPERA",
                NOMBRE_INSTITUCION = form.AcademicsNameInstitution,
                UBICACION_PLAZA = form.AcademicsProgramName,
            };

            certificateRethus = await DownloadCertificateSso(rethusDto);
            var file = new ConvertPdfService();
            await file.ConvertHtmlToPdf(certificateRethus, outputPath);
        }

        public Task<UserFormConsecutiveResponseDto> GetInformationConsecutive(string userFormId)
        {
            var response = _context.UserForm
                .Where(c => c.UserFormId == userFormId)
                .Select(
                    c =>
                        new UserFormConsecutiveResponseDto
                        {
                            Consecutive = c.Consecutive,
                            ConsecutiveDate = c.ConsecutiveDate
                        }
                )
                .FirstOrDefaultAsync();

            return response;
        }

        public string GetUserByUserFormId(string userFormId)
        {
            var userId = _context.UserForm
                .Where(x => x.UserFormId == userFormId)
                .Select(x => x.UserId)
                .FirstOrDefault();

            return userId;
        }

        public bool ValidateExistFileForDownload(string userId)
        {
            var userForm = _context.UserForm
                .Where(x => x.UserId == userId && x.Status == UserFormStatus.approved)
                .Select(
                    x =>
                        new ValidateFileUserForm
                        {
                            PersonalIdentification = x.PersonalIdentification,
                            TypeProcedure = x.TypeProcedure,
                            UserFormId = x.UserFormId
                        }
                )
                .FirstOrDefault();

            if (userForm == null)
            {
                return false;
            }

            var typeProcedure =
                userForm.TypeProcedure == ConfigurationTypeProcedure.RETHUS
                    ? "//certifcate-rethus-"
                    : "//certifcate-sso-";

            var outputPath =
                _config.GetSection("routeFileProcedures").Value
                + userForm.UserFormId
                + typeProcedure
                + userForm.UserFormId
                + ".pdf";

            bool existFile = FileHelper.ValidatePath(outputPath);
            return existFile;
        }

        public async Task<string?> DonwloadCertificateFileByUserId(string userId)
        {
            var userForm = _context.UserForm
                .Where(form => form.UserId == userId)
                .Select(
                    columns =>
                        new ValidateFileUserForm
                        {
                            PersonalIdentification = columns.PersonalIdentification,
                            TypeProcedure = columns.TypeProcedure,
                            UserFormId = columns.UserFormId
                        }
                )
                .FirstOrDefault();

            if (userForm == null)
            {
                return null;
            }

            var typeProcedure =
                userForm.TypeProcedure == ConfigurationTypeProcedure.RETHUS
                    ? "//certifcate-rethus-"
                    : "//certifcate-sso-";

            var outputPath =
                _config.GetSection("routeFileProcedures").Value
                + userForm.UserFormId
                + typeProcedure
                + userForm.UserFormId
                + ".pdf";

            return outputPath;
        }

        public DetailsEditProccessPersonalDto GetEditPersonalInformation(string userFormId)
        {
            var userForm = _context.UserForm
                .Where(user => user.UserFormId == userFormId)
                .Include(uf => uf.CountryOfBirth)
                .Include(uf => uf.DepartmentBirth)
                .Include(uf => uf.MunicipalityBirth)
                .Include(uf => uf.PlaceResidence)
                .Include(uf => uf.DepartmentResidence)
                .Include(uf => uf.MunicipalityResidence)
                .Include(uf => uf.CountryInstitution)
                .Include(uf => uf.DepartmentInstitution)
                .Include(uf => uf.MunicipalityInstitution)
                .Select(
                    columns =>
                        new DetailsEditProccessPersonalDto
                        {
                            PersonalTypeIdentification =
                                columns.PersonalTypeIdentification.ToString(),
                            PersonalGender = columns.PersonalGender.ToString(),
                            PersonalIdentification = columns.PersonalIdentification,
                            PersonalFirstName = columns.PersonalFirstName,
                            PersonalLastName = columns.PersonalLastName,
                            PersonalCountryBirthName = columns.CountryOfBirth.Name,
                            PersonalCountryBirth = columns.CountryOfBirth.CountryId,
                            PersonalDepartmentBirthName = columns.DepartmentBirth.Name,
                            PersonalDepartmentBirth = columns.DepartmentBirth.CountryId,
                            PersonalMunicipalityBirthName = columns.MunicipalityBirth.Name,
                            PersonalMunicipalityBirth = columns.MunicipalityBirth.CityId,
                            DateBirth = columns.DateBirth,
                            PersonalPlaceResidenceName = columns.PlaceResidence.Name,
                            PersonalPlaceResidence = columns.PlaceResidence.CountryId,
                            PersonalDepartmentResidenceName = columns.DepartmentResidence.Name,
                            PersonalDepartmentResidence = columns.DepartmentResidence.CountryId,
                            PersonalMunicipalityResidenceName = columns.MunicipalityResidence.Name,
                            PersonalMunicipalityResidence = columns.MunicipalityResidence.CityId,
                            PersonalAddress = columns.PersonalAddress,
                            PersonalTelephone = columns.PersonalTelephone,
                            PersonalPhone = columns.PersonalPhone,
                            PersonalEmail = columns.PersonalEmail,
                            PersonalEthnicGroup = columns.PersonalEthnicGroup.ToString(),
                            PersonalPlaceOfIssue = columns.PersonalPlaceOfIssue
                        }
                )
                .FirstOrDefault();

            return userForm;
        }

        public DetailsEditProccessAcademicDto GetEditInformationAcademic(string userFormId)
        {
            var userForm = _context.UserForm
                .Where(user => user.UserFormId == userFormId)
                .Include(uf => uf.DepartmentInstitution)
                .Include(uf => uf.MunicipalityInstitution)
                .Select(
                    columns =>
                        new DetailsEditProccessAcademicDto
                        {
                            AcademicsOriginTitle = columns.AcademicsOriginTitle,
                            AcademicsTypeInstitution = columns.AcademicsTypeInstitution.ToString(),
                            AcademicsProgramType = columns.AcademicsProgramType,
                            AcademicsDepartmentInstitutionName = columns.DepartmentInstitution.Name,
                            AcademicsDepartmentInstitution = columns
                                .DepartmentInstitution
                                .DepartmentId,
                            AcademicsMunicipalityInstitutionName = columns
                                .MunicipalityInstitution
                                .Name,
                            AcademicsMunicipalityInstitution = columns
                                .MunicipalityInstitution
                                .CityId,
                            AcademicsCountryInstitutionName = columns.CountryInstitution.Name,
                            AcademicsCountryInstitution = columns.CountryInstitution.CountryId,
                            AcademicsNameInstitution = columns.AcademicsNameInstitution,
                            AcademicsProgramName = columns.AcademicsProgramName,
                            AcademicsGradeDate = columns.AcademicsGradeDate,
                            AcademicsNumberConvalidation = columns.AcademicsNumberConvalidation,
                            AcademicsDateConvalidation = columns.AcademicsDateConvalidation,
                            AcademicsEquivalentTitle = columns.AcademicsEquivalentTitle,
                        }
                )
                .FirstOrDefault();
            return userForm;
        }

        public async Task<ApiResponse> updateFormInformation(
            string formId,
            UserFormUpdateDto payload
        )
        {
            ApiResponse response = new ApiResponse();

            UserForm form = this.GetById(formId);

            if (form == null)
            {
                response.AddError(
                    "No se encontro el formulario del usuario",
                    HttpStatusCode.NotFound,
                    false
                );
            }

            var payloadProperties = typeof(UserFormUpdateDto).GetProperties();

            foreach (var property in payloadProperties)
            {
                // Obtener el valor de la propiedad del payload
                var payloadValue = property.GetValue(payload);

                // Solo actualizar si el valor no es nulo (significa que fue enviado)
                if (payloadValue != null)
                {
                    // Buscar la propiedad correspondiente en el formulario (form)
                    var formProperty = typeof(UserForm).GetProperty(property.Name);

                    // Si la propiedad existe en la entidad, la actualizamos
                    if (formProperty != null)
                    {
                        formProperty.SetValue(form, payloadValue);
                    }
                }
            }
            await _context.SaveChangesAsync();

            response.IsSuccess = true;
            response.Messages.Add("Se actualizo correctamente la informacion del formulario");
            return response;
        }

        private async Task<List<CombinedDetailsProccessDto>> GetUserFormsBatch(
            ApplicationDbContext dbContext,
            int skip,
            int take,
            string startDate,
            string endDate
        )
        {
            string startDateString = startDate?.Trim();
            string endDateString = endDate?.Trim();

            DateTime parseStartDate = DateTime.ParseExact(
                startDateString,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture
            );
            DateTime parseEndDate = DateTime
                .ParseExact(endDateString, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                .AddDays(1)
                .AddTicks(-1);

            return await dbContext.UserForm
                .Where(
                    uf =>
                        uf.Status == UserFormStatus.approved
                        && uf.Consecutive != null
                        && uf.ConsecutiveDate != null
                        && uf.CreatedAt >= parseStartDate
                        && uf.CreatedAt <= parseEndDate
                )
                .Include(uf => uf.DepartmentBirth)
                .Include(uf => uf.MunicipalityBirth)
                .Include(uf => uf.DepartmentResidence)
                .Include(uf => uf.MunicipalityResidence)
                .Include(uf => uf.CountryInstitution)
                .Include(uf => uf.DepartmentInstitution)
                .OrderBy(u => u.UserFormId)
                .Skip(skip)
                .Take(take)
                .Select(
                    columns =>
                        new CombinedDetailsProccessDto
                        {
                            PersonalTypeIdentification =
                                columns.PersonalTypeIdentification.ToString(),
                            PersonalGender = columns.PersonalGender.ToString(),
                            PersonalIdentification = columns.PersonalIdentification,
                            PersonalFirstName = columns.PersonalFirstName,
                            PersonalLastName = columns.PersonalLastName,
                            PersonalDepartmentBirth = columns.DepartmentBirth.Name,
                            PersonalMunicipalityBirth = columns.MunicipalityBirth.Name,
                            DateBirth = columns.DateBirth,
                            PersonalDepartmentResidence = columns.DepartmentResidence.Name,
                            PersonalMunicipalityResidence = columns.MunicipalityResidence.Name,
                            PersonalAddress = columns.PersonalAddress,
                            PersonalTelephone = columns.PersonalTelephone,
                            PersonalPhone = columns.PersonalPhone,
                            PersonalEmail = columns.PersonalEmail,
                            PersonalEthnicGroup = columns.PersonalEthnicGroup.ToString(),
                            AcademicsTypeInstitution = columns.AcademicsTypeInstitution.ToString(),
                            AcademicsProgramType = columns.AcademicsProgramType,
                            AcademicsProgramName = columns.AcademicsProgramName,
                            AcademicsGradeDate = columns.AcademicsGradeDate,
                            AcademicsNumberConvalidation = columns.AcademicsNumberConvalidation,
                            AcademicsDateConvalidation = columns.AcademicsDateConvalidation,
                            AcademicsEquivalentTitle = columns.AcademicsEquivalentTitle,
                            Consecutive = columns.Consecutive,
                            ConsecutiveDate = columns.ConsecutiveDate,
                        }
                )
                .ToListAsync();
        }

        public async Task GenerateExcelWithBatches(
            string filePath,
            int totalRecords,
            int batchSize,
            ApplicationDbContext dbContext,
            string startDate,
            string endDate
        )
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("data");

                // Definir las cabeceras del archivo Excel
                worksheet.Cell(1, 1).Value = "TIPO DOCUMENTO";
                worksheet.Cell(1, 2).Value = "GENERO";
                worksheet.Cell(1, 3).Value = "IDENTIFICACION";
                worksheet.Cell(1, 4).Value = "NOMBRE Y APELLIDOS";
                worksheet.Cell(1, 5).Value = "DEPARTAMENTO DE NACIMIENTO";
                worksheet.Cell(1, 6).Value = "MUNICIPIO NACIMIENTO";
                worksheet.Cell(1, 7).Value = "FECHA NACIMIENTO";
                worksheet.Cell(1, 8).Value = "DEPARTAMENTO DE RESIDENCIA";
                worksheet.Cell(1, 9).Value = "MUNICIPIO DE RESIDENCIA";
                worksheet.Cell(1, 10).Value = "DIRECCION";
                worksheet.Cell(1, 11).Value = "TELEFONO FIJO";
                worksheet.Cell(1, 12).Value = "CELULAR";
                worksheet.Cell(1, 13).Value = "CORREO ELECTRONICO";
                worksheet.Cell(1, 14).Value = "GRUPO ETNICO";
                worksheet.Cell(1, 15).Value = "INSTITUCION";
                worksheet.Cell(1, 16).Value = "TIPO DE PROGRAMA";
                worksheet.Cell(1, 17).Value = "NOMBRE DEL PROGRAMA";
                worksheet.Cell(1, 18).Value = "FECHA DE GRADO";
                worksheet.Cell(1, 19).Value = "NUMERO CONVALIDACION";
                worksheet.Cell(1, 20).Value = "FECHA DE CONVALIDACION";
                worksheet.Cell(1, 21).Value = "TITULO EQUIVALENTE";
                worksheet.Cell(1, 22).Value =
                    "NUMERO DEL ACTO ADMINISTRATIVO QUE AUTORIZA EL SERVICIO";
                worksheet.Cell(1, 23).Value =
                    "FECHA DEL ACTO ADMINISTRATIVO QUE AUTORIZA EL SERVICIO";

                int currentRow = 2;

                int totalBatches = (int)Math.Ceiling((double)totalRecords / batchSize);

                // Crear un semáforo que permite 5 tareas a la vez
                SemaphoreSlim semaphore = new SemaphoreSlim(10);
                try
                {
                    var tasks = new List<Task>();

                    for (int i = 0; i < totalBatches; i++)
                    {
                        int skip = i * batchSize;
                        var task = Task.Run(async () =>
                        {
                            await semaphore.WaitAsync();
                            try
                            {
                                var batchData = await this.GetUserFormsBatch(
                                    dbContext,
                                    skip,
                                    batchSize,
                                    startDate,
                                    endDate
                                );

                                lock (worksheet)
                                {
                                    foreach (var user in batchData)
                                    {
                                        worksheet.Cell(currentRow, 1).Value =
                                            user.PersonalTypeIdentification;
                                        worksheet.Cell(currentRow, 2).Value = user.PersonalGender;
                                        worksheet.Cell(currentRow, 3).Value =
                                            user.PersonalIdentification;
                                        worksheet.Cell(currentRow, 4).Value =
                                            user.PersonalFirstName + " " + user.PersonalLastName;
                                        worksheet.Cell(currentRow, 5).Value =
                                            user.PersonalDepartmentBirth;
                                        worksheet.Cell(currentRow, 6).Value =
                                            user.PersonalMunicipalityBirth;
                                        worksheet.Cell(currentRow, 7).Value = user.DateBirth;
                                        worksheet.Cell(currentRow, 8).Value =
                                            user.PersonalDepartmentResidence;
                                        worksheet.Cell(currentRow, 9).Value =
                                            user.PersonalMunicipalityResidence;
                                        worksheet.Cell(currentRow, 10).Value = user.PersonalAddress;
                                        worksheet.Cell(currentRow, 11).Value =
                                            user.PersonalTelephone;
                                        worksheet.Cell(currentRow, 12).Value = user.PersonalPhone;
                                        worksheet.Cell(currentRow, 13).Value = user.PersonalEmail;
                                        worksheet.Cell(currentRow, 14).Value =
                                            user.PersonalEthnicGroup;
                                        worksheet.Cell(currentRow, 15).Value =
                                            user.AcademicsTypeInstitution;
                                        worksheet.Cell(currentRow, 16).Value =
                                            user.AcademicsProgramType;
                                        worksheet.Cell(currentRow, 17).Value =
                                            user.AcademicsProgramName;
                                        worksheet.Cell(currentRow, 18).Value =
                                            user.AcademicsGradeDate;
                                        worksheet.Cell(currentRow, 19).Value =
                                            user.AcademicsNumberConvalidation;
                                        worksheet.Cell(currentRow, 20).Value =
                                            user.AcademicsDateConvalidation;
                                        worksheet.Cell(currentRow, 21).Value =
                                            user.AcademicsEquivalentTitle;
                                        worksheet.Cell(currentRow, 22).Value = user.Consecutive;
                                        worksheet.Cell(currentRow, 23).Value = user.ConsecutiveDate;

                                        currentRow++;
                                    }
                                }
                            }
                            finally
                            {
                                semaphore.Release();
                            }
                        });
                        tasks.Add(task);
                    }

                    await Task.WhenAll(tasks);
                    try
                    {
                        workbook.SaveAs(filePath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al guardar el archivo: {ex.Message}");
                        throw new Exception("No se pudo guardar el archivo de Excel.", ex);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($">>>>>>>>>>>>>>: {ex.Message}");
                    throw new Exception("No se pudo guardar el archivo de Excel.", ex);
                }
            }
        }

        public int GetTotalRecords()
        {
            return _context.UserForm.Count();
        }

        public int GetTotalRecordsByDateRange(string startDate, string endDate)
        {
            string startDateString = startDate?.Trim();
            string endDateString = endDate?.Trim();

            DateTime parseStartDate = DateTime.ParseExact(
                startDateString,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture
            );
            DateTime parseEndDate = DateTime
                .ParseExact(endDateString, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                .AddDays(1)
                .AddTicks(-1);

            return _context.UserForm.Count(
                uf => uf.CreatedAt >= parseStartDate && uf.CreatedAt <= parseEndDate
            );
        }

        public async Task<PaginationResultDto<TResult>> GetPagedData<TResult>(
            PaginationRequestDto<FilterQueryParametersDto> request,
            Func<UserForm, TResult> selector
        )
        {
            return await _repositoryPaginationV2.GetPagedAsync(request, selector);
        }

        public async Task<List<UserForm>> GetUserFormsByBatch(
            int batchSize,
            int pageNumber,
            ApplicationDbContext dbContext
        )
        {
            return await dbContext.UserForm
                .Where(
                    x =>
                        x.Status == UserFormStatus.pending
                        && x.StepForm == ReviewStepForm.success
                        && x.Consecutive == null
                        && x.ConsecutiveDate == null
                )
                .OrderByDescending(x => x.CreatedAt) // Ordenar por fecha de creación, más reciente primero
                .Skip((pageNumber - 1) * batchSize) // Paginación: saltar los formularios de páginas anteriores
                .Take(batchSize) // Tomar solo la cantidad definida por batchSize
                .ToListAsync();
        }

        public async Task ProcessUserFormCertificatesByBatch(
            int batchSize,
            int maxDegreeOfParallelism,
            int consecutiveStart,
            int consecutiveEnd,
            string consecutiveDate,
            ApplicationDbContext dbContext
        )
        {
            int pageNumber = 1;
            int totalForms = 0;
            int completedForms = consecutiveEnd;

            List<UserForm> userFormsBatch;

            // Bucle para obtener y procesar cada lote de formularios
            do
            {
                // Obtener un lote paginado de formularios

                userFormsBatch = await GetUserFormsByBatch(batchSize, pageNumber, dbContext);

                if (
                    userFormsBatch == null
                    || !userFormsBatch.Any()
                    || consecutiveStart == completedForms
                )
                {
                    break; // No hay más formularios para procesar
                }

                if (totalForms == 0)
                {
                    // Solo calcular el total una vez en el primer lote
                    totalForms = await dbContext.UserForm.CountAsync(
                        x =>
                            x.Status == UserFormStatus.approved
                            && x.Consecutive != null
                            && x.ConsecutiveDate != null
                    );
                }

                var tasks = new List<Task>();

                foreach (var userForm in userFormsBatch)
                {
                    var task = Task.Run(async () =>
                    {
                        lock (dbContext)
                        {
                            if (userForm.StepForm == ReviewStepForm.success)
                            {
                                userForm.Consecutive = consecutiveStart.ToString();
                                userForm.ConsecutiveDate = consecutiveDate;
                                userForm.Status = UserFormStatus.approved;

                                dbContext.UserForm.Update(userForm);
                                _ = dbContext.SaveChanges();

                                var userConfiguration = dbContext.Configurations.FirstOrDefault(
                                    c => c.UserId == userForm.UserId
                                );

                                if (userConfiguration == null)
                                {
                                    throw new InvalidOperationException(
                                        $"No se encontró la configuración para el UserId {userForm.UserId}"
                                    );
                                }

                                userConfiguration.State = ConfigurationsState.completed;
                                userConfiguration.Step = ConfigurationStep.success;

                                dbContext.Configurations.Update(userConfiguration);
                                dbContext.SaveChanges();

                                consecutiveStart++;

                                if (userForm.TypeProcedure == ConfigurationTypeProcedure.RETHUS)
                                {
                                    CreateCertificateRethus(userForm);
                                }
                                else
                                {
                                    CreateCertificateSso(userForm);
                                }
                            }
                        }
                    });

                    tasks.Add(task);
                }

                // Esperar a que todas las tareas del lote terminen
                await Task.WhenAll(tasks);

                // Pasar a la siguiente página
                pageNumber++;
            } while (userFormsBatch.Count == batchSize); // Continuar si el tamaño del lote es igual al batchSize
        }
    }
}
