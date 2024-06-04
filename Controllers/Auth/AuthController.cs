using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rethus_backend.Models;
using rethus_backend.Models.Dto.Auth;
using System.Net;

namespace rethus_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ApiBaseController
{
    public LoginController(IServiceProvider provider)
        : base(provider) { }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<AuthResponseDto>> Authenticate([FromBody] AuthRequestDto _user)
    {
        bool user = _unitOfWork.User.IsExistUser(_user.email);

        if (!user)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Messages.Add("Username not exist");
            return BadRequest(_response);
        }

        bool userActive = _unitOfWork.User.IsUserActive(_user.email);

        if (!userActive)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Messages.Add("El usuario no esta activo, comuniquese con el administrador");
            return BadRequest(_response);
        }

        AuthResponseDto userAuthenticate = await _unitOfWork.Auth.Authenticate(_user);
        if (string.IsNullOrEmpty(userAuthenticate.token))
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Messages.Add("Username or password is error");
            return BadRequest(_response);
        }

        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        _response.Result = userAuthenticate;
        return Ok(_response);
    }

    [AllowAnonymous]
    [HttpGet]
    public ActionResult<List<UserResponseDto>> Get()
    {
        throw new NotImplementedException();
    }
}
