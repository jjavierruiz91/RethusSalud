using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rethus_backend.Models;

namespace rethus_backend.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class LoginController : ApiBaseController
{


  public LoginController(IServiceProvider provider) : base(provider)
  {
  }

  [HttpGet]
  public ActionResult<List<UserResponseDto>> Get()
  {
    throw new NotImplementedException();
  }
}
