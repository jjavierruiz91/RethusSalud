using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rethus_backend.Models;
using rethus_backend.Models.Dto.UserForm;
using rethus_backend.Models.Dto.UserFormFiles;
using System.Net;

namespace rethus_backend.Controllers;


[ApiController]
[Route("[controller]")]
public class UserFormController : ApiBaseController
{


  public UserFormController(IServiceProvider provider) : base(provider) { }


  [HttpPost]
  public async Task<ActionResult<ApiResponse>> PostAsync([FromBody] UserFormCreateDto userForm)
  {
    ApiResponse createUser = await _unitOfWork.UserForm.post(userForm);

    if (createUser.IsSuccess == false)
    {
      return BadRequest(createUser);
    }

    var user_configuration = _unitOfWork.UserConfiguration.GetByUserId(userForm.userId);
    _unitOfWork.UserConfiguration.updateAutomaticStepConfiguration(user_configuration.ConfigurationsId);

    return Ok(createUser);
  }

  [HttpPost("load-files/{userFormId}")]
  public async Task<ActionResult<ApiResponse>> PostAsyncUserFormFiles(string userFormId, [FromBody] UserFormFilesCreateDto _files)
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
    if (user == null) return NotFound();

    return Ok(user);
  }
}
