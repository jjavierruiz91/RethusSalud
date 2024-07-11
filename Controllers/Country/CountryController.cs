using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace rethus_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class CountryController : ApiBaseController
{
    public CountryController(IServiceProvider provider)
        : base(provider) { }

    [HttpGet]
    public ActionResult LoadCountries()
    {
        _unitOfWork.Country.LoadCountries();
        return Ok();
    }
}
