using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rethus_backend.Models;
using rethus_backend.Models.Dto.User;
using rethus_backend.Models.Dto.Configuration;
using System.Net;
using rethus_backend.Models.Dto.UserPublic;
using rethus_backend.Utilities.Security.Hashing;

namespace rethus_backend.Controllers;

// [Authorize(Policy = "PublicPolicy")]
[ApiController]
[Route("[controller]")]
public class UserPublicController : ApiBaseController
{
    public UserPublicController(IServiceProvider provider)
        : base(provider) { }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<ApiResponse>> PostAsync([FromBody] CreateRequestDto _user)
    {
        if (_user == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Messages.Add("El usuario no puede ser nulo");
            return BadRequest(_response);
        }

        bool isValid = Password.ValidatePassword(_user.password, _user.confirmPassword);

        if (!isValid)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Messages.Add("La contraseña no cumple con los requisitos de seguridad");
            return BadRequest(_response);
        }

        bool IsExistIdentification = _unitOfWork.User.IsExistIdentification(_user.identification);

        if (IsExistIdentification)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Messages.Add(
                "Algo salió mal. Por favor, verifica tus credenciales e inténtalo de nuevo."
            );
            return BadRequest(_response);
        }

        User newUser = await _unitOfWork.User.Register(_user);
        if (newUser == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Messages.Add("Algo salió mal. Por favor, el proceso de guardado fallo!");
            return BadRequest(_response);
        }

        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }

    [AllowAnonymous]
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

    [AllowAnonymous]
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

    [AllowAnonymous]
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
}
