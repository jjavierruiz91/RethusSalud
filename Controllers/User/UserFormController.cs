using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rethus_backend.Models;
using rethus_backend.Models.Dto.UserForm;
using System.Net;

namespace rethus_backend.Controllers;


[ApiController]
[Route("[controller]")]
public class UserFormController : ApiBaseController
{


  public UserFormController(IServiceProvider provider) : base(provider) { }


  [HttpPost]
  public async Task<ActionResult<ApiResponse>> PostAsync([FromBody] UserFormCreateDto _user)
  {
    bool user = _unitOfWork.UserForm.IsUniqueUser(_user.PersonalEmail);

    if (!user)
    {
      _response.IsSuccess = false;
      _response.StatusCode = HttpStatusCode.BadRequest;
      _response.Messages.Add("Email already exists");
      return BadRequest(_response);
    }

    ApiResponse newUser = await _unitOfWork.UserForm.post(_user);
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
  public ActionResult<List<UserForm>> GetAll()
  {
    var users = _unitOfWork.UserForm.GetAll();
    return Ok(users);
  }

  [Authorize]
  [HttpGet("{id}")]
  public IActionResult GetById(string id)
  {
    var user = _unitOfWork.UserForm.GetById(id);
    if (user == null) return NotFound();

    return Ok(user);
  }
}
