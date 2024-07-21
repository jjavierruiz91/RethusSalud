using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rethus_backend.Models;

namespace rethus_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class CountryController : ApiBaseController
{
    public CountryController(IServiceProvider provider)
        : base(provider) { }

    [HttpGet("load")]
    public ActionResult LoadCountries()
    {
        _unitOfWork.Country.LoadCountriesJsonToBd();
        _unitOfWork.Department.LoadDeparmentJsonToBd();
        _unitOfWork.City.LoadCityJsonToBd();

        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetCountries()
    {
        var countries = await _unitOfWork.Country.GetCountries();

        if (countries == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            BadRequest(_response);
        }

        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        _response.Result = countries;

        Response.Headers["Cache-Control"] = "public,max-age=86400"; // 86400 segundos = 24 horas

        return Ok(_response);
    }

    [HttpGet("{countryId}")]
    public async Task<ActionResult<ApiResponse>> GetCities(int countryId)
    {
        var cities = await _unitOfWork.Country.GetCountryId(countryId);

        if (cities == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            BadRequest(_response);
        }

        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        _response.Result = cities;

        Response.Headers["Cache-Control"] = "public,max-age=86400";

        return Ok(_response);
    }
}
