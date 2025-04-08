using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rethus_backend.Controllers;
using rethus_backend.Models;
using rethus_backend.Models.Dto.User;
using rethus_backend.Utilities.FileHelper;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserDigitalSignatureController : ApiBaseController
{
    private readonly IConfiguration _config;

    public UserDigitalSignatureController(IServiceProvider provider, IConfiguration config)
        : base(provider)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    // Obtener la firma de un usuario
    [HttpGet("{userId}")]
    [Authorize(Roles = Policies.SuperAdmin)]
    public async Task<IActionResult> GetByUserId(string userId)
    {
        var signature = await _unitOfWork.UserDigitalSignature.GetByUserIdAsync(userId);
        if (signature == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.Messages.Add("No se encontró la firma del usuario");
            return BadRequest(_response);
        }

        _response.IsSuccess = true;

        var mapperSignature = new UserDigitalSignature
        {
            UserDigitalSignatureId = signature.UserDigitalSignatureId,
            SignatureName = signature.SignatureName,
            SignatureType = signature.SignatureType,
            Status = signature.Status,
            SignaturePositionType = signature.SignaturePositionType,
        };

        _response.Result = mapperSignature;
        return Ok(_response);
    }

    [HttpPost("{userId}")]
    [Authorize(Roles = Policies.SuperAdmin)]
    public async Task<ActionResult<ApiResponse>> post(
        string userId,
        [FromForm] CreateUserDigitalSignatureDto payload
    )
    {
        if (
            payload.SignatureImage.ContentType != "image/png"
            && payload.SignatureImage.ContentType != "image/jpeg"
        )
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Messages.Add(
                "El formato de la imagen no es válido solo se aceptan imágenes en formato PNG o JPEG"
            );
            return BadRequest(_response);
        }

        var user = _unitOfWork.User.GetByUserId(userId);
        if (user == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.Messages.Add("No se encontró el usuario");
            return BadRequest(_response);
        }

        var isSignatureInUse = await _unitOfWork.UserDigitalSignature.IsSignatureInUseAsync(
            payload.SignatureType,
            SignatureStatus.Active
        );
        if (isSignatureInUse)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.Conflict;
            _response.Messages.Add("La firma ya está siendo utilizada");
            return BadRequest(_response);
        }

        if (
            !user.roles.Contains(Policies.FuncionarioEtapa1)
            && !user.roles.Contains(Policies.FuncionarioEtapa2)
            && !user.roles.Contains(Policies.FuncionarioEtapa3)
            && !user.roles.Contains(Policies.Admin)
        )
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.Forbidden;
            _response.Messages.Add("El usuario no cumple los requisitos para tener una firma");
            return BadRequest(_response);
        }

        var userSignature = await _unitOfWork.UserDigitalSignature.GetByUserIdAsync(userId);
        if (userSignature != null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.Conflict;
            _response.Messages.Add("El usuario ya tiene una firma");
            return BadRequest(_response);
        }

        var ruta = _config.GetSection("routeUserDigitalSignature").Value + userId;

        FileHelper.CreateFolder(ruta);

        var baseUrlFile = FileHelper.AddAsync(payload.SignatureImage, ruta);

        var signature = new UserDigitalSignature
        {
            UserId = userId,
            SignatureName = payload.SignatureName,
            SignatureType = payload.SignatureType,
            SignatureImagePath = baseUrlFile,
            SignaturePositionType = payload.SignaturePositionType,
        };

        await _unitOfWork.UserDigitalSignature.AddAsync(signature);

        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        _response.Messages.Add("Firma creada con éxito");
        return Ok(_response);
    }

    [HttpGet("image/{signatureId}")]
    [Authorize(Roles = Policies.SuperAdmin)]
    public async Task<IActionResult> GetSignatureImage(string signatureId)
    {
        var signature = await _unitOfWork.UserDigitalSignature.GetByIdAsync(signatureId);
        if (signature == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.Messages.Add("No se encontró la firma digital");
            return NotFound(_response);
        }

        var imagePath = signature.SignatureImagePath;
        if (!FileHelper.ValidatePath(imagePath))
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.Messages.Add("No se encontró la imagen de la firma");
            return NotFound(_response);
        }

        var excelBytes = System.IO.File.ReadAllBytes(imagePath);
        return File(excelBytes, "image/jpg");
    }

    [HttpPut("{userDigitalSignatureId}/user/{userId}")]
    [Authorize(Roles = Policies.SuperAdmin)]
    public async Task<ActionResult<ApiResponse>> update(
        string userDigitalSignatureId,
        string userId,
        [FromForm] UpdateUserDigitalSignatureDto payload
    )
    {
        if (
            payload.SignatureImage != null
            && payload.SignatureImage.ContentType != "image/png"
            && payload.SignatureImage.ContentType != "image/jpeg"
        )
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Messages.Add(
                "El formato de la imagen no es válido solo se aceptan imágenes en formato PNG o JPEG"
            );
            return BadRequest(_response);
        }

        var userSignature = await _unitOfWork.UserDigitalSignature.GetByUserIdAsync(userId);
        if (userSignature == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.Conflict;
            _response.Messages.Add("Este usuario no tiene firma");
            return BadRequest(_response);
        }

        var isSignatureInUse = await _unitOfWork.UserDigitalSignature.IsSignatureInUseAsync(
            payload.SignatureType,
            SignatureStatus.Active
        );
        if (isSignatureInUse && payload.SignatureType != userSignature.SignatureType)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.Conflict;
            _response.Messages.Add(
                "Este tipo de firma que quiere actualizar esta siendo utilizada por otro usuario"
            );
            return BadRequest(_response);
        }

        var isSignatureInUseByUser =
            await _unitOfWork.UserDigitalSignature.IsSignatureInUseByUserIdAsync(
                userId,
                userDigitalSignatureId
            );
        if (!isSignatureInUseByUser)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.Conflict;
            _response.Messages.Add("Esta firma que quiere actualizar no pertenece a este usuario");
            return BadRequest(_response);
        }

        var user = _unitOfWork.User.GetByUserId(userId);
        if (user == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.Messages.Add("No se encontró el usuario");
            return BadRequest(_response);
        }

        if (
            !user.roles.Contains(Policies.FuncionarioEtapa1)
            && !user.roles.Contains(Policies.FuncionarioEtapa2)
            && !user.roles.Contains(Policies.FuncionarioEtapa3)
            && !user.roles.Contains(Policies.Admin)
        )
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.Forbidden;
            _response.Messages.Add("El usuario no cumple los requisitos para tener una firma");
            return BadRequest(_response);
        }

        if (payload.SignatureImage != null)
        {
            var ruta = _config.GetSection("routeUserDigitalSignature").Value + userId;
            FileHelper.CreateFolder(ruta);

            var baseUrlFile = FileHelper.AddAsync(payload.SignatureImage, ruta);

            userSignature.SignatureImagePath = baseUrlFile;
        }

        userSignature.SignatureName = payload.SignatureName;
        userSignature.SignatureType = payload.SignatureType;
        userSignature.Status = payload.SignatureStatus;
        userSignature.SignaturePositionType = payload.SignaturePositionType;

        await _unitOfWork.UserDigitalSignature.UpdateAsync(userSignature);

        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        _response.Messages.Add("Firma creada con éxito");
        return Ok(_response);
    }
}
