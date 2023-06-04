using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rethus_backend.Models;
using rethus_backend.Models.Dto.UserForm;
using rethus_backend.Models.Dto.UserFormFiles;
using System.Net;

namespace rethus_backend.Controllers;


[ApiController]
[Route("[controller]")]
public class UserFormFilesController : ApiBaseController
{


  public UserFormFilesController(IServiceProvider provider) : base(provider) { }

  [HttpPost("{userId}")]
  public async Task<ActionResult<ApiResponse>> PostAsyncUserFormFiles(string userId, [FromForm] UserFormFilesCreateDto _files)
  {
    var response = _unitOfWork.UserFormFiles.RegisterUserFormFile(userId, _files);

    if (response.IsSuccess = false)
    {
      return BadRequest(response);
    }

    var user_configuration = _unitOfWork.UserConfiguration.GetByUserId(userId);
    _unitOfWork.UserConfiguration.updateAutomaticStepConfiguration(user_configuration.ConfigurationsId);

    return Ok(response);
  }

  [Authorize]
  [HttpGet]
  public ActionResult<List<UserForm>> GetAll()
  {
    var users = _unitOfWork.UserFormFiles.GetAll();
    return Ok(users);
  }

  // [Authorize]
  [HttpGet("{id}")]
  public IActionResult GetById(string id)
  {
    var user = _unitOfWork.UserFormFiles.GetById(id);
    if (user == null) return NotFound();

    return Ok(user);
  }

}