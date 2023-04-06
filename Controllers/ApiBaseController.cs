
using Microsoft.AspNetCore.Mvc;
using rethus_backend.Models;
using rethus_backend.Repository.IRepository;
using Microsoft.AspNetCore.Http;

namespace rethus_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class ApiBaseController : ControllerBase
{
  protected readonly IUnitOfWork _unitOfWork = null!;
  protected readonly ApiResponse _response;

  public ApiBaseController(IServiceProvider provider)
  {
    _response = new ApiResponse();
  }
}
