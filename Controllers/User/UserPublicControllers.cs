using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rethus_backend.Models;
using rethus_backend.Models.Dto.User;
using rethus_backend.Models.Dto.Configuration;
using System.Net;

namespace rethus_backend.Controllers;


// [Authorize(Policy = "PublicPolicy")]
[ApiController]
[Route("[controller]")]
public class UserPublicController : ApiBaseController
{

 public UserPublicController(IServiceProvider provider) : base(provider) { }


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
}
