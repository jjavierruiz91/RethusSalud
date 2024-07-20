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
using rethus_backend.Models.Dto.Comments;
using rethus_backend.Utilities.Templates.dto;
using rethus_backend.Utilities.Templates;

namespace rethus_backend.Repository
{
    public class UserFormRepository : Repository<UserForm>, IUserFormRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        private readonly PaginationService<
            UserForm,
            CommonQueryParametersDto,
            UserFormResponseDto
        > _paginationService;

        public UserFormRepository(
            ApplicationDbContext db,
            IConfiguration config,
            PaginationService<
                UserForm,
                CommonQueryParametersDto,
                UserFormResponseDto
            > paginationService
        )
            : base(db)
        {
            _context = db;
            _config = config;
            _paginationService = paginationService;
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

        public PaginationResultDto<UserFormResponseDto> GetPagination(
            PaginationRequestDto<CommonQueryParametersDto> request,
            Func<UserForm, UserFormResponseDto> mapper
        )
        {
            var result = _paginationService.GetPaginatedEntities(request, mapper);

            return result;
        }

        public UserForm GetById(string id)
        {
            return _context.UserForm.Find(id);
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

            if (payload.files.Count == 0)
            {
                response.AddError("La lista de archivo no puede estar vacia");
            }

            // var amount = GetAmountFilesByTypeProcedure(userForm.typeProcedure);

            // if (amount == 0 || payload.files.Count != (int)amount)
            // {
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
                            PersonalCountryBirth = columns.PersonalCountryBirthId,
                            PersonalDepartmentBirth = columns.PersonalDepartmentBirthId,
                            PersonalMunicipalityBirth = columns.PersonalMunicipalityBirthId,
                            DateBirth = columns.DateBirth,
                            PersonalPlaceResidence = columns.PersonalPlaceResidenceId,
                            PersonalDepartmentResidence = columns.PersonalDepartmentResidenceId,
                            PersonalMunicipalityResidence = columns.PersonalMunicipalityResidenceId,
                            PersonalAddress = columns.PersonalAddress,
                            PersonalTelephone = columns.PersonalTelephone,
                            PersonalPhone = columns.PersonalPhone,
                            PersonalEmail = columns.PersonalEmail,
                            PersonalEthnicGroup = columns.PersonalEthnicGroup.ToString(),
                        }
                )
                .FirstOrDefault();

            return userForm;
        }

        public DetailsProccessAcademicDto GetProccessAcademic(string userFormId)
        {
            var userForm = _context.UserForm
                .Where(user => user.UserFormId == userFormId)
                .Select(
                    columns =>
                        new DetailsProccessAcademicDto
                        {
                            AcademicsOriginTitle = columns.AcademicsOriginTitle,
                            AcademicsTypeInstitution = columns.AcademicsTypeInstitution.ToString(),
                            AcademicsProgramType = columns.AcademicsProgramType,
                            AcademicsDepartmentInstitution =
                                columns.AcademicsDepartmentInstitutionId,
                            AcademicsMunicipalityInstitution =
                                columns.AcademicsMunicipalityInstitutionId,
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

            UserForm form = GetById(userFormId);

            if (form == null)
            {
                response.AddError("El commentario no existe", HttpStatusCode.NotFound, false);
                return response;
            }

            form.StepForm = UserFormConstants.GetNextRebiewStepForm(form.StepForm);

            _context.SaveChanges();
            response.Messages.Add("El formulario ha sido aprobado");
            response.Result = form;
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

            _context.SaveChanges();
            response.Messages.Add("El formulario ha sido rechazado");
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }

        public UserForm GetFormUserId(string userId)
        {
            UserForm user = _context.UserForm.FirstOrDefault(x => x.UserId == userId);
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

        public ApiResponse AddConsecutive(string userFormId, string consecutive)
        {
            var response = new ApiResponse();

            UserForm userForm = GetById(userFormId);

            if (userForm == null)
            {
                response.AddError("El formulario no existe", HttpStatusCode.NotFound, false);
                return response;
            }

            if (userForm.StepForm != ReviewStepForm.success)
            {
                response.AddError(
                    "El formulario no tiene el estado correcto",
                    HttpStatusCode.BadRequest,
                    false
                );
            }

            // if (userForm.Status == UserFormStatus.approved)
            // {
            //     response.AddError(
            //         "Este formulario tiene asignado un consecutivo!",
            //         HttpStatusCode.BadRequest,
            //         false
            //     );
            //     return response;
            // }

            if (consecutive.Length == 0)
            {
                response.AddError(
                    "El consecutivo no puede estar vacio",
                    HttpStatusCode.BadRequest,
                    false
                );
                return response;
            }

            userForm.Consecutive = consecutive;
            userForm.Status = UserFormStatus.approved;

            _context.SaveChanges();
            response.Messages.Add("El consecutivo a sido agregado");
            response.StatusCode = HttpStatusCode.OK;
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
                            PersonalIdentification = columns.PersonalIdentification
                        }
                )
                .FirstOrDefault();

            var outputPath =
                _config.GetSection("routeFileProcedures").Value
                + form.PersonalIdentification
                + "//certifcate-"
                + form.TypeProcedure
                + "-"
                + form.PersonalIdentification
                + ".pdf";
            var url = await FileHelper.GetPdfFileAsync(outputPath);

            if (url == null)
            {
                return null;
            }

            var fileBase64 = await FileHelper.FileAsBase64Async(url);

            return fileBase64;
        }

        public void ValidateCertificateUserForm(string userFormId)
        {
            UserForm formFile = _context.UserForm.FirstOrDefault(x => x.UserFormId == userFormId);

            if (formFile == null || formFile.Status != UserFormStatus.approved)
            {
                return;
            }

            if (formFile.TypeProcedure == ConfigurationTypeProcedure.RETHUS)
            {
                Task.Run(async () => CreateCertificateRethus(formFile));
            }
            else
            {
                Task.Run(async () => CreateCertificateSso(formFile));
            }
        }

        public async void CreateCertificateRethus(UserForm form)
        {
            var outputPath =
                _config.GetSection("routeFileProcedures").Value
                + form.PersonalIdentification
                + "//certifcate-rethus-"
                + form.PersonalIdentification
                + ".pdf";

            string certificateRethus = "";

            var rethusDto = new TemplateRethusDto
            {
                CONSECUTIVO = form.Consecutive,
                CONSECUTIVO_FECHA = form.CreatedAt.ToString(),
                NOMBRE_PROFESIONAL = form.PersonalFirstName + form.PersonalLastName,
                EXPEDIDA_PROFESIONAL = form.AcademicsGradeDate.ToString(),
                PROFESION_PROFESIONAL = form.AcademicsProgramName,
                UNIVERSIDAD_PROFESIONAL = form.AcademicsNameInstitution,
                CEDULA_PROFESIONAL = form.PersonalIdentification,
            };

            certificateRethus = await DownloadCertificateRethus(rethusDto);
            await ConverPdfService.ConvertHtmlToPdf(certificateRethus, outputPath);
        }

        public async void CreateCertificateSso(UserForm form)
        {
            var outputPath =
                _config.GetSection("routeFileProcedures").Value
                + form.PersonalIdentification
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
            await ConverPdfService.ConvertHtmlToPdf(certificateRethus, outputPath);
        }
    }
}
