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
    public UserFormFilesController(IServiceProvider provider)
        : base(provider) { }

    [HttpPost("{userId}")]
    [Authorize(Roles = Policies.User)]
    public async Task<ActionResult<ApiResponse>> PostAsyncUserFormFiles(
        string userId,
        [FromForm] UserFormFilesCreateDto _files
    )
    {
        var response = _unitOfWork.UserFormFiles.RegisterUserFormFile(userId, _files);

        if (response.IsSuccess = false)
        {
            return BadRequest(response);
        }

        var user_configuration = _unitOfWork.UserConfiguration.GetByUserId(userId);
        _unitOfWork.UserConfiguration.updateAutomaticStepConfiguration(
            user_configuration.ConfigurationsId
        );

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
    [HttpGet("files/{id}")]
    public async Task<List<byte[]>> GetFilesByUserFomrId(string id)
    {
        var user = await _unitOfWork.UserFormFiles.GetFilesByUserFormId(id);

        return user;
    }

    [HttpGet("view-file/{id}")]
    public async Task<UserFormFileDetails> GetFileByUserFomrId(string id)
    {
        var detailsFile = await _unitOfWork.UserFormFiles.GetFileByUserFormId(id);
        return detailsFile;
    }

    [HttpGet("{id}")]
    public ActionResult GetUserFomrId(string id)
    {
        var user = _unitOfWork.UserFormFiles.GetUserFormId(id);
        if (user == null)
            return NotFound();

        return Ok(user);
    }
}
