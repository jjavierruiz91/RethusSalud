using System.Net;
using Microsoft.AspNetCore.Mvc;
using rethus_backend.Controllers;
using rethus_backend.Models;

[ApiController]
[Route("[controller]")]
public class DepartmentController : ApiBaseController
{
    public DepartmentController(IServiceProvider provider)
        : base(provider) { }

    [HttpGet("{countryId}")]
    public async Task<ActionResult<ApiResponse>> GetDeparment(int countryId)
    {
        var departments = await _unitOfWork.Department.GetDepartments(countryId);

        if (departments == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            BadRequest(_response);
        }

        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        _response.Result = departments;

        Response.Headers["Cache-Control"] = "public,max-age=86400"; // 86400 segundos = 24 horas

        return Ok(_response);
    }
}
