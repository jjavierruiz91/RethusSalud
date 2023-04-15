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


  public LoginController(IServiceProvider provider) : base(provider)
  {
  }

  [HttpPost]
  public async Task<ActionResult<AuthResponseDto>> Authenticate([FromBody] AuthRequestDto _user)
  {
    bool user = _unitOfWork.User.IsExistUser(_user.email);

    if (!user)
    {
      return BadRequest(new { message = "Username or password is incorrect" });
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


  [HttpGet]
  public ActionResult<List<UserResponseDto>> Get()
  {
    throw new NotImplementedException();
  }
}
