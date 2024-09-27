using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rethus_backend.Models;
using rethus_backend.Models.Dto.UserFormFiles;

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
        var response = await _unitOfWork.UserFormFiles.RegisterUserFormFileAsync(userId, _files);

        if (!response.IsSuccess)
        {
            BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPut("{userFormId}")]
    [Authorize(
        Roles = Policies.FuncionarioEtapa1
            + ","
            + Policies.FuncionarioEtapa2
            + ","
            + Policies.FuncionarioEtapa3
            + ","
            + Policies.Inventory
            + ","
            + Policies.User
    )]
    public async Task<ActionResult<ApiResponse>> PutAsyncUserFormFiles(
        string userFormId,
        [FromForm] UserFormFilesUpdateDto _files
    )
    {
        ApiResponse response = await _unitOfWork.UserFormFiles.UpdateUserFormFileAsync(
            userFormId,
            _files
        );

        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [Authorize(Roles = Policies.User)]
    [HttpGet]
    public ActionResult<List<UserForm>> GetAll()
    {
        var users = _unitOfWork.UserFormFiles.GetAll();
        return Ok(users);
    }

    [Authorize(
        Roles = Policies.FuncionarioEtapa1
            + ","
            + Policies.FuncionarioEtapa2
            + ","
            + Policies.FuncionarioEtapa3
            + ","
            + Policies.Inventory
            + ","
            + Policies.User
    )]
    [HttpGet("files/{id}")]
    public async Task<List<byte[]>> GetFilesByUserFomrId(string id)
    {
        var user = await _unitOfWork.UserFormFiles.GetFilesByUserFormId(id);

        return user;
    }

    [Authorize(
        Roles = Policies.FuncionarioEtapa1
            + ","
            + Policies.FuncionarioEtapa2
            + ","
            + Policies.FuncionarioEtapa3
            + ","
            + Policies.Inventory
            + ","
            + Policies.User
    )]
    [HttpGet("view-file/{id}")]
    public async Task<UserFormFileDetails> GetFileByUserFomrId(string id)
    {
        var detailsFile = await _unitOfWork.UserFormFiles.GetFileByUserFormId(id);
        return detailsFile;
    }

    [Authorize(
        Roles = Policies.FuncionarioEtapa1
            + ","
            + Policies.FuncionarioEtapa2
            + ","
            + Policies.FuncionarioEtapa3
            + ","
            + Policies.Inventory
            + ","
            + Policies.User
    )]
    [HttpGet("{id}")]
    public ActionResult GetUserFomrId(string id)
    {
        var user = _unitOfWork.UserFormFiles.GetUserFormId(id);
        if (user == null)
            return NotFound();

        return Ok(user);
    }
}
