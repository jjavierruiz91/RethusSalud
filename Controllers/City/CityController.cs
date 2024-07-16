using System.Net;
using Microsoft.AspNetCore.Mvc;
using rethus_backend.Controllers;
using rethus_backend.Models;

[ApiController]
[Route("[controller]")]
public class CityController : ApiBaseController
{
    public CityController(IServiceProvider provider)
        : base(provider) { }

    [HttpGet("{departmentId}")]
    public async Task<ActionResult<ApiResponse>> GetCities(int departmentId)
    {
        var cities = await _unitOfWork.City.GetCities(departmentId);

        if (cities == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            BadRequest(_response);
        }

        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        _response.Result = cities;

        Response.Headers["Cache-Control"] = "public,max-age=86400"; // 86400 segundos = 24 horas

        return Ok(_response);
    }
}
