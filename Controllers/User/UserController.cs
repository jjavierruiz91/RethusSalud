using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rethus_backend.Models;
using rethus_backend.Models.Dto.User;
using rethus_backend.Models.Dto.Configuration;
using System.Net;
using rethus_backend.Utilities.Constants.UserConstants;
using rethus_backend.Utilities.Constants.PaginatioConstants;
using rethus_backend.Utilities.Constants.User.UserConfiguration;

namespace rethus_backend.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserController : ApiBaseController
{
    public UserController(IServiceProvider provider)
        : base(provider) { }

    [HttpPost]
    public async Task<ActionResult<ApiResponse>> PostAsync([FromBody] CreateRequestDto _user)
    {
        bool user = _unitOfWork.User.IsUniqueUser(_user.email);

        if (!user)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Messages.Add("Username already exists");
            return BadRequest(_response);
        }

        User newUser = await _unitOfWork.User.Register(_user);
        if (newUser == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Messages.Add("Register Error");
            return BadRequest(_response);
        }

        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }

    [HttpPost("administrative")]
    [Authorize(Roles = Policies.SuperAdmin)]
    public async Task<ActionResult<ApiResponse>> PostUser(
        [FromBody] CreateUserAdministrativeRequestDto _user
    )
    {
        bool validRol = _unitOfWork.User.ValidateUserRole(_user.type);
        if (!validRol)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Messages.Add("Invalid user type");
            return BadRequest(_response);
        }

        bool user = _unitOfWork.User.IsUniqueUser(_user.email);

        if (!user)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Messages.Add("Username already exists");
            return BadRequest(_response);
        }

        User newUser = await _unitOfWork.User.RegisterUserAdministration(_user);
        if (newUser == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Messages.Add("Register Error");
            return BadRequest(_response);
        }

        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }

    [HttpPut("administrative/{id}")]
    [Authorize(Roles = Policies.SuperAdmin)]
    public async Task<ActionResult<ApiResponse>> UpdateUser(
        string id,
        [FromBody] UpdateUserAdministrativeRequestDto _user
    )
    {
        bool validRol = _unitOfWork.User.ValidateUserRole(_user.type);
        if (!validRol)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Messages.Add("Invalid user type");
            return BadRequest(_response);
        }

        User ExistUser = _unitOfWork.User.GetByUserId(id);

        if (ExistUser == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Messages.Add("User not exists");
            return BadRequest(_response);
        }

        User newUser = await _unitOfWork.User.UpdateUserAdministration(_user, ExistUser);
        if (newUser == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Messages.Add("Update Error");
            return BadRequest(_response);
        }

        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }

    [HttpDelete("administrative/inactive/{id}")]
    [Authorize(Roles = Policies.SuperAdmin)]
    public async Task<ActionResult<ApiResponse>> deleteUser(string id)
    {
        User user = _unitOfWork.User.GetByUserId(id);

        if (user == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Messages.Add("User not exists");
            return BadRequest(_response);
        }

        User newUser = await _unitOfWork.User.UpdateStatusUserAdministaration(
            user,
            UserStatus.inactive
        );

        if (newUser == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Messages.Add("Delete Error");
            return BadRequest(_response);
        }

        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }

    [HttpDelete("administrative/active/{id}")]
    [Authorize(Roles = Policies.SuperAdmin)]
    public async Task<ActionResult<ApiResponse>> activeUser(string id)
    {
        User user = _unitOfWork.User.GetByUserId(id);

        if (user == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Messages.Add("User not exists");
            return BadRequest(_response);
        }

        User newUser = await _unitOfWork.User.UpdateStatusUserAdministaration(
            user,
            UserStatus.active
        );

        if (newUser == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Messages.Add("Delete Error");
            return BadRequest(_response);
        }

        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }

    [HttpGet("administrative")]
    [Authorize(Roles = Policies.SuperAdmin)]
    public PaginationResultDto<UserDto> GetAllPaginado(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
    )
    {
        var request = new PaginationRequestDto<UserQueryParametersDto>
        {
            Page = page,
            PageSize = pageSize,
        };

        Func<User, UserDto> mapper = user =>
            new UserDto
            {
                UserId = user.UserId,
                name = user.name,
                email = user.email,
                Status = user.Status,
                Roles = user.roles,
                CreatedAt = user.CreatedAt
            };
        var result = _unitOfWork.User.GetPagination(request, mapper);

        return result;
    }

    [HttpGet("{id}")]
    public IActionResult GetById(string id)
    {
        var user = _unitOfWork.User.GetByUserId(id);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpGet("user-configuration/{id}")]
    public IActionResult GetUserConfigurationById(string id)
    {
        var user = _unitOfWork.UserConfiguration.GetById(id);
        if (user == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Messages.Add("La configuracion del usuario no existe");
        }

        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        _response.Result = user;
        return Ok(_response);
    }

    [HttpPut("user-configuration/step/{id}")]
    public IActionResult UpdateStepConfigurationById(
        string id,
        [FromBody] UpdateStepConfigurationDto step
    )
    {
        var user = _unitOfWork.UserConfiguration.updateStepConfiguration(id, step.step);
        if (user == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Messages.Add("Error al actualizar el step");
        }

        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }

    [HttpPut("user-configuration/select-process/{id}")]
    [Authorize(Roles = Policies.User)]
    public IActionResult UpdateTypeProcessConfigurationById(
        string id,
        [FromBody] UpdateTypeProcessConfigurationDto payload
    )
    {
        var typeProcedure = UserConfiguration.GetConfigurationType(payload.type);
        if (typeProcedure == ConfigurationTypeProcedure.DEFAULT)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Messages.Add("El tipo de tramite no es permitido");
            BadRequest(_response);
        }

        var user = _unitOfWork.UserConfiguration.updateTypeProcessConfiguration(id, typeProcedure);
        if (user == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Messages.Add("Error al actualizar el tipo de tramite");
            BadRequest(_response);
        }

        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        _response.Result = user.Result;
        return Ok(_response);
    }

    [HttpPut("user-configuration/term-conditions/{id}")]
    [Authorize(Roles = Policies.User)]
    public IActionResult UpdateTermConditionsConfigurationById(
        string id,
        [FromBody] UpdateTermConditionsConfigurationDto payload
    )
    {
        var user = _unitOfWork.UserConfiguration.updateTermConditionsConfiguration(
            id,
            payload.termCondition
        );
        if (user == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Messages.Add("Error al actualizar los terminos y condiciones");
        }

        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        _response.Result = user.Result;
        return Ok(_response);
    }
}
