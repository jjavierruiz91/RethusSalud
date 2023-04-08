using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rethus_backend.Models;
using rethus_backend.Models.Dto.User;
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

  // [Authorize]
  [HttpGet]
  public ActionResult<List<UserResponseDto>> GetAll()
  {
    var users = _unitOfWork.User.GetAll();
    return Ok(users);
  }

  [HttpGet("{id}")]
  public IActionResult GetById(string id)
  {
    var user = _unitOfWork.User.GetById(id);
    if (user == null) return NotFound();

    return Ok(user);
  }
}
