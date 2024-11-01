using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Models.Dto.UserFormFiles;

namespace rethus_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class UserFormFilesController : ApiBaseController
{
    private readonly IServiceProvider _provider;

    public UserFormFilesController(IServiceProvider provider)
        : base(provider)
    {
        _provider = provider;
    }

    [HttpPost("{userId}")]
    [Authorize(Roles = Policies.User)]
    public async Task<ActionResult<ApiResponse>> PostAsyncUserFormFiles(
        string userId,
        [FromForm] UserFormFilesCreateDto _files
    )
    {
        var configuration = _unitOfWork.UserConfiguration.ValidateStepConfiguration(
            userId,
            ConfigurationStep.load_user_form
        );
        if (!configuration.IsSuccess)
        {
            _response.IsSuccess = false;
            _response.Messages = configuration.Messages;
            _response.StatusCode = HttpStatusCode.Conflict;
            return BadRequest(_response);
        }

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

    [HttpPatch("generate")]
    [Authorize(Roles = Policies.Inventory)]
    public async Task<ActionResult<ApiResponse>> PatchGenerateCertificate(
        [FromBody] ConfigGenerateCerticateDto configs
    )
    {
        _response.StatusCode = HttpStatusCode.OK;
        _response.IsSuccess = true;

        if (configs.ConsecutiveStart == 0)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.Messages.Add("ConsecutiveStart no puede ser cero.");
        }

        if (configs.ConsecutiveEnd == 0)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.Messages.Add("ConsecutiveEnd no puede ser cero.");
        }

        if (string.IsNullOrEmpty(configs.ConsecutiveDate))
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.Messages.Add("ConsecutiveDate no puede estar vacío.");
        }

        if (!_response.IsSuccess)
        {
            return BadRequest(_response);
        }

        await Task.Run(async () =>
        {
            using (var scope = _provider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                await _unitOfWork.UserForm.ProcessUserFormCertificatesByBatch(
                    500,
                    100,
                    configs.ConsecutiveStart,
                    configs.ConsecutiveEnd,
                    configs.ConsecutiveDate,
                    dbContext
                );
            }
        });

        _response.Messages.Add("Certificados generandoce");
        return _response;
    }
}
