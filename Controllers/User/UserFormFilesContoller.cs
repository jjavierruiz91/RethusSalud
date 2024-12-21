using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Models.Dto.UserFormFiles;
using rethus_backend.Utilities.FileHelper;

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
            ConfigurationStep.load_user_files
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

        if (string.IsNullOrEmpty(configs.ConsecutiveStart))
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.Messages.Add("ConsecutiveStart es requerido");
        }

        if (string.IsNullOrEmpty(configs.ConsecutiveEnd))
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.Messages.Add("ConsecutiveEnd es requerido.");
        }

        if (string.IsNullOrEmpty(configs.ConsecutiveDate))
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.Messages.Add("ConsecutiveDate no puede estar vacío.");
        }

        bool existsConsecutive = _unitOfWork.UserForm.ExistFormWithRangeConsecutive(
            configs.ConsecutiveStart,
            configs.ConsecutiveEnd
        );

        if (existsConsecutive)
        {
            _response.StatusCode = HttpStatusCode.Conflict;
            _response.IsSuccess = false;
            _response.Messages.Add("Existe un formulario en este rango de consecutivo");
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
                    300,
                    1,
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

    [HttpGet("donwload/zip")]
    public IActionResult DownloadZip([FromQuery] string target)
    {
        if (string.IsNullOrEmpty(target))
        {
            _response.AddError("Error al descargar el archivo", HttpStatusCode.BadRequest, false);
            return BadRequest(_response);
        }

        string filePath = FileHelper.GetPathZip(target);

        if (!System.IO.File.Exists(filePath))
        {
            _response.AddError("El archivo no se encuentra", HttpStatusCode.BadRequest, false);
            return NotFound(_response);
        }

        // Si todo está bien, devolvemos el archivo como un archivo zip.
        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        return File(fileBytes, "application/zip", Path.GetFileName(filePath));
    }
}
