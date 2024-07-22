using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rethus_backend.Models;
using rethus_backend.Models.Dto.Comments;
using rethus_backend.Models.Dto.UserForm;
using rethus_backend.Models.Dto.UserFormFiles;
using rethus_backend.Utilities.Constants.PaginatioConstants;
using rethus_backend.Utilities.Constants.User.UserFormConstants;
using System.Net;

namespace rethus_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class UserFormController : ApiBaseController
{
    public UserFormController(IServiceProvider provider)
        : base(provider) { }

    [HttpPost]
    [Authorize(Roles = Policies.User)]
    public async Task<ActionResult<ApiResponse>> PostAsync([FromBody] UserFormCreateDto userForm)
    {
        ApiResponse createUser = await _unitOfWork.UserForm.post(userForm);

        if (createUser.IsSuccess == false)
        {
            return BadRequest(createUser);
        }

        var user_configuration = _unitOfWork.UserConfiguration.GetByUserId(userForm.userId);
        _unitOfWork.UserConfiguration.updateAutomaticStepConfiguration(
            user_configuration.ConfigurationsId
        );

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
    public PaginationResultDto<UserFormResponseDto> GetAllPaginado(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string PersonalIdentification = "",
        [FromQuery] ReviewStepForm step = ReviewStepForm.officer1,
        [FromQuery] ConfigurationTypeProcedure? TypeProcedure = null,
        [FromQuery] string? CreatedAt = null,
        [FromQuery] string? status = null
    )
    {
        var request = new PaginationRequestDto<CommonQueryParametersDto>
        {
            Page = page,
            PageSize = pageSize,
            QueryParameters = new CommonQueryParametersDto { StepForm = step }
        };

        if (PersonalIdentification.Length > 0)
        {
            request.QueryParameters.PersonalIdentification = PersonalIdentification;
        }
        if (TypeProcedure != null)
        {
            request.QueryParameters.TypeProcedure = TypeProcedure;
        }
        if (CreatedAt != null)
        {
            request.QueryParameters.CreatedAt = DateTime.Parse(CreatedAt);
            request.QueryParameters.ComparisonOperators = new Dictionary<string, string>
            {
                ["CreatedAt"] = ">="
            };
        }

        if (status != null)
        {
            var formStatus = UserFormConstants.GetStatus(status);

            if (formStatus != null)
            {
                request.QueryParameters.status = formStatus;
            }
        }

        Func<UserForm, UserFormResponseDto> mapper = user =>
            new UserFormResponseDto
            {
                UserFormId = user.UserFormId,
                PersonalFirstName = user.PersonalFirstName,
                PersonalIdentification = user.PersonalIdentification,
                TypeProcedure = user.TypeProcedure.ToString(),
                StepForm = user.StepForm,
                CreatedAt = user.CreatedAt,
                Status = user.Status.ToString()
            };
        var result = _unitOfWork.UserForm.GetPagination(request, mapper);

        return result;
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
    public ActionResult<ApiResponse> ApprovedForm(string formId)
    {
        ApiResponse approvedForm = _unitOfWork.UserForm.ApprovedForm(formId);

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
        ApiResponse response = _unitOfWork.UserForm.AddConsecutive(formId, payload.consecutive);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        await Task.Run(async () => _unitOfWork.UserForm.ValidateCertificateUserForm(formId));

        return response;
    }

    [HttpGet("donwload/inventory/file/{formId}")]
    [Authorize(Roles = Policies.Inventory)]
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
        var consecutive = await _unitOfWork.UserForm.GetConsecutive(formId);

        if (consecutive == null)
        {
            _response.IsSuccess = true;
            _response.Result = false;
            return Ok(_response);
        }

        _response.IsSuccess = true;
        _response.Messages.Add("El formulario tiene consecutivo");
        _response.Result = consecutive;
        return Ok(_response);
    }
}
