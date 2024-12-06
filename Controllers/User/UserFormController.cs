using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Models.Dto.Pagination;
using rethus_backend.Models.Dto.UserForm;
using rethus_backend.Models.Dto.UserFormFiles;
using rethus_backend.Utilities.Constants.PaginatioConstants;
using rethus_backend.Utilities.Constants.User.UserFormConstants;
using System.Globalization;
using System.Net;

namespace rethus_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class UserFormController : ApiBaseController
{
    private static Task _excelGenerationTask; // Variable para almacenar la tarea de generación de Excel
    private static object _lock = new object(); // Objeto de bloqueo para evitar condiciones de carrera
    private string _excelFilePath;

    private readonly IServiceProvider _provider;

    public UserFormController(IServiceProvider provider)
        : base(provider)
    {
        _provider = provider;
    }

    [HttpPost]
    [Authorize(Roles = Policies.User)]
    public async Task<ActionResult<ApiResponse>> PostAsync([FromBody] UserFormCreateDto userForm)
    {
        var configuration = _unitOfWork.UserConfiguration.ValidateStepConfiguration(
            userForm.userId,
            ConfigurationStep.load_user_form
        );
        if (!configuration.IsSuccess)
        {
            _response.IsSuccess = false;
            _response.Messages = configuration.Messages;
            _response.StatusCode = HttpStatusCode.Conflict;
            return BadRequest(_response);
        }

        ApiResponse createUser = await _unitOfWork.UserForm.post(userForm);

        if (createUser.IsSuccess == false)
        {
            return BadRequest(createUser);
        }
        await Task.Run(() =>
        {
            Configurations user_configuration = _unitOfWork.UserConfiguration.GetByUserId(
                userForm.userId
            );
            var result = _unitOfWork.UserConfiguration.updateAutomaticStepConfiguration(
                user_configuration.ConfigurationsId
            );

            var userFormId = _unitOfWork.UserForm.GetUserFormIdByUserId(userForm.userId);

            if (userFormId == null)
            {
                return;
            }
            _unitOfWork.UserConfiguration.UpdateFormIdConfiguration(
                user_configuration.ConfigurationsId,
                userFormId
            );
        });
        return Ok(createUser);
    }

    [HttpPost("load-files/{userFormId}")]
    public async Task<ActionResult<ApiResponse>> PostAsyncUserFormFiles(
        string userFormId,
        [FromBody] UserFormFilesCreateDto _files
    )
    {
        var response = _unitOfWork.UserForm.RegisterUserFormFile(userFormId, _files);

        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public ActionResult<List<UserForm>> GetAll([FromQuery] int? page)
    {
        var users = _unitOfWork.UserForm.GetAll(page);
        return Ok(users);
    }

    // [Authorize]
    [HttpGet("{id}")]
    public IActionResult GetById(string id)
    {
        var user = _unitOfWork.UserForm.GetById(id);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpGet("personal/{userFormId}")]
    public IActionResult GetInformationPersonalProcess(string userFormId)
    {
        var user = _unitOfWork.UserForm.GetPersonalInformation(userFormId);
        if (user == null)
            return NotFound();

        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        _response.Result = user;
        return Ok(_response);
    }

    [HttpGet("personal/{userFormId}/edit")]
    public IActionResult GetInformationPersonalEditProcess(string userFormId)
    {
        var user = _unitOfWork.UserForm.GetEditPersonalInformation(userFormId);
        if (user == null)
            return NotFound();

        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        _response.Result = user;
        return Ok(_response);
    }

    [HttpGet("validate/{userId}/exist")]
    public IActionResult GetValidateFormUserId(string UserId)
    {
        var user = _unitOfWork.UserForm.ValidateFormUserId(UserId);

        if (user == null)
        {
            _response.Result = null;
        }

        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        _response.Result = user;
        return Ok(_response);
    }

    [HttpGet("academic/{userFormId}")]
    public IActionResult GetInformationAcademicProcess(string userFormId)
    {
        var user = _unitOfWork.UserForm.GetProccessAcademic(userFormId);
        if (user == null)
            return NotFound();

        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        _response.Result = user;
        return Ok(_response);
    }

    [HttpGet("academic/{userFormId}/edit")]
    public IActionResult GetInformationAcademicEditProcess(string userFormId)
    {
        var user = _unitOfWork.UserForm.GetEditInformationAcademic(userFormId);
        if (user == null)
            return NotFound();

        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        _response.Result = user;
        return Ok(_response);
    }

    // Endpoint para aprobar revision
    [HttpPatch("approved/{formId}")]
    [Authorize(
        Roles = Policies.FuncionarioEtapa1
            + ","
            + Policies.FuncionarioEtapa2
            + ","
            + Policies.FuncionarioEtapa3
            + ","
            + Policies.Inventory
    )]
    public async Task<ActionResult<ApiResponse>> ApprovedForm(
        string formId,
        [FromBody] UserReviewRolDto payload
    )
    {
        ApiResponse approvedForm = _unitOfWork.UserForm.ApprovedForm(formId, payload.userRol);

        if (!approvedForm.IsSuccess)
        {
            return BadRequest(approvedForm);
        }

        await Task.Run(() =>
        {
            string userId = _unitOfWork.UserForm.GetUserByUserFormId(formId);
            var user_configuration = _unitOfWork.UserConfiguration.GetByUserId(userId);
            _unitOfWork.UserConfiguration.updateAutomaticStateConfiguration(
                user_configuration.ConfigurationsId
            );
        });
        await Task.Run(() => _unitOfWork.UserForm.ValidateCertificateUserForm(formId));

        return approvedForm;
    }

    // Endpoint para rechazar revision
    [HttpPatch("reject/{formId}")]
    [Authorize(
        Roles = Policies.FuncionarioEtapa1
            + ","
            + Policies.FuncionarioEtapa2
            + ","
            + Policies.FuncionarioEtapa3
            + ","
            + Policies.Inventory
    )]
    public ActionResult<ApiResponse> RejectForm(string formId)
    {
        ApiResponse rejectForm = _unitOfWork.UserForm.RejectForm(formId);
        return rejectForm;
    }

    [HttpPatch("consecutive/{formId}")]
    [Authorize(Roles = Policies.Inventory)]
    public async Task<ActionResult<ApiResponse>> PathAddNewConsecutive(
        string formId,
        UserFormConsecutiveDto payload
    )
    {
        ApiResponse response = await _unitOfWork.UserForm.AddConsecutive(formId, payload);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        await Task.Run(() =>
        {
            string userId = _unitOfWork.UserForm.GetUserByUserFormId(formId);
            _unitOfWork.UserConfiguration.UpdateStateComplateConfiguration(userId);
            _unitOfWork.UserForm.ValidateCertificateUserForm(formId);
        });

        return response;
    }

    [HttpPatch("update/consecutive/{formId}")]
    [Authorize(Roles = Policies.Inventory)]
    public async Task<ActionResult<ApiResponse>> UpdateConsecutive(
        string formId,
        UserFormConsecutiveDto payload
    )
    {
        ApiResponse response = await _unitOfWork.UserForm.UpdateConsecutive(formId, payload);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        await Task.Run(() => _unitOfWork.UserForm.ValidateCertificateUserForm(formId));

        return response;
    }

    [HttpGet("donwload/inventory/file/{formId}")]
    [Authorize(Roles = Policies.FuncionarioEtapa1 + "," + Policies.Inventory)]
    public async Task<ActionResult<ApiResponse>> GetFileByUserFomrId(string formId)
    {
        var detailsFile = await _unitOfWork.UserForm.GetFileInventory(formId);
        _response.Result = detailsFile;
        return _response;
    }

    [HttpGet("inventory/consecutive/{formId}")]
    [Authorize(Roles = Policies.Inventory)]
    public async Task<ActionResult<ApiResponse>> GetConsecutive(string formId)
    {
        var consecutive = await _unitOfWork.UserForm.GetInformationConsecutive(formId);

        if (consecutive == null)
        {
            _response.IsSuccess = true;
            _response.Result = false;
            return BadRequest(_response);
        }

        _response.IsSuccess = true;
        _response.Messages.Add("El formulario tiene consecutivo");
        _response.Result = consecutive;
        return Ok(_response);
    }

    [HttpGet("validate/file/{userId}")]
    [Authorize(Roles = Policies.User)]
    public async Task<ActionResult<ApiResponse>> ValidateFile(string userId)
    {
        bool existFile = _unitOfWork.UserForm.ValidateExistFileForDownload(userId);

        _response.Result = existFile;
        return _response;
    }

    [HttpPatch("update/{formId}")]
    [Authorize(Roles = Policies.User)]
    public async Task<ActionResult<ApiResponse>> patchInformationForm(
        string formId,
        UserFormUpdateDto payload
    )
    {
        bool isHaveComments = _unitOfWork.Comments.CountPendingCommentsExternalForm(formId);
        if (isHaveComments)
        {
            _response.Messages.Add(
                "Te recomendamos revisar y aprobar los comentarios antes de proceder con la actualización de la información."
            );
            _response.IsSuccess = false;
            return BadRequest(_response);
        }

        if (string.IsNullOrEmpty(formId))
        {
            _response.Messages.Add("Form ID is required");
            _response.IsSuccess = false;
            return BadRequest(_response);
        }

        ApiResponse reponse = await _unitOfWork.UserForm.updateFormInformation(formId, payload);

        if (!reponse.IsSuccess)
        {
            return BadRequest(_response);
        }

        await Task.Run(() =>
        {
            string userId = _unitOfWork.UserForm.GetUserByUserFormId(formId);
            _unitOfWork.UserConfiguration.UpdateStateUpdateConfiguration(userId);
        });

        _response.IsSuccess = true;
        _response.Messages.Add("Se actualizo la informacion del formulario x");
        return Ok(_response);
    }

    [HttpGet("generate-excel")]
    [Authorize(Roles = Policies.FuncionarioEtapa1)]
    public async Task<ActionResult> GenerateExcel(string startDate, string endDate)
    {
        // Usamos SemaphoreSlim para gestionar concurrencia de manera asíncrona
        var semaphore = new SemaphoreSlim(1, 1);

        await semaphore.WaitAsync();

        try
        {
            // Verificar si ya hay una tarea en curso
            if (_excelGenerationTask != null && !_excelGenerationTask.IsCompleted)
            {
                _response.Messages.Add(
                    "La generación del Excel ya está en progreso. Por favor, inténtelo más tarde."
                );
                _response.IsSuccess = false;
                return Ok(_response);
            }

            // Obtener total de registros y tamaño de lote
            int totalRecords = _unitOfWork.UserForm.GetTotalRecordsByDateRange(startDate, endDate);
            int batchSize = 500;

            // Generar el archivo Excel de forma asíncrona
            _excelGenerationTask = Task.Run(async () =>
            {
                using (var scope = _provider.CreateScope())
                {
                    var dbContext =
                        scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    try
                    {
                        // Generar un nombre único para el archivo Excel
                        var uuid = Guid.NewGuid().ToString();
                        var date = DateTime.Now.ToString(
                            "dd_MM_yyyy",
                            CultureInfo.InvariantCulture
                        );
                        var sheetName = $"{uuid}_{date}.xlsx";

                        // Ruta donde se guardará el archivo
                        string filePath = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "resources",
                            sheetName
                        );
                        _excelFilePath = filePath;

                        // Llamar al método que genera el archivo en lotes
                        await _unitOfWork.UserForm.GenerateExcelWithBatches(
                            filePath,
                            totalRecords,
                            batchSize,
                            dbContext,
                            startDate,
                            endDate
                        );
                    }
                    catch (Exception ex)
                    {
                        _response.Messages.Add("Error al generar el Excel: " + ex.Message);
                        _response.IsSuccess = false;
                    }
                }
            });

            // Esperar a que se complete la tarea de generación del Excel
            await _excelGenerationTask;

            // Verificar si se generó el archivo exitosamente
            if (System.IO.File.Exists(_excelFilePath))
            {
                var fileBytes = await System.IO.File.ReadAllBytesAsync(_excelFilePath);
                var fileName = Path.GetFileName(_excelFilePath);
                return File(
                    fileBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName
                );
            }
            else
            {
                _response.Messages.Add("Error: El archivo Excel no se pudo generar.");
                _response.IsSuccess = false;
                return Ok(_response);
            }
        }
        finally
        {
            // Liberar el semáforo
            semaphore.Release();
        }
    }

    [HttpGet("pagination")]
    [Authorize(
        Roles = Policies.FuncionarioEtapa1
            + ","
            + Policies.FuncionarioEtapa2
            + ","
            + Policies.FuncionarioEtapa3
            + ","
            + Policies.Inventory
    )]
    public async Task<ActionResult<PaginationResultDto<UserFormResponseDto>>> GetPaginatedUserForms(
        [FromQuery] FilterQueryParametersDto filters,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
    )
    {
        // Crea los parámetros de la query con los filtros recibidos
        var queryParameters = new FilterQueryParametersDto
        {
            UserFormId = filters.UserFormId,
            Status = filters.Status,
            PersonalIdentification = filters.PersonalIdentification,
            TypeProcedure = filters.TypeProcedure,
            startDate = filters.startDate,
            endDate = filters.endDate,
            Step = filters.Step
        };

        // Construye la solicitud de paginación
        var paginationRequest = new PaginationRequestDto<FilterQueryParametersDto>
        {
            Page = page,
            PageSize = pageSize,
            QueryParameters = queryParameters
        };

        // Ejecuta el método GetPagedData pasando el selector para el mapeo a UserFormResponseDto
        var result = await _unitOfWork.UserForm.GetPagedData(
            paginationRequest,
            user =>
                new UserFormResponseDto
                {
                    UserFormId = user.UserFormId,
                    PersonalFirstName = user.PersonalFirstName,
                    PersonalIdentification = user.PersonalIdentification,
                    TypeProcedure = user.TypeProcedure.ToString(),
                    StepForm = user.StepForm,
                    CreatedAt = user.CreatedAt,
                    Status = user.Status.ToString(),
                    Consecutive = user.Consecutive
                }
        );

        // Retorna el resultado con el formato esperado
        return Ok(result);
    }

    [HttpGet("download-excel")]
    // [Authorize(Roles = Policies.FuncionarioEtapa1)]
    public IActionResult DownloadExcel()
    {
        lock (_lock)
        {
            // Verificar si la tarea sigue en progreso
            if (_excelGenerationTask != null && !_excelGenerationTask.IsCompleted)
            {
                return Ok(new { isSuccess = false, message = "El archivo aún se está generando." });
            }
            // Verificar si el archivo está disponible
            if (string.IsNullOrEmpty(_excelFilePath) || !System.IO.File.Exists(_excelFilePath))
            {
                return NotFound(
                    new
                    {
                        isSuccess = false,
                        message = "No se ha generado ningún archivo o no está disponible."
                    }
                );
            }

            // Si el archivo ya está generado, devolver el archivo
            var excelBytes = System.IO.File.ReadAllBytes(_excelFilePath);
            return File(
                excelBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Excel_Generado.xlsx"
            );
        }
    }

    [HttpGet("validate/program/psicologia/{formId}")]
    [Authorize(Roles = Policies.User)]
    public async Task<ActionResult<ApiResponse>> isTypeProgramPsicologia(string formId)
    {
        bool isProgramPsicologia = _unitOfWork.UserForm.IsTypeProgramIsPsicologia(formId);

        _response.Result = isProgramPsicologia;
        return _response;
    }

    [HttpGet("generate-zip")]
    public async Task<ActionResult<ApiResponse>> GenerateZip([FromQuery] DateTime? date)
    {
        // Verificar que la fecha sea proporcionada
        if (date == null)
        {
            _response.AddError(
                "El campo de fecha es obligatorio",
                HttpStatusCode.NotAcceptable,
                false
            );
            return BadRequest(_response);
        }

        var currentDate = DateTime.Today;
        // Verificar que la fecha proporcionada sea del día actual
        if (date.Value.Date != currentDate)
        {
            _response.AddError(
                "Solo se permite generar archivos del día actual",
                HttpStatusCode.NotAcceptable,
                false
            );
            return BadRequest(_response);
        }

        try
        {
            var startDate = currentDate; // Inicio del día (00:00:00)
            var endDate = currentDate.AddDays(1).AddTicks(-1); // Fin del día (23:59:59.9999999)

            // Iniciar la creación de scope para obtener el contexto de la base de datos
            using (var scope = _provider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Llamar al servicio que procesa el ZIP y los PDFs
                await _unitOfWork.UserForm.ProcessGenerateZipPdf(
                    500,
                    startDate,
                    endDate,
                    dbContext
                );

                // Responder que el proceso fue iniciado correctamente
                _response.Messages.Add(
                    "El proceso para generar el ZIP ha comenzado correctamente."
                );
                _response.IsSuccess = true;
                return Ok(_response);
            }
        }
        catch (Exception ex)
        {
            // Manejar errores inesperados
            _response.AddError(
                $"Error al procesar la solicitud: {ex.Message}",
                HttpStatusCode.InternalServerError,
                false
            );
            return StatusCode((int)HttpStatusCode.InternalServerError, _response);
        }
    }
}
