using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rethus_backend.Models;
using rethus_backend.Models.Dto.User;
using rethus_backend.Models.Dto.Configuration;
using System.Net;

namespace rethus_backend.Controllers;


[ApiController]
[Route("[controller]")]
public class UserController : ApiBaseController
{


  public UserController(IServiceProvider provider) : base(provider) { }


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

  [Authorize]
  [HttpGet]
  public ActionResult<List<UserResponseDto>> GetAll()
  {
    var users = _unitOfWork.User.GetAll();
    return Ok(users);
  }

  [Authorize]
  [HttpGet("{id}")]
  public IActionResult GetById(string id)
  {
    var user = _unitOfWork.User.GetById(id);
    if (user == null) return NotFound();

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
  public IActionResult UpdateStepConfigurationById(string id, [FromBody] UpdateStepConfigurationDto step)
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
  public IActionResult UpdateTypeProcessConfigurationById(string id, [FromBody] UpdateTypeProcessConfigurationDto payload)
  {
    var user = _unitOfWork.UserConfiguration.updateTypeProcessConfiguration(id, payload.type);
    if (user == null)
    {
      _response.IsSuccess = false;
      _response.StatusCode = HttpStatusCode.BadRequest;
      _response.Messages.Add("Error al actualizar el tipo de tramite");
    }

    _response.IsSuccess = true;
    _response.StatusCode = HttpStatusCode.OK;
    _response.Result = user.Result;
    return Ok(_response);
  }

  [HttpPut("user-configuration/term-conditions/{id}")]
  public IActionResult UpdateTermConditionsConfigurationById(string id, [FromBody] UpdateTermConditionsConfigurationDto payload)
  {
    var user = _unitOfWork.UserConfiguration.updateTermConditionsConfiguration(id, payload.termCondition);
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
