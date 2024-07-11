using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rethus_backend.Models;
using rethus_backend.Models.Dto.User;
using rethus_backend.Models.Dto.Configuration;
using System.Net;
using rethus_backend.Models.Dto.UserPublic;

namespace rethus_backend.Controllers;

// [Authorize(Policy = "PublicPolicy")]
[ApiController]
[Route("[controller]")]
public class UserPublicController : ApiBaseController
{
    public UserPublicController(IServiceProvider provider)
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

    [HttpPost("process")]
    public IActionResult GetStateProcess([FromBody] UserFormProcessDto identification)
    {
        var user = _unitOfWork.UserForm.GetDetailProcess(identification);
        if (user == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.Messages.Add("La informacion no fue encontrada");
            return NotFound(_response);
        }

        _response.Result = user;
        return Ok(_response);
    }

    [HttpPost("restore")]
    public async Task<IActionResult> restorePassword([FromBody] UserRestorePassword payload)
    {
        var user = _unitOfWork.User.GetUserByEmail(payload.email);
        if (user == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.Messages.Add("El usuario con ese correo no fue encontrado");
            return NotFound(_response);
        }

        var userTokenTemp = _unitOfWork.Auth.generateJwtTokenTemp(user.UserId);

        _unitOfWork.User.UpdateTokenUser(user.UserId, userTokenTemp);

        RestoreSendEmailUser configTemplateDto = new RestoreSendEmailUser
        {
            Token = userTokenTemp,
            Email = payload.email
        };

        var restorePassword = _unitOfWork.User.restorePassword(configTemplateDto);

        return Ok(_response);
    }

    [HttpPut("update")]
    public async Task<IActionResult> newPassword([FromBody] UserUpdatePassword payload)
    {
        var isValideToken = _unitOfWork.Auth.validateJwtTokenTmep(payload.token);
        if (!isValideToken)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.Messages.Add(
                "Ocurrio un error al actualizar la contrasena contactese con soporte"
            );
            return NotFound(_response);
        }

        var user = _unitOfWork.User.GetUserByToken(payload.token);
        if (user == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.Messages.Add(
                "Ocurrio un error al actualizar la contrasena contactese con soporte"
            );
            return NotFound(_response);
        }

        UserPayloadPassword newPayload = new UserPayloadPassword
        {
            newPassword = payload.password,
            UserId = user.UserId
        };

        var restorePassword = await _unitOfWork.User.updatePassword(newPayload);
        if (!restorePassword)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.Messages.Add(
                "Ocurrio un error al actualizar la contrasena contactese con soporte"
            );
            return BadRequest(_response);
        }

        return Ok(_response);
    }

    // [HttpGet("countries")]
    // public IActionResult GetContries()
    // {
    //     return null;
    // }
}
